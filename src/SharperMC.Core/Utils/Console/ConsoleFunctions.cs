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

using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Utils.Console
{
    public static class ConsoleFunctions
    {
        /// <summary>
        /// Writes to console
        /// </summary>
        public static void WriteLine(string text, params object[] args)
        {
            Write(new ChatText(TextUtils.Format(text, args), TextColor.Reset));
        }

        /// <summary>
        /// Writes to console with custom foreGround option
        /// </summary>
        public static void WriteLine(string text, TextAttribute color, params object[] args)
        {
            Write(new ChatText(TextUtils.Format(text, args), color));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteInfoLine(string text, params object[] args)
        {
            Write(TextUtils.NewFancyText("[Info] ", TextColor.Green, TextUtils.Format(text, args)));
        }

        /// <summary>
        /// Writes information to console with using a FancyText
        /// </summary>
        public static void WriteInfoLine(ChatText text, params object[] args)
        {
            text.Text = TextUtils.Format(text.Text, args);
            Write(TextUtils.NewFancyText("[Info] ", TextColor.Green, text));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteFatalErrorLine(string text, params object[] args)
        {
            Write(new ChatText("[FatalError] ", TextColor.Red, TextColor.Bold)
                {Next = new ChatText(TextUtils.Format(text, args), TextColor.Reset)});
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteErrorLine(string text, params object[] args)
        {
            Write(TextUtils.NewFancyText("[Error] ", TextColor.DarkRed, TextUtils.Format(text, args)));
        }

        /// <summary>
        /// Writes information to console with a neat color
        /// </summary>
        public static void WriteWarningLine(string text, params object[] args)
        {
            Write(TextUtils.NewFancyText("[Warning] ", TextColor.Yellow, TextUtils.Format(text, args)));
        }

        /// <summary>
        /// Writes debug info to the console but can also be disables from ServerSettings.
        /// </summary>
        public static void WriteDebugLine(string text, params object[] args)
        {
            if (!Server.ServerSettings.Debug) return;
            Write(TextUtils.NewFancyText("[Debug] ", TextColor.Gray, TextUtils.Format(text, args)));
        }

        public static void Write(ChatText text)
        {
            GuiApp.Log(text);
        }

        public static void ClearConsole()
        {
            System.Console.Clear();
        }

        // Pauses 
        public static void Pause()
        {
            GuiApp.Pause = true;
        }

        public static void Continue()
        {
            Write(new ChatText("\n", TextColor.Reset));
            GuiApp.Pause = false;
        }
    }
}