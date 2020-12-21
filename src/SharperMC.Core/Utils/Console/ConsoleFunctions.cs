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

using System;
using SharperMC.Core.Config;

namespace SharperMC.Core.Utils.Console
{
    internal static class ConsoleFunctions
    {
        /// <summary>
        /// Writes to console but also contains a way to disable new line 
        /// </summary>
        public static void WriteLine(string text, bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes to console with custom foreGround option but also includes a way to disable new line
        /// </summary>
        public static void WriteLine(string text, ConsoleColor foreGroundColor, bool newline = true,
            params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = foreGroundColor;
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
            System.Console.ResetColor();
        }

        /// <summary>
        /// Writes to console with custom foreGround and backGround options but also includes a way to disable new line
        /// </summary>
        public static void WriteLine(string text, ConsoleColor foreGroundColor, ConsoleColor backGroundColor,
            bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = foreGroundColor;
            System.Console.BackgroundColor = backGroundColor;
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
            System.Console.ResetColor();
        }

        /// <summary>
        /// Writes information to console with a neat color but you can also disable the new line feature
        /// </summary>
        public static void WriteInfoLine(string text, bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("[INFO] ");
            System.Console.ResetColor();
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes information to console with a neat color but you can also disable the new line feature
        /// </summary>
        public static void WriteFatalErrorLine(string text, bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("[FATAL ERROR] ");
            System.Console.ResetColor();
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes information to console with a neat color but you can also disable the new line feature
        /// </summary>
        public static void WriteErrorLine(string text, bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("[ERROR] ");
            System.Console.ResetColor();
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes information to console with a neat color but you can also disable the new line feature
        /// </summary>
        public static void WriteWarningLine(string text, bool newline = true, params object[] args)
        {
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.Write("[WARNING] ");
            System.Console.ResetColor();
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes debug info to the console but can also be disables from ServerSettings.
        /// </summary>
        public static void WriteDebugLine(string text, bool newline = true, params object[] args)
        {
            if (!Server.ServerSettings.Debug) return;
            Globals.ColoredConsole.Printed = true;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write("[DEBUG] ");
            System.Console.ResetColor();
            if (args == null || args.Length == 0)
                System.Console.Write(text + (newline ? "\n" : ""));
            else
                System.Console.Write(text + (newline ? "\n" : ""), args);
        }

        public static void ClearConsole()
        {
            System.Console.Clear();
        }

        public static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}