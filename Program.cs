using System;

namespace CalcProject
{
    static class Program
    {
        private static bool _doAgain = true;
        static void Main()
        {
            while (_doAgain)
            {
                Console.WriteLine("Select calc mode:\n1 - Calculate from file and save results in new file\n2 - Input expression in console");
                string inputMode = Console.ReadLine();
                string message;

                if (inputMode.Equals("1"))
                {
                    message = "Input path to file (for example TestFile.txt) or type \"exit\" to quit";
                    DoLogic(ref _doAgain, message, CalcFile);                                         
                }

                else if (inputMode.Equals("2"))
                {
                    message = "Input expression or type \"exit\" to quit";
                    DoLogic(ref _doAgain, message, CalcConsole);
                }
            }
        }

        private static void CalcFile(string path)
        {
            if (!DoAgain(path)) { }

            else
            {
                CalcFile calc = new CalcFile(path);
                calc.Start();
                Console.WriteLine("Results saved in \"Results.txt\"");
            }
        }

        private static void CalcConsole(string input)
        {
            if (!DoAgain(input)) { }

            else
            {
                var rc = new Calc(input);
                Console.WriteLine($"Result: {rc.Calculate()}\n");
            }
        }

        private static bool DoAgain(string userInput)
        {
            _doAgain = !userInput.Equals("exit", StringComparison.CurrentCultureIgnoreCase);
            return _doAgain;
        }

        private static void DoLogic(ref bool _doAgain, string message, Action<string> CalcCaller)
        {
            while (_doAgain)
            {
                Console.WriteLine(message);
                string userInput = Console.ReadLine();

                try
                {
                    CalcCaller(userInput);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}