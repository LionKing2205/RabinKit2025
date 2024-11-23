using RabinKit.Core.Abstractions;
using RabinKit.Core.Entities;
//using RabinKit.Core.Enums;
using RabinKit.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.Text.RegularExpressions;
using IronPython.Hosting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RabinKit.Core.Services
{
    public class TaskRunner
    {

        private readonly IDbContext _dbContext;
        private readonly ILogger<TaskRunner> _logger;
        private readonly ScriptEngine _engine;

        public TaskRunner(
        IDbContext dbContext,
        ILogger<TaskRunner> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _engine = Python.CreateEngine();

            var paths = _engine.GetSearchPaths()
                .Append(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "PythonModules"))
                .ToList();

            _engine.SetSearchPaths(paths);
        }
        public async Task RunAsync(TaskAttempt taskAttempt)
        {
            taskAttempt.Code = EditCode(taskAttempt);
           
            var outputVars = RunCode(taskAttempt, taskAttempt.Inputs);
            taskAttempt.Result = string.Join(
                Environment.NewLine,
                outputVars.Select(x => $"{x.Name} = {x.Value}"));

            RunTest(taskAttempt);

            if (taskAttempt.IsNew)
                _dbContext.TaskAttempts.Add(taskAttempt);
            else
                _dbContext.TaskAttempts.Update(taskAttempt);

            await _dbContext.SaveChangesAsync();
        }

        private async void RunTest(TaskAttempt taskAttempt)
        {
            bool lastResult = true;
            bool shouldBreak = false;

            foreach (var test in taskAttempt.TaskComponents.Values)
            {
                var i = 0;
                var result = RunCode(taskAttempt, test.InputVars).Select(x => x.Value).ToList();
                var prepareoutputs = test.OutputVars.Select(x => x.Value).ToList();
                foreach (var value in result) {
                    if (result[i].ToString() != prepareoutputs[i].ToString())
                    {
                        lastResult = false;
                        shouldBreak = true;
                        break;
                    }
                    i ++;
                }
                if (shouldBreak){break;}
            }
            if (lastResult)
            {
                taskAttempt.IsPassed = true;
                TaskStatusR statusRes = await _dbContext.TaskStatus.FirstOrDefaultAsync(x => x.TaskId == taskAttempt.TaskId);
                if (statusRes != null)
                {
                    statusRes.IsPassed = true;
                }
            }
            else taskAttempt.IsPassed = false;
        }

        private IEnumerable<(string Name, dynamic Value)> RunCode(
          TaskAttempt taskAttempt, Dictionary<string, string> parameters)
        {
            var code = taskAttempt.Code;
            var scope = _engine.CreateScope();
            scope.SetVariable(taskAttempt.TaskComponents.Output.FirstOrDefault(), 0);

            foreach (var taskAttemptParameter in parameters)
            {
                var value = _engine.Execute($"int(\"{taskAttemptParameter.Value}\")");
                scope.SetVariable(taskAttemptParameter.Key, value);
            }
            var codelibs = $"import zlib\n";
            code = codelibs + code;
            _engine.Execute(code, scope);

            var outputVars = taskAttempt.TaskComponents.Output.Select(x => (x, scope.GetVariable(x)));
            return outputVars;
        }

        public static string PrepareScriptForDisplay(string code)
        {
            return new Regex(
                @"^s*globals+[wd ,]+(s*=s*None)?s*$n?",
                RegexOptions.Compiled | RegexOptions.Multiline)
                .Replace(code, "");
        }

        private static string EditCode(TaskAttempt taskAttempt)
        {
            var editedcode = PrepareScriptForDisplay(
                taskAttempt.TaskComponents.Input
                    .Aggregate(
                        taskAttempt.Code,
                        (acc, cur) =>
                            acc.Replace($"{cur} = None\n", "")));
            editedcode = PrepareScriptForDisplay(
                 taskAttempt.TaskComponents.Output
                  .Aggregate(
                    editedcode,
                   (acc, cur) =>
                       acc.Replace($"{cur} = None\n", "")));
         //   editedcode = Regex.Replace(editedcode, @"^s*n", "", RegexOptions.Multiline);
            editedcode = Regex.Replace(editedcode, @"(ns*n)+", "n");
            editedcode = editedcode.Trim();
            return editedcode;
        }
    }
}
