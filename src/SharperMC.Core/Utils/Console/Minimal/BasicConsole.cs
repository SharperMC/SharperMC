using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Utils.Console.Minimal
{
    public class BasicConsole
    {
        public static readonly BasicConsole Instance = new BasicConsole();

        public void StartInputting(string[] args)
        {
            while (true) GuiApp.LineRed(System.Console.ReadLine());
        }

        public void Log(ChatText text)
        {
            text.PrintNext(GuiApp.ConsoleColors);
            if (!GuiApp.Pause) System.Console.Write("\n");
        }
    }
}