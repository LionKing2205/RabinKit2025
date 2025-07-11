﻿using RabinKit.Core.Abstractions;
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
using System.Text;

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
        var performanceTestIdCounter = new AutoCounter<long>(1);

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
                task.istest,
                performanceTestIdCounter: performanceTestIdCounter
            );
        }
        foreach (var task in SecondModuleParameters.Tasks)
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
                task.istest,
                performanceTestIdCounter: performanceTestIdCounter
            );
        }
        foreach (var task in ThirdModuleParameters.Tasks)
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
                task.istest,
                performanceTestIdCounter: performanceTestIdCounter
            );
        }
        foreach (var task in FourthModuleParameters.Tasks)
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
                task.istest,
                performanceTestIdCounter: performanceTestIdCounter

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
        AutoCounter<long> testIdCounter,
        List<ModuleSetExtension.TestValuesSet> testParams,
        bool istest = false,
        AutoCounter<long> performanceTestIdCounter = null
        )
    {
        
        string description;
        var id = Convert.ToInt64(module * 10 + number);
        try
        {
            using (var reader = new StreamReader(GetStream($"RabinKit.Core.Components.Descriptions.{id}.md"), Encoding.UTF8))
            {
                description = await reader.ReadToEndAsync();
            }
        }
        catch (Exception)
        {
            description = "None";
        }
        
        var task = new TaskComponent
        {
            Id = id,
            Name = name,
            Description = description,
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

        if (id >= 40)
        {
            List<PerformanceTest>? performanceTests = [new PerformanceTest()];
            performanceTests.ForEach(x =>
            {
                x.Id = performanceTestIdCounter;
                x.TaskComponent = task;
            });
            await _dbContext.PerformanceTests.MutateAsync(performanceTests);
        }

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


