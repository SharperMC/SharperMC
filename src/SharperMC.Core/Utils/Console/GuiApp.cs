using System;
using System.Linq;
using SharperMC.Core.Commands;
using SharperMC.Core.Utils.Console.Minimal;
using SharperMC.Core.Utils.Console.Tabbing;
using SharperMC.Core.Utils.Console.Utils;

namespace SharperMC.Core.Utils.Console
{
    public static class GuiApp
    {
        public static event Action<string> LineRead;
        public static bool ConsoleColors = false;
        public static bool Minimal = false;
        public static bool Pause = false;

        public static void Start(string[] args)
        {
            LineRead += (s) =>
            {
                if (!string.IsNullOrEmpty(s))
                    CommandManager.ParseCommand(Globals.ConsoleSender, s);
            };
            ConsoleColors = args.Contains("--console-colors");
            if (ConsoleColors)
            {
                Log(new FancyText("[Log] ", FancyColor.Blue)
                {
                    Next = new FancyText("ConsoleColors enabled.", FancyColor.Reset)
                });
            }
            // ReSharper disable once AssignmentInConditionalExpression
            if (Minimal = args.Contains("--minimal"))
            {
                // do minimal when using in a web-panel-thingy like pterodactyl
                MinimalStart(args);
            }
            else
            {
                TabStart(args);
            }
        }

        public static void LineRed(string s)
        {
            LineRead?.Invoke(s);
        }

        public static void Log(FancyText text)
        {
            if (Minimal) BasicConsole.Instance.Log(text);
            else TabConsole.Instance.Log(text);
        }

        private static void TabStart(string[] args)
        {
            TabConsole.Instance.StartInputting(args);
        }

        private static void MinimalStart(string[] args)
        {
            BasicConsole.Instance.StartInputting(args);
        }
    }
}
/*
Application.Init();
            var top = Application.Top;
            var win = new Window("MyApp") {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            var menu = new MenuBar(new[] {
                new MenuBarItem("_File", new[] {
                    new MenuItem ("_New", "Creates a new file", () => { }),
                    new MenuItem ("_Close", "", () => { top.Running = false; }),
                    new MenuItem ("_Quit", "", () => { top.Running = false; }),
                })
            });
            top.Add(menu);
            var login = new Label("Login: ") { X = 3, Y = 2 };
            var password = new Label("Password: ") {
                X = Pos.Left(login),
                Y = Pos.Top(login) + 1
            };
            var loginText = new TextField("") {
                X = Pos.Right(password),
                Y = Pos.Top(login),
                Width = 40
            };
            var passText = new TextField("") {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText)
            };
            var loginEnter = new Action<View.KeyEventEventArgs>((k) => {
                if (!k.KeyEvent.Key.Equals(Key.Enter)) return;
                loginText.Text = "";
                passText.Text = "";
            });
            loginText.KeyPress += loginEnter;
            passText.KeyPress += loginEnter;

            var addTestButton = new Button(21, 14, "AddTest");
            var addTestAction = new Action(() => {
                login.EnsureFocus();

                loginText.Text += "Aha!" + addTestButton.HasFocus;
            });
            addTestButton.Clicked += addTestAction;

            // Add some controls, 
            win.Add(
                // The ones with my favorite layout system, Computed
                login, password, loginText, passText,

                // The ones laid out like an australopithecus, with Absolute positions:
                new CheckBox(3, 6, "Remember me"),
                new RadioGroup(3, 8, new ustring[] { "_Personal", "_Company", "H_ello" }),
                new Button(3, 14, "Ok"),
                new Button(10, 14, "Cancel"),
                addTestButton,
                new Label(3, 18, "Press F9 or ESC plus 9 to activate the menubar")
            ); ; ;

            Application.Run();
*/