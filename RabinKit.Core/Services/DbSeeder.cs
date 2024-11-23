using RabinKit.Core.Abstractions;
using RabinKit.Core.Entities;
using RabinKit.Core.Extensions;
using RabinKit.Core.Components;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RabinKit.Core.Services;

public class DbSeeder : IDbSeeder
{
    private static readonly Assembly Assembly = Assembly.GetAssembly(typeof(DbSeeder))!;
    private readonly IDbContext _dbContext;

    public DbSeeder(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var counter = new AutoCounter<long>(1);

        foreach (var task in FirstModuleParameters.Tasks)
        {
            await InsertTaskAsync(
                task.module,
                task.number,
                task.name,
                task.inputVars,
                task.outputVars,
                task.toolboxFileName,
                counter,
                task.testParams,
                task.istest
            );
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task InsertTaskAsync(
        int module,
        int number,
        string name,
        string[] inputVars,
        string[] outputVars,
        string toolboxFileName,
        //long testIdCounter,
        AutoCounter<long> testIdCounter,
        List<ModuleSetExtension.TestValuesSet> testParams,
        bool istest = false)//,
      //  List<PerformanceTest>? performanceTests = null)
    {
        var id = Convert.ToInt64(module * 10 + number);
        var task = new TaskComponent
        {
            Id = id,
           // Number = numbers,
            Name = name,
            Description = await new StreamReader(
                    GetStream($"RabinKit.Core.Components.Descriptions.{id}.md"))
                .ReadToEndAsync(),
            Input = inputVars,
            Output = outputVars,
            Toolbox = await LoadToolboxAsync(toolboxFileName)!,
            IsTest = istest,
        };

        await _dbContext.TaskComponents.MutateAsync(task);

        await _dbContext.TestValues.MutateAsync(
            testParams.Select(x => new TestValue
            {
                Id = testIdCounter,
                TaskId = task.Id,
                InputVars = ToPropertiesDictionary(x.InputVars),
                OutputVars = ToPropertiesDictionary(x.OutputVars)
            }));

        await CreateStatusR(id);

        //if (performanceTests is not null)
        //{
        //    performanceTests.ForEach(x =>
        //    {
        //        x.Id = performanceTestIdCounter;
        //        x.TaskDescription = task;
        //    });
        //    await _dbContext.PerformanceTests.MutateAsync(performanceTests);
        //}

    }
    private async Task CreateStatusR(long id)
    {
        TaskStatusR _status = await _dbContext.TaskStatus.FirstOrDefaultAsync(x => x.TaskId == id);
        if (_status == null)
        {
            var newStatusR = new TaskStatusR
            {
                TaskId = id
            };
            _dbContext.TaskStatus.Add(newStatusR);
            await _dbContext.SaveChangesAsync();
        }
    }
    private Dictionary<string, string> ToPropertiesDictionary(object instance)
        => instance.GetType()
            .GetProperties()
            .ToDictionary(x => x.Name, x => x.GetValue(instance)!.ToString()!);

    private Stream GetStream(string fileName)
        => Assembly.GetManifestResourceStream(fileName)!;

    private async Task<JsonObject> LoadToolboxAsync(string toolboxFileName)
{
    string resourceName = $"RabinKit.Core.Components.Toolboxes.{toolboxFileName}.json";

    using (var stream = GetStream(resourceName))
    {
        if (stream == null)
        {
            throw new InvalidOperationException("Failed to get a valid stream.");
        }

        return (await JsonSerializer.DeserializeAsync<JsonObject>(stream))!;
    }
}

}


