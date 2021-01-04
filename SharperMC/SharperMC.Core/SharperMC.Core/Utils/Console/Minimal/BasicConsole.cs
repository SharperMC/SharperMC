using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Utils.Console.Minimal
{
    public class BasicConsole
    {
        public static readonly BasicConsole Instance = new BasicConsole();

        public void StartInputting()
        {
            while (true) GuiApp.LineRed(System.Console.ReadLine());
        }

        public void Log(ChatText text)
        {
            if (!GuiApp.Pause) text.SetNext(new ChatText("", TextColor.Reset));
            text.PrintNext(GuiApp.ConsoleColors);
            if (!GuiApp.Pause) System.Console.Write("\n");
        }
    }
}