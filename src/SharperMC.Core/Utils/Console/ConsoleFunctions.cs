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

namespace SharperMC.Core.Utils
{
    internal static class ConsoleFunctions
    {
        /// <summary>
        /// Writes to console but also contains a way to disable new line 
        /// </summary>
        public static void WriteLine(string text, bool newline = true, params object[] args)
        {
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }

        /// <summary>
        /// Writes to console with custom foreGround option but also includes a way to disable new line
        /// </summary>
        public static void WriteLine(string text, ConsoleColor foreGroundColor, bool newline = true,
            params object[] args)
        {
            Console.ForegroundColor = foreGroundColor;
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes to console with custom foreGround and backGround options but also includes a way to disable new line
        /// </summary>
        public static void WriteLine(string text, ConsoleColor foreGroundColor, ConsoleColor backGroundColor,
            bool newline = true, params object[] args)
        {
            Console.ForegroundColor = foreGroundColor;
            Console.BackgroundColor = backGroundColor;
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
            Console.ResetColor();
        }

        public static void WriteInfoLine(string text, bool newline = true, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[INFO] ");
            Console.ResetColor();
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }

        public static void WriteFatalErrorLine(string text, bool newline = true, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[FATAL ERROR] ");
            Console.ResetColor();
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }

        public static void WriteErrorLine(string text, bool newline = true, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ResetColor();
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }

        public static void WriteWarningLine(string text, bool newline = true, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[WARNING] ");
            Console.ResetColor();
            if (args == null || args.Length == 0)
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }

        public static void WriteDebugLine(string text, bool newline = true, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[DEBUG] ");
            Console.ResetColor();
            if (args == null || args.Length == 0) 
                Console.Write(text + (newline ? "\n" : ""));
            else 
                Console.Write(text + (newline ? "\n" : ""), args);
        }
    }
}