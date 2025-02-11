using RabinKit.Core.Extensions;
using static RabinKit.Core.Extensions.ModuleSetExtension;

namespace RabinKit.Core.Components
{
    public static class SecondModuleParameters
    {
        private static AutoCounter<int> numberCounter = new AutoCounter<int>(1);
        public static List<(int module, int number, string name, string[] inputVars,
            string[] outputVars, string toolboxFileName, List<TestValuesSet> testParams, bool istest)>
            Tasks = new List<(int, int, string, string[], string[], string, List<TestValuesSet>, bool)>
            {
                (2,numberCounter,
                "Найти квадратные корни по модулю от каждого ключа (mp и mq)",
                ["p", "q","c"],["mp", "mq"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6551,
                        q = 6247,
                        c = 20018970,
                    },
                    OutputVars: new
                    {
                        mp = 750,
                        mq = 4677,
                    }),
                ],
                false),
                (2,numberCounter,
                "Реализовать расширенный алгоритм Евклида",
                ["p", "q"],["yp", "yq"],
                "SecondEuclide",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6551,
                        q = 6247,
                    },
                    OutputVars: new
                    {
                        yp = -1459,
                        yq = 1530,
                    }),
                ],
                false),
                (2,numberCounter,
                "Найти 4 потенциальных ключа с помощью китайской теоремы об остатках",
                ["mp", "mq", "yp", "yq", "p", "q"], ["m1", "m2", "m3", "m4"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        mp = 750,
                        mq = 4677,
                        yp = -1459,
                        yq = 1530,
                        p = 6551,
                        q = 6247
                    },
                    OutputVars: new
                    {
                        m1 = 6510944,
                        m2 = 34413153,
                        m3 = 19941994,
                        m4 = 20982103
                    }),
                ],
                false),
                (2,numberCounter,
                "Найти 4 потенциальных ключа с помощью исходного сообщения и приватных ключей",
                ["c", "p", "q"], ["m1", "m2", "m3", "m4"],
                "SecondEuclide",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        c = 20018970,
                        p = 6551,
                        q = 6247
                    },
                    OutputVars: new
                    {
                        m1 = 6510944,
                        m2 = 34413153,
                        m3 = 19941994,
                        m4 = 20982103
                    }),
                ],
                false),
            };
    }
}
