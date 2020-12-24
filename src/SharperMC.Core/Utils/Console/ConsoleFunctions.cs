// Distrubuted under the MIT license
// ===================================================
// SharperMC uses the permissive MIT license.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the “Software”), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ©Copyright SharperMC - 2020

using SharperMC.Core.Config;
using SharperMC.Core.Utils.Console.Utils;

namespace SharperMC.Core.Utils.Console
{
    public static class ConsoleFunctions
    {
        /// <summary>
        /// Writes to console
        /// </summary>
        public static void WriteLine(string text, params object[] args)
        {
            Write(new FancyText(Format(text, args), FancyColor.Reset));
        }

        /// <summary>
        /// Writes to console with custom foreGround option
        /// </summary>
        public static void WriteLine(string text, FancyColor color, params object[] args)
        {
            Write(new FancyText(Format(text, args), color));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteInfoLine(string text, params object[] args)
        {
            Write(NewFancyText("[Info] ", FancyColor.Green, Format(text, args)));
        }

        /// <summary>
        /// Writes information to console with using a FancyText
        /// </summary>
        public static void WriteInfoLine(FancyText text, params object[] args)
        {
            text.Text = Format(text.Text, args);
            Write(NewFancyText("[Info] ", FancyColor.Green, text));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteFatalErrorLine(string text, params object[] args)
        {
            Write(new FancyText("[FatalError] ", FancyColor.Red, FancyColor.Bold)
                {Next = new FancyText(Format(text, args), FancyColor.Reset)});
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteErrorLine(string text, params object[] args)
        {
            Write(NewFancyText("[Error] ", FancyColor.DarkRed, Format(text, args)));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteWarningLine(string text, params object[] args)
        {
            Write(NewFancyText("[Warning] ", FancyColor.Yellow, Format(text, args)));
        }

        /// <summary>
        /// Writes debug info to the console but can also be disables from ServerSettings.
        /// </summary>
        public static void WriteDebugLine(string text, params object[] args)
        {
            if (!Server.ServerSettings.Debug) return;
            Write(NewFancyText("[Debug] ", FancyColor.Gray, Format(text, args)));
        }

        public static void Write(FancyText text)
        {
            GuiApp.Log(text);
        }

        public static void ClearConsole()
        {
            System.Console.Clear();
        }

        public static string Format(string text, params object[] args)
        {
            return (args == null || args.Length == 0) ? text : string.Format(text, args);
        }

        public static FancyText NewFancyText(string prefix, FancyColor color, string text)
        {
            return NewFancyText(prefix, color, new FancyText(text, FancyColor.Reset));
        }

        public static FancyText NewFancyText(string prefix, FancyColor color, FancyText text)
        {
            return new FancyText(prefix, color) {Next = text};
        }

        // Pauses 
        public static void Pause()
        {
            GuiApp.Pause = true;
        }

        public static void Continue()
        {
            Write(new FancyText("\n", FancyColor.Reset));
            GuiApp.Pause = false;
        }
    }
}