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

namespace SharperMC.Core
{
    internal static class ConsoleFunctions
    {
        /*
         * TODO: Probably a easier way to add parameter arguments, should work for now 
         */
        public static void WriteLine(string text, params object[] args)
        {
            if (args == null)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }

        public static void WriteLine(string text, ConsoleColor foreGroundColor, params object[] args)
        {
            Console.ForegroundColor = foreGroundColor;
            if (args == null)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
            Console.ResetColor();
        }

        public static void WriteLine(string text, ConsoleColor foreGroundColor, ConsoleColor backGroundColor,
            params object[] args)
        {
            Console.ForegroundColor = foreGroundColor;
            Console.BackgroundColor = backGroundColor;
            if (args == null)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
            Console.ResetColor();
        }

        public static void WriteInfoLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[INFO] ");
            Console.ResetColor();
            Console.Write(text + "\n");
        }

        public static void WriteInfoLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[INFO] ");
            Console.ResetColor();
            if (args == null)
                Console.Write(text + "\n");
            else
                Console.Write(text + "\n", args);
        }

        public static void WriteFatalErrorLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[FATAL ERROR] ");
            Console.ResetColor();
            if (args == null)
                Console.Write(text + "\n");
            else
                Console.Write(text + "\n", args);
        }

        public static void WriteErrorLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ResetColor();
            if (args == null)
                Console.Write(text + "\n");
            else
                Console.Write(text + "\n", args);
        }

        public static void WriteWarningLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[WARNING] ");
            Console.ResetColor();
            if (args == null)
                Console.Write(text + "\n");
            else
                Console.Write(text + "\n", args);
        }

        public static void WriteServerLine(string text, params object[] args)
        {
            WriteInfoLine(text, args);
        }

        public static void WriteDebugLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[DEBUG] ");
            Console.ResetColor();
            if (args == null)
                Console.Write(text + "\n");
            else
                Console.Write(text + "\n", args);
        }
    }
}