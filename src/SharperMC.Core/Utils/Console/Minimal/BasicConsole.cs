using SharperMC.Core.Utils.Console.Utils;

namespace SharperMC.Core.Utils.Console.Minimal
{
    public class BasicConsole
    {
        public static readonly BasicConsole Instance = new BasicConsole();

        public void StartInputting(string[] args)
        {
            while (true) GuiApp.LineRed(System.Console.ReadLine());
        }

        public void Log(FancyText text)
        {
            text.PrintNext(GuiApp.ConsoleColors);
            if (!GuiApp.Pause) System.Console.Write("\n");
        }
    }
}