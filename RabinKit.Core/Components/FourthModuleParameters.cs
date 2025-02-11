using RabinKit.Core.Extensions;
using static RabinKit.Core.Extensions.ModuleSetExtension;

namespace RabinKit.Core.Components
{
    public static class FourthModuleParameters
    {
        private static AutoCounter<int> numberCounter = new AutoCounter<int>(1);
        public static List<(int module, int number, string name, string[] inputVars,
            string[] outputVars, string toolboxFileName, List<TestValuesSet> testParams, bool istest)>
            Tasks = new List<(int, int, string, string[], string[], string, List<TestValuesSet>, bool)>
            {
                (4,numberCounter,
                "Реализовать расшифровку сообщения методом Брутфорса",
                ["c", "n", "m_id"],["p", "q", "m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        c = 20018970,
                        n = 40924097,
                        m_id = 3,
                    },
                    OutputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        m = 19941994
                    }),
                ],
                false),
                (4,numberCounter,
                "Реализовать расшифровку сообщения методом Ро-Полларда",
                ["c", "n", "m_id"],["p", "q", "m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        c = 20018970,
                        n = 40924097,
                        m_id = 3,
                    },
                    OutputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        m = 19941994
                    }),
                ],
                false),
                (4,numberCounter,
                "Реализовать расшифровку сообщения методом Брента",
                ["c", "n", "m_id"],["p", "q", "m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        c = 20018970,
                        n = 40924097,
                        m_id = 3,
                    },
                    OutputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        m = 19941994
                    }),
                ],
                false),
                //ToDo: доб-ть условие, что p-наименьшее
            };
    }
}
