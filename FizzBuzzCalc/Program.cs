namespace FizzBuzzCalc
{
    using FlaUI.UIA3;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using FlaUI.Core;
    using OperatingSystem = FlaUI.Core.Tools.OperatingSystem;

    public static class Program
    {
        private const string Fizz = "Fizz";
        private const string Buzz = "Buzz";

        private const string MagicWindows10CalculatorName = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

        public static void Main(string[] args)
        {
            const int endValue = 52;

            Console.WriteLine($"Running FizzBuzz from 1 to {endValue}");

            if (!OperatingSystem.IsWindows10())
            {
                ShowError("Can only run on Windows 10, sorry!");
                return;
            }

            using (var automation = new UIA3Automation())
            {
                CloseExistingCalculators(automation);

                Application.LaunchStoreApp(MagicWindows10CalculatorName);

                var host = GetCalculatorProcess();

                if (host == null)
                {
                    ShowError("Failed to find the associated ApplicationFrameHost.");
                    return;
                }

                var calculatorApp = Application.Attach(host);

                Thread.Sleep(TimeSpan.FromSeconds(1.5));

                try
                {
                    var topWindows = calculatorApp.GetAllTopLevelWindows(automation);

                    if (topWindows.Length == 0)
                    {
                        ShowError("Failed to find the main window for Calculator.");
                        return;
                    }

                    var mainWindow = topWindows[0];

                    if (mainWindow == null)
                    {
                        ShowError("Failed to find the main window for Calculator.");
                        return;
                    }

                    var calculator = new Calculator(mainWindow);

                    for (var i = 1; i <= endValue; i++)
                    {
                        var fizzy = calculator.Divide(i, 3);
                        var buzzy = calculator.Divide(i, 5);

                        var written = false;

                        if (IsWhole(fizzy))
                        {
                            written = true;
                            Console.Write(Fizz);
                        }

                        if (IsWhole(buzzy))
                        {
                            written = true;
                            Console.Write(Buzz);

                            calculator.ClearHistory();
                        }

                        if (!written)
                        {
                            Console.Write(i.ToString("G"));
                        }

                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Failed due to an unexpected error: {ex}.");
                }
                finally
                {
                    calculatorApp?.Close();
                }
            }

            Console.WriteLine("FizzBuzz complete, thanks for watching!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static bool IsWhole(decimal value)
        {
            return Math.Floor(value) == value;
        }

        private static void ShowError(string value)
        {
            var current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = current;
            Console.ReadKey();
        }

        private static Process GetCalculatorProcess()
        {
            var host = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.Contains("ApplicationFrameHost", StringComparison.OrdinalIgnoreCase)
                                                                  && x.MainWindowTitle.Contains("calc", StringComparison.OrdinalIgnoreCase));

            return host;
        }

        private static void CloseExistingCalculators(AutomationBase automation)
        {
            var existingHost = GetCalculatorProcess();

            if (existingHost != null)
            {
                using (var tempApp = Application.Attach(existingHost))
                {
                    var topWindows = tempApp.GetAllTopLevelWindows(automation);

                    foreach (var topWindow in topWindows)
                    {
                        topWindow.Close();
                    }
                }
            }
        }
    }
}
