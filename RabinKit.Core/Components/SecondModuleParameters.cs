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
                        p = 6247,
                        q = 6551,
                        c = 20018970,
                    },
                    OutputVars: new
                    {
                        mp = 4677,
                        mq = 750,
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
                        p = 6247,
                        q = 6551,
                    },
                    OutputVars: new
                    {
                        yp = 1530,
                        yq = -1459,
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
                        mp = 11337032,
                        mq = 20662512,
                        yp = -323824,
                        yq = 253879,
                        p = 21550819,
                        q = 27488183
                    },
                    OutputVars: new
                    {
                        m1 = 216815840381495,
                        m2 = 375577016090382,
                        m3 = 199419941994,
                        m4 = 592193436529883
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
                        c = 399755760729601,
                        p = 21550819,
                        q = 27488183
                    },
                    OutputVars: new
                    {
                        m1 = 216815840381495,
                        m2 = 375577016090382,
                        m3 = 199419941994,
                        m4 = 592193436529883
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
                        c = 399755760729601,
                        p = 21550819,
                        q = 27488183
                    },
                    OutputVars: new
                    {
                        m1 = 216815840381495,
                        m2 = 375577016090382,
                        m3 = 199419941994,
                        m4 = 592193436529883
                    }),
                ],
                true),
            };
    }
}
