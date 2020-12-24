using System;

namespace SharperMC.Core.Utils.Console.Utils
{
    public class FancyColor
    {
        public static readonly FancyColor Black = new FancyColor('0', "30", 0x000000, ConsoleColor.Black);
        public static readonly FancyColor DarkBlue = new FancyColor('1', "34", 0x0000AA, ConsoleColor.DarkBlue);
        public static readonly FancyColor DarkGreen = new FancyColor('2', "32", 0x0AA00, ConsoleColor.DarkGreen);
        public static readonly FancyColor DarkAqua = new FancyColor('3', "36", 0x0AAAA, ConsoleColor.DarkCyan);
        public static readonly FancyColor DarkRed = new FancyColor('4', "31", 0xAA0000, ConsoleColor.DarkRed);
        public static readonly FancyColor DarkPurple = new FancyColor('5', "35", 0xAA00AA, ConsoleColor.DarkMagenta);
        public static readonly FancyColor Gold = new FancyColor('6', "33", 0xFFAA00, ConsoleColor.DarkYellow);
        public static readonly FancyColor Gray = new FancyColor('7', "m\x1b[38;5;244", 0xAAAAAA, ConsoleColor.Gray);

        public static readonly FancyColor DarkGray =
            new FancyColor('8', "m\x1b[38;5;237", 0x555555, ConsoleColor.DarkGray);

        public static readonly FancyColor Blue = new FancyColor('9', "94", 0x5555FF, ConsoleColor.Blue);
        public static readonly FancyColor Green = new FancyColor('a', "92", 0x55FF55, ConsoleColor.Green);
        public static readonly FancyColor Aqua = new FancyColor('b', "96", 0x55FFFF, ConsoleColor.Cyan);
        public static readonly FancyColor Red = new FancyColor('c', "91", 0xFF5555, ConsoleColor.Red);
        public static readonly FancyColor Purple = new FancyColor('d', "95", 0xFF55FF, ConsoleColor.Magenta);
        public static readonly FancyColor Yellow = new FancyColor('e', "33", 0xFFFF55, ConsoleColor.Yellow);
        public static readonly FancyColor White = new FancyColor('f', "0", 0xFFFFFF, ConsoleColor.White);
        public static readonly FancyColor Obfuscated = new FancyColor('k', "5");
        public static readonly FancyColor Bold = new FancyColor('l', "1");
        public static readonly FancyColor Strikethrough = new FancyColor('m', "9");
        public static readonly FancyColor Underline = new FancyColor('n', "4");
        public static readonly FancyColor Italic = new FancyColor('o', "3");
        public static readonly FancyColor Reset = new FancyColor('r', "0");
        public readonly bool IsColor;
        public readonly char ChatCode;
        public readonly int HexCode;

        public readonly ConsoleColor ConsoleColor;

        // public readonly string PrintCol;
        public readonly string PrintFunc;

        private FancyColor(char chatCode, string printFunc)
        {
            IsColor = false;
            ChatCode = chatCode;
            // PrintCol = printFunc;
            PrintFunc = $"\x1b[{printFunc}m";
        }

        private FancyColor(char chatCode, string printFunc, int hexCode, ConsoleColor consoleColor)
        {
            IsColor = true;
            PrintFunc = printFunc;
            ChatCode = chatCode;
            HexCode = hexCode;
            ConsoleColor = consoleColor;
            // PrintCol = printFunc;
            PrintFunc = $"\x1b[0m\x1b[{printFunc}m";
        }
    }
}