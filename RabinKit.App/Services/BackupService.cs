using CommunityToolkit.Maui.Storage;
using RabinKit.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace RabinKit.App.Services;

public class BackupService
{
    private readonly EfContext _context;
    private readonly NavigationManager _manager;
    private readonly IServiceScopeFactory _factory;
    private readonly ExceptionHandler _exceptionHandler;

    private readonly string DbFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3");
    private readonly string BackupFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3.backup");
    private readonly string RestoreFileName = Path.Combine(
        FileSystem.AppDataDirectory,
        "Database.db3.restore");

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

    //public async Task ExportAsync()
    //{
    //    await _exceptionHandler.HandleAsync(
    //        async () =>
    //        {
    //            await _context.Database.ExecuteSqlRawAsync("VACUUM;");
    //            await _context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(TRUNCATE)");
    //            await ReleaseAsync();

    //            File.Copy(DbFileName, BackupFileName, overwrite: true);

    //            await using var stream = File.OpenRead(BackupFileName);

    //            var result = await FileSaver.SaveAsync("data.backup", stream);
    //        },
    //        catchAll: true,
    //        finallyFunc: () => _manager.Refresh(true));
    //}

    //public async Task ImportAsync(Stream fileContent)
    //{
    //    await _exceptionHandler.HandleAsync(
    //        async () =>
    //        {
    //            File.Delete(BackupFileName);
    //            File.Copy(DbFileName, BackupFileName, overwrite: true);

    //            await _context.Database.EnsureDeletedAsync();
    //            await ReleaseAsync();

    //            await using (var dbFileStream = File.OpenWrite(DbFileName))
    //                await fileContent.CopyToAsync(dbFileStream);

    //            await using var scope = _factory.CreateAsyncScope();

    //            _ = await scope.ServiceProvider.GetRequiredService<EfContext>().Database
    //                .GetPendingMigrationsAsync();
    //            await scope.ServiceProvider.GetRequiredService<DbMigrator>().MigrateAsync();
    //        },
    //        catchFunc: () =>
    //        {
    //            if (File.Exists(BackupFileName))
    //                File.Move(BackupFileName, DbFileName, overwrite: true);
    //        },
    //        finallyFunc: () => _manager.NavigateTo("/", true),
    //        catchAll: true);
    //}

    //public async Task ResetAsync()
    //{
    //    await _exceptionHandler.HandleAsync(async () =>
    //    {
    //        await ReleaseAsync();

    //        File.Copy(DbFileName, BackupFileName, overwrite: true);

    //        await using (var scope = _factory.CreateAsyncScope())
    //        {
    //            await scope.ServiceProvider.GetRequiredService<EfContext>().Database.EnsureDeletedAsync();
    //            await scope.ServiceProvider.GetRequiredService<DbMigrator>().MigrateAsync();
    //        }

    //        _manager.NavigateTo("/", true);
    //    });
    //}

    private async Task ReleaseAsync()
    {
        var connection = _context.Database.GetDbConnection();
        await connection.CloseAsync();
        await connection.DisposeAsync();
        await _context.DisposeAsync();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}