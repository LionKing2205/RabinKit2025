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
using IronPython.Compiler.Ast;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using IronPython.Hosting;


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

        //private IEnumerable<(string Name, dynamic Value)> RunCode(
        //  TaskAttempt taskAttempt, Dictionary<string, string> parameters)
        //{
        //    var code = taskAttempt.Code;
        //    var scope = _engine.CreateScope();
        //    scope.SetVariable(taskAttempt.TaskComponents.Output.FirstOrDefault(), 0);

        //    foreach (var taskAttemptParameter in parameters)
        //    {
        //        var value = _engine.Execute($"int(\"{taskAttemptParameter.Value}\")");
        //        scope.SetVariable(taskAttemptParameter.Key, value);
        //    }

        //    var moduleid = taskAttempt.TaskId / 10;
        //   // code = EditCodeDefs(code);

        //    if ((moduleid == 3) || (moduleid == 4)){
        //        code = EditCodeDefsDecrypt(code);
        //    }
        //    _engine.Execute(code, scope);
        //    ///for module 4 add return code
        //    var outputVars = taskAttempt.TaskComponents.Output.Select(x => (x, scope.GetVariable(x)));
        //    return outputVars;

        //    //todo: доработать преобразование списка в выводимое значение
        //}
        private IEnumerable<(string Name, dynamic Value)> RunCode(
    TaskAttempt taskAttempt, Dictionary<string, string> parameters)
        {
            // Prepare code once based on module ID
            var code = taskAttempt.Code;
            var moduleId = taskAttempt.TaskId / 10;

            if (moduleId == 3 || moduleId == 4)
            {
                code = EditCodeDefsDecrypt(code);
            }

            // Compile the code once for reuse
            var compiledCode = _engine.CreateScriptSourceFromString(code).Compile();

            // Create and populate scope
            var scope = _engine.CreateScope();

            // Set default output value
            var firstOutput = taskAttempt.TaskComponents.Output.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstOutput))
            {
                scope.SetVariable(firstOutput, 0);
            }

            // Process all parameters at once
            if (parameters.Count > 0)
            {
                // Create a compiled converter for parameters
                var intConverter = _engine.CreateScriptSourceFromString("def convert(x): return int(x)").Compile();
                intConverter.Execute(scope);

                foreach (var param in parameters)
                {
                    // Use compiled convert function instead of executing a new script for each value
                    dynamic convertFunction = scope.GetVariable("convert");
                    dynamic value = convertFunction(param.Value);
                    scope.SetVariable(param.Key, value);
                }
            }
            compiledCode.Execute(scope);
            return taskAttempt.TaskComponents.Output.Select(x => (x, scope.GetVariable(x)));
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

                            m1 = math.fabs(pow((a + b), 1, n))
                            m2 = n - m1
                            m3 = math.fabs(pow((a - b), 1, n))
                            m4 = n - m3

                            possible_answers = [m1,m2,m3,m4]

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
                        
                            m1 = math.fabs(pow((a + b), 1, n))
                            m2 = n - m1
                            m3 = math.fabs(pow((a - b), 1, n))
                            m4 = n - m3
                        
                            possible_answers = [m1,m2,m3,m4]

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
                                result = random.getrandbits(bit_length - 2) | (1 << (bit_length - 1)) | 0b11
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

        public async Task RunPerformanceTestAsync(
            PerformanceTestAttempt test, int AttemptsMaxCount, int max_time,
            Action<long, TimeSpan?, long?, int> callback,
            CancellationToken cancellationToken = default)
        {

            var attempt = test.PerformanceTest.TaskComponent.Attempts
                .Where(x => x.IsPassed)
                .MaxBy(x => x.CreatedAt)
                ?? throw new ApplicationExceptionBase("Удачный запуск скрипта не найден");

            var prepareScript = """"
                            import struct
                            import random
                            import math
                            import zlib
                            import random
                            from numbers import Number

                            def generate_random_number(bit_length):
                                #while True:
                                #    rand_num = random.getrandbits(bit_length)
                                #    result = rand_num | 0b11 
                                #    if result.bit_length() < bit_length:
                                #        result = (1 << (bit_length - 1)) | result
                                #    return result
                                result = random.getrandbits(bit_length - 2) | (1 << (bit_length - 1)) | 0b11
                                return result

                            def modular_exponentiation(base, exponent, modulus):
                                result = 1
                                base = base % modulus
                                while exponent > 0:
                                    if (exponent & 1) == 1:  
                                        result = (result * base) % modulus
                                    exponent = exponent >> 1 
                                    base = (base * base) % modulus
                                return result

                            def miller_rabin_test(n, k=20):
                                if n <= 1:
                                    return False
                                if n <= 3:
                                    return True
                                if n % 2 == 0:
                                    return False

                                r, d = 0, n - 1
                                while d % 2 == 0:
                                    d //= 2
                                    r += 1

                                for _ in range(k):
                                    a = random.randint(2, n - 2)
                                    x = modular_exponentiation(a, d, n)

                                    if x == 1 or x == n - 1:
                                        continue

                                    for _ in range(r - 1):
                                        x = modular_exponentiation(x, 2, n)
                                        if x == n - 1:
                                            break
                                    else:
                                        return False  

                                return True 

                            def generate_prime_4k_plus_3(x):
                              while True:
                                k = generate_random_number((math.ceil(x / 2)))
                                if miller_rabin_test(k):
                                  break
                              return k

                            def generate_key_message(bits):
                                p = generate_prime_4k_plus_3(bits)
                                q = generate_prime_4k_plus_3(bits)
                                while p == q:
                                    q = generate_prime_4k_plus_3(bits)
                                
                                n = p * q

                                c = pow(12345, 2, n)
                                return n,c
                                 
                            n,c  = generate_key_message(input_value)
                            m_id = 1
                            """";

            var code = EditCode(attempt);
           // code = EditCodeDefs(code);
            code = EditCodeDefsDecrypt(code);

            var runs = test.Runs.ToList();

            var steps = 1;

            for (var index = 0; index < runs.Count; index++)
            {
                var value = runs[index].Key;

                if (cancellationToken.IsCancellationRequested)
                {
                    test.Runs[value] = default;
                    callback(
                        value,
                        test.Runs[value],
                        runs.Cast<KeyValuePair<long, TimeSpan?>?>().ElementAtOrDefault(index + 1)?.Key,
                        default);
                    break;
                }

                if (index % steps != 0)
                {
                    test.Runs[value] = null;
                }
                else
                {
                    //var stopWatch = new Stopwatch();
                    //var elapsedSum = TimeSpan.Zero;
                    //var runAttempt = 0;

                    //try
                    //{
                    //    for (runAttempt = 0; elapsedSum < TimeSpan.FromMinutes(max_time) && runAttempt < AttemptsMaxCount; runAttempt++)
                    //    {
                    //        await Task.Run(() =>
                    //        {
                    //            var scope = _engine.CreateScope();
                    //            scope.SetVariable("input_value", value);
                    //            _engine.Execute(prepareScript, scope);
                    //            stopWatch.Restart();
                    //            cancellationToken.ThrowIfCancellationRequested();
                    //            _engine.Execute(code, scope);
                    //            stopWatch.Stop();
                    //        },
                    //            cancellationToken);

                    //        elapsedSum += stopWatch.Elapsed;
                    //    }

                    //    test.Runs[value] = elapsedSum / runAttempt;

                    //}
                    //catch (Exception e)
                    //{
                    //    _logger.LogWarning(e, "Ошибка выполнения скрипта");
                    //}
                    //finally
                    //{
                    //    callback(
                    //        value,
                    //        test.Runs[value],
                    //        runs.Cast<KeyValuePair<long, TimeSpan?>?>().ElementAtOrDefault(index + 1)?.Key,
                    //        runAttempt);
                    //}
                    var actualRuns = 0;
                    try
                    {
                        var prepareScriptCompiled = _engine.CreateScriptSourceFromString(prepareScript).Compile();
                        var codeCompiled = _engine.CreateScriptSourceFromString(code).Compile();

                        var sharedScope = _engine.CreateScope();
                        sharedScope.SetVariable("input_value", value);

                        var totalElapsed = TimeSpan.Zero;

                        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(max_time));
                        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
                        var stopWatch = new Stopwatch();

                        while (actualRuns < AttemptsMaxCount && totalElapsed < TimeSpan.FromMinutes(max_time))
                        {
                            try
                            {
                                var runScope = _engine.CreateScope();
                                foreach (var variableName in sharedScope.GetVariableNames())
                                {
                                    runScope.SetVariable(variableName, sharedScope.GetVariable(variableName));
                                }

                                await Task.Run(() =>
                                {
                                    prepareScriptCompiled.Execute(sharedScope);
                                    stopWatch.Restart();
                                    codeCompiled.Execute(runScope);
                                    stopWatch.Stop();
                                }, linkedCts.Token);

                                var elapsed = stopWatch.Elapsed;
                                totalElapsed += elapsed;
                                actualRuns++;
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (Exception innerEx)
                            {
                                _logger.LogWarning(innerEx, $"Ошибка в попытке {actualRuns} для значения {value}");
                            }
                        }

                        if (actualRuns > 0)
                        {
                            test.Runs[value] = totalElapsed / actualRuns;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, $"Критическая ошибка при выполнении скрипта для значения {value}");
                    }
                    finally
                    {
                        // Финальное обновление прогресса
                        callback(
                            value,
                            actualRuns > 0 ? test.Runs.GetValueOrDefault(value) : null,
                            runs.Cast<KeyValuePair<long, TimeSpan?>?>().ElementAtOrDefault(index + 1)?.Key,
                            actualRuns);
                    }

                }
            }
           // await FillNullValues(test);
        }
        private async Task FillNullValues(PerformanceTestAttempt test)
        {
            var Runs = test.Runs;
            List<long> keys = new List<long>(Runs.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                if (Runs[keys[i]] == null)
                {
                    TimeSpan? previousValue = null;
                    TimeSpan? nextValue = null;

                    // Находим предыдущее ненулевое значение
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (Runs[keys[j]] != null)
                        {
                            previousValue = Runs[keys[j]];
                            break;
                        }
                    }

                    // Находим следующее ненулевое значение
                    for (int j = i + 1; j < keys.Count; j++)
                    {
                        if (Runs[keys[j]] != null)
                        {
                            nextValue = Runs[keys[j]];
                            break;
                        }
                    }

                    // Если оба значения найдены, вычисляем среднее
                    if (previousValue.HasValue && nextValue.HasValue)
                    {
                        Runs[keys[i]] = TimeSpan.FromTicks((previousValue.Value.Ticks + nextValue.Value.Ticks) / 2);
                    }
                }
            }
        }
    }
}
