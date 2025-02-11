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
using IronPython.Runtime.Operations;

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
                foreach (var value in result)
                {
                    if (prepareoutputs[i] != "keytest")
                    {
                        if (result[i].ToString() != prepareoutputs[i].ToString())
                        {
                            lastResult = false;
                            shouldBreak = true;
                            break;
                        }
                    }
                    else
                    {
                        var bitLengths = test.InputVars
                            .Where(x => x.Key == "bit_length") 
                            .Select(x => x.Value) 
                            .FirstOrDefault();
                        var res = result[i];
                        bool testresult =  KeyTest((int)result[i], Convert.ToInt32(bitLengths));
                        if (testresult == false)
                        {
                            lastResult = false;
                            shouldBreak = true;
                            break;
                        }
                    }
                    i++;
                }
                if (shouldBreak){break;}
            }
            if (lastResult)
            {
                TaskStatusR statusRes = await _dbContext.TaskStatus.FirstOrDefaultAsync(x => x.TaskId == taskAttempt.TaskId);
                if (statusRes != null)
                {
                    statusRes.IsPassed = true;
                }
                taskAttempt.IsPassed = true;
            }
            else taskAttempt.IsPassed = false;
        }

        private static bool KeyTest(int p, int bitLength)
        {
            if (!IsPrime(p))
            {
                return false;
            }
            // Проверяем, что длина p соответствует половине указанной битовой длины
            int requiredLength = bitLength / 2;
            if (GetBitLength(p) != requiredLength)
            {
                return false;
            }
            // Проверяем условие p = k * 4 + 3
            if (p % 4 != 3)
            {
                return false;
            }
            return true;
        }
        static int GetBitLength(int number)
        {
            return (int)Math.Floor(Math.Log2(number)) + 1;
        }

        private static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true; 

            if (number % 2 == 0 || number % 3 == 0) return false; 

            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                {
                    return false;
                }
            }
            return true;
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

            var moduleid = taskAttempt.TaskId / 10;
            code = EditCodeDefs(code);

            if ((moduleid == 3) || (moduleid == 4)){
                code = EditCodeDefsDecrypt(code);
            }
            _engine.Execute(code, scope);
            ///for module 4 add return code
            var outputVars = taskAttempt.TaskComponents.Output.Select(x => (x, scope.GetVariable(x)));
            return outputVars;

            //todo: доработать преобразование списка в выводимое значение
        }
        private static string EditCodeDefsDecrypt(string code)
        {
            var decryption_def = $"\n\n" + """
                        import array
                        import math

                        def funcz(c, x):
                          y = round((x + 1) / 4)
                          return pow(c, y, x)

                        def extended_gcd(a, b):
                            if b == 0:
                                return a, 1, 0
                            gcd, x1, y1 = extended_gcd(b, a % b)
                            x = y1
                            q = a // b
                            y = x1 - q * y1
                            return gcd, x, y
                        

                        def decryption_mod(p, q, c, correct_id):
                            correct_id = correct_id - 1
                            n = p * q
                            roots_p = funcz(c, p)
                            roots_q = funcz(c, q)
                            y = extended_gcd(p, q)
                            yp = y[1]
                            yq = y[2]

                            a = roots_q * yp * p
                            b = roots_p * yq * q

                            possible_answers = [
                            pow(round(math.fabs(a + b)), 1, n),
                            n - pow(round(math.fabs(a + b)), 1, n),
                            pow(round(math.fabs(a - b)), 1, n),
                            n - pow(round(math.fabs(a - b)), 1, n)
                            ]
                            if correct_id < len(possible_answers):
                                return possible_answers[correct_id]
                            else:
                                return 0

                        def decryption_mod_list(p, q, c):
                            
                            n = p * q
                            roots_p = funcz(c, p)
                            roots_q = funcz(c, q)
                            y = extended_gcd(p, q)
                            yp = y[1]
                            yq = y[2]
                        
                            a = roots_q * yp * p
                            b = roots_p * yq * q
                        
                            possible_answers = [
                            pow(round(math.fabs(a + b)), 1, n),
                            n - pow(round(math.fabs(a + b)), 1, n),
                            pow(round(math.fabs(a - b)), 1, n),
                            n - pow(round(math.fabs(a - b)), 1, n)
                            ]
                            return possible_answers
                        """ + $"\n\n";
                            //return array.array('i', possible_answers)
            int index = code.IndexOf("def");
            if (index != -1)
            {
                    code = code.Insert(index, decryption_def);
            }
            else
            {
                int noneIndex = code.IndexOf("None");
                if (noneIndex != -1)
                {
                        code = code.Insert(index, decryption_def);
                }
                else
                {
                    int lastImportIndex = code.LastIndexOf("import");
                    if (lastImportIndex != -1)
                    {
                        int endOfLineIndex = code.IndexOf("\n", lastImportIndex);
                        if (endOfLineIndex == -1)
                        {
                            code += "\n" + decryption_def + "\n";
                        }
                        else
                        {
                            code = code.Insert(endOfLineIndex + 1, decryption_def + "\n");
                        }
                    }
                    else
                    {
                        code = decryption_def + "\n" + code;
                    }
                }
            }
            
            return code;
        }
        private static string EditCodeDefs(string code)
        {
            var codelibs = $"import zlib\nimport random\n";
            var generate_random_number = $"\n\n" + """
                            def generate_random_number(bit_length):
                                while True:
                                    rand_num = random.getrandbits(bit_length)
                                    result = rand_num | 0b11 
                                    if result.bit_length() < bit_length:
                                        result = (1 << (bit_length - 1)) | result
                                    return result
                            """ + $"\n\n";
            int index = code.IndexOf("def");
            if (index != -1)
            {
                int index1 = code.IndexOf("generate_random_number");
                if (index1 != -1)
                {
                    code = code.Insert(index, generate_random_number);
                }
            }
            else
            {
                int noneIndex = code.IndexOf("None");
                if (noneIndex != -1)
                {
                    int index1 = code.IndexOf("generate_random_number");
                    if (index1 != -1)
                    {
                        code = code.Insert(index, generate_random_number);
                    }
                }
            }
            code = codelibs + code;
            return code;
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
            var editedCode = Regex.Replace(taskAttempt.Code, @"(s*.*\bNone\b.*s*\n?)(s*\n)+", "", RegexOptions.Multiline);
            //var editedCode = PrepareScriptForDisplay(
            //    taskAttempt.TaskComponents.Input
            //        .Aggregate(
            //            taskAttempt.Code,
            //            (acc, cur) =>
            //                acc.Replace($"{cur} = None\n", "")));
            //editedCode = PrepareScriptForDisplay(
            //     taskAttempt.TaskComponents.Output
            //      .Aggregate(
            //        editedcode,
            //       (acc, cur) =>
            //           acc.Replace($"{cur} = None\n", "")));
            //   editedCode = Regex.Replace(editedcode, @"^s*n", "", RegexOptions.Multiline);
            editedCode = Regex.Replace(editedCode, @"(ns*n)+", "n");
            editedCode = editedCode.Trim();
            return editedCode;
        }
    }
}
