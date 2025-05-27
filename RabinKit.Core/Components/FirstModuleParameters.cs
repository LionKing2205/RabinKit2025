using RabinKit.Core.Extensions;
using static RabinKit.Core.Extensions.ModuleSetExtension;

namespace RabinKit.Core.Components
{
    public static class FirstModuleParameters
    {
        private static AutoCounter<int> numberCounter = new AutoCounter<int>(1);
        public static List<(int module, int number, string name, string[] inputVars,
            string[] outputVars, string toolboxFileName, List<TestValuesSet> testParams, bool istest)>
            Tasks = new List<(int, int, string, string[], string[], string, List<TestValuesSet>, bool)>
            {
                (1,numberCounter,
                "Разработать функцию для дискретного возведения в квадрат по модулю",
                ["m", "n"],["c"],
                "FirstBegin",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        m = 723,
                        n = 43357,
                    },
                    OutputVars: new
                    {
                        с = 2445,
                    }),
                ],
                false),///доб-ть условие m<n
                (1,numberCounter,
                "Разработать блочную программу проверки числа на простоту тестом Ферма",
                ["p"],["is_prime"],
                "FirstPrime",
                [
                new TestValuesSet(
                    InputVars: new
                    {
                        p = 53,
                    },
                    OutputVars: new
                    {
                        is_prime = true,
                    }),
                new TestValuesSet(
                    InputVars: new
                    {
                        p = 81,
                    },
                    OutputVars: new
                    {
                        is_prime = false,
                    })
                ],
                false),
                (1,numberCounter,
                "Разработать блочную программу проверки числа на простоту тестом Миллера-Рабина",
                ["p"],["is_prime"],
                "FirstPrime",
                [
                new TestValuesSet(
                    InputVars: new
                    {
                        p = 53,
                    },
                    OutputVars: new
                    {
                        is_prime = true,
                    }),
                new TestValuesSet(
                    InputVars: new
                    {
                        p = 81,
                    },
                    OutputVars: new
                    {
                        is_prime = false,
                    }),
                new TestValuesSet(
                    InputVars: new
                    {
                        p = 561,
                    },
                    OutputVars: new
                    {
                        is_prime = false,
                    }),
                ],
                false),
                (1,numberCounter,
                "Разработать генератор ключа для криптосистемы Рабина",
                ["bit_length"],["p", "q"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        bit_length = 20,
                    },
                    OutputVars: new
                    {
                        p = "keytest",
                        q = "keytest",
                    }),
                ],
                false),
                (1,numberCounter,
                "Зашифровать произвольное сообщение по алгоритму Рабина, на основе ключей, полученных из п.4",
                ["p", "q", "m"],["c"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 83,
                        q = 51,
                        m = 123,
                    },
                    OutputVars: new
                    {
                        c = 2430
                    }),
                ],
                false),
                (1,numberCounter,
                "Зашифровать произвольное сообщение по алгоритму Рабина",
                ["bit_length", "m"],["p","q","c"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        bit_length = 32,
                        m = 1
                    },
                    OutputVars: new
                    {
                        p = "keytest",
                        q = "keytest",
                        c = 1
                    }),
                ],
                true),
            };
    }
}
