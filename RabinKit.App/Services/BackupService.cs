using CommunityToolkit.Maui.Storage;
using RabinKit.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.IO;

namespace RabinKit.App.Services;

public class BackupService
{
    private readonly EfContext _context;
    private readonly NavigationManager _manager;
    private readonly IServiceScopeFactory _factory;
    private readonly ExceptionHandler _exceptionHandler;
    //TODO сделать относительный путь
   // private readonly string filePath = Path.Combine(Environment.CurrentDirectory, "encryptionKeys.txt");

    private readonly string DbFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3");
    private readonly string BackupFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3.backup");
    private readonly string RestoreFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3.restore");

    private static readonly byte[] Key = Convert.FromBase64String(Keys.Key);
    private static readonly byte[] IV = Convert.FromBase64String(Keys.IV);
    
    public BackupService(
        EfContext context,
        NavigationManager manager,
        IServiceScopeFactory factory,
        ExceptionHandler exceptionHandler)
    {
        _context = context;
        _manager = manager;
        _factory = factory;
        _exceptionHandler = exceptionHandler;
    }

    public async Task ExportAsync()
    {
       // LoadKeysFromFile(filePath);
        await _exceptionHandler.HandleAsync(
            async () =>
            {
                await _context.Database.ExecuteSqlRawAsync("VACUUM;");
                await _context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(TRUNCATE)");
                await ReleaseAsync();

                File.Copy(DbFileName, BackupFileName, overwrite: true);

                await using var stream = File.OpenRead(BackupFileName);

                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var dataToEncrypt = ms.ToArray();

                                // Шифрование данных
                var encryptedData = Encrypt(dataToEncrypt);
                var encstream = new MemoryStream(encryptedData);
                // Сохранение зашифрованного файла
                var result = await FileSaver.SaveAsync("data.backup", encstream);
            },
            catchAll: true,
            finallyFunc: () => _manager.Refresh(true));
    }


    public async Task ImportAsync(Stream fileContent)
    {
      //  LoadKeysFromFile(filePath);
        await _exceptionHandler.HandleAsync(
            async () =>
            {
                File.Delete(BackupFileName);
                File.Copy(DbFileName, BackupFileName, overwrite: true);

                await _context.Database.EnsureDeletedAsync();
                await ReleaseAsync();

                // Чтение зашифрованного файла
                using var ms = new MemoryStream();
                await fileContent.CopyToAsync(ms);
                var encryptedData = ms.ToArray();

                // Расшифрование данных
                var decryptedData = Decrypt(encryptedData);
                if (decryptedData == null || decryptedData.Length == 0)
                {
                    throw new InvalidOperationException("Расшифрованные данные пусты или недействительны.");
                }

                await using (var dbFileStream = File.OpenWrite(DbFileName))
                {
                    await dbFileStream.WriteAsync(decryptedData);
                    await dbFileStream.FlushAsync(); // Обязательно сбросьте буфер
                }

                await using var scope = _factory.CreateAsyncScope();
                _ = await scope.ServiceProvider.GetRequiredService<EfContext>().Database
                    .GetPendingMigrationsAsync();
                await scope.ServiceProvider.GetRequiredService<DbMigrator>().MigrateAsync();
            },
            catchFunc: () =>
            {
                if (File.Exists(BackupFileName))
                    File.Move(BackupFileName, DbFileName, overwrite: true);
            },
            finallyFunc: () => _manager.NavigateTo("/", true),
            catchAll: true);
    }


    public async Task ResetAsync()
    {
        await _exceptionHandler.HandleAsync(async () =>
        {
            await ReleaseAsync();

            File.Copy(DbFileName, BackupFileName, overwrite: true);

            await using (var scope = _factory.CreateAsyncScope())
            {
                await scope.ServiceProvider.GetRequiredService<EfContext>().Database.EnsureDeletedAsync();
                await scope.ServiceProvider.GetRequiredService<DbMigrator>().MigrateAsync();
            }

            _manager.NavigateTo("/", true);
        });
    }

    private async Task ReleaseAsync()
    {
        var connection = _context.Database.GetDbConnection();
        await connection.CloseAsync();
        await connection.DisposeAsync();
        await _context.DisposeAsync();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private byte[] Encrypt(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    private byte[] Decrypt(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(data);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        cs.CopyTo(resultStream);
        return resultStream.ToArray();
    }
}