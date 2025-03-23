using RabinKit.Core.Extensions;
using static RabinKit.Core.Extensions.ModuleSetExtension;

namespace RabinKit.Core.Components
{
    public static class ThirdModuleParameters
    {
        private static AutoCounter<int> numberCounter = new AutoCounter<int>(1);
        public static List<(int module, int number, string name, string[] inputVars,
            string[] outputVars, string toolboxFileName, List<TestValuesSet> testParams, bool istest)>
            Tasks = new List<(int, int, string, string[], string[], string, List<TestValuesSet>, bool)>
            {
                (3,numberCounter,
                "Зашифровать сообщение и создать crc-код исходного сообщения",
                ["p", "q", "m"],["crc", "c"],
                "FirstFull",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        m = 19941994,
                    },
                    OutputVars: new
                    {
                        crc = 712449276,
                        c = 20018970
                    }),
                ],
                false),
                (3,numberCounter,
                "Расшифровать сообщение и снять неоднозначность полученным crc кодом",
                ["p", "q", "crc", "c"],["m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        crc = 712449276,
                        c = 20018970,
                    },
                    OutputVars: new
                    {
                        m = 19941994
                    }),
                ],
                false),
                (3,numberCounter,
                "Зашифровать сообщение и получить номер варианта расшифровки",
                ["p", "q", "m"],["num", "c"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        m = 19941994,
                    },
                    OutputVars: new
                    {
                        num = 3,
                        c = 20018970
                    }),
                ],
                false),
                (3,numberCounter,
                "Расшифровать сообщение и снять неоднозначность с установленным номером ответа",
                ["p", "q", "num", "c"],["m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        num = 3,
                        c = 20018970,
                    },
                    OutputVars: new
                    {
                        m = 19941994
                    }),
                ],
                false),
                (3,numberCounter,
                "Расшифровать сообщение и снять неоднозначность",
                ["p", "q", "num", "c"],["m"],
                "Full",
                [
                    new TestValuesSet(
                    InputVars: new
                    {
                        p = 6247,
                        q = 6551,
                        num = 3,
                        c = 20018970,
                    },
                    OutputVars: new
                    {
                        m = 19941994
                    }),
                ],
                true),
            };
    }
}
