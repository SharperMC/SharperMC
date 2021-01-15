using System;
using System.Collections.Generic;

namespace SharperMC.Core.Utils.Text
{
    public class TextAttribute
    {
        public readonly bool IsColor;
        public readonly char ChatCode;
        public readonly int HexCode;

        public readonly ConsoleColor ConsoleColor;

        public readonly string AnsiCode;
        public readonly string JsonKey;
        public readonly object JsonValue;

        // Initializer for non-colors.
        public TextAttribute(char chatCode, string ansiCode, string jsonKey) :
            this(chatCode, ansiCode, jsonKey, true)
        {
        }

        // Initializer for reset.
        public TextAttribute(char chatCode, string ansiCode, string jsonKey, object jsonValue)
        {
            IsColor = false;
            ChatCode = chatCode;
            AnsiCode = $"\x1b[{ansiCode}m";
            JsonKey = jsonKey;
            JsonValue = jsonValue;
        }

        // Initializer for colors.
        public TextAttribute(char chatCode, string ansiCode, int hexCode, ConsoleColor consoleColor, object jsonValue)
        {
            IsColor = true;
            AnsiCode = ansiCode;
            ChatCode = chatCode;
            HexCode = hexCode;
            ConsoleColor = consoleColor;
            AnsiCode = $"\x1b[0m\x1b[{ansiCode}m";
            JsonKey = "color";
            JsonValue = jsonValue;
        }
    }

    public static class TextColor
    {
        public static readonly TextAttribute
            Black = new TextAttribute('0', "30", 0x000000, ConsoleColor.Black, "black");

        public static readonly TextAttribute DarkBlue =
            new TextAttribute('1', "34", 0x0000AA, ConsoleColor.DarkBlue, "dark_blue");

        public static readonly TextAttribute DarkGreen =
            new TextAttribute('2', "32", 0x0AA00, ConsoleColor.DarkGreen, "dark_green");

        public static readonly TextAttribute DarkAqua =
            new TextAttribute('3', "36", 0x0AAAA, ConsoleColor.DarkCyan, "dark_aqua");

        public static readonly TextAttribute DarkRed =
            new TextAttribute('4', "31", 0xAA0000, ConsoleColor.DarkRed, "dark_red");

        public static readonly TextAttribute DarkPurple =
            new TextAttribute('5', "35", 0xAA00AA, ConsoleColor.DarkMagenta, "dark_purple");

        public static readonly TextAttribute Gold =
            new TextAttribute('6', "33", 0xFFAA00, ConsoleColor.DarkYellow, "gold");

        public static readonly TextAttribute Gray = new TextAttribute('7', "m\x1b[38;5;244", 0xAAAAAA,
            ConsoleColor.Gray,
            "gray");

        public static readonly TextAttribute DarkGray =
            new TextAttribute('8', "m\x1b[38;5;237", 0x555555, ConsoleColor.DarkGray, "dark_gray");

        public static readonly TextAttribute Blue = new TextAttribute('9', "94", 0x5555FF, ConsoleColor.Blue, "blue");

        public static readonly TextAttribute
            Green = new TextAttribute('a', "92", 0x55FF55, ConsoleColor.Green, "green");

        public static readonly TextAttribute Aqua = new TextAttribute('b', "96", 0x55FFFF, ConsoleColor.Cyan, "aqua");
        public static readonly TextAttribute Red = new TextAttribute('c', "91", 0xFF5555, ConsoleColor.Red, "red");

        public static readonly TextAttribute Purple =
            new TextAttribute('d', "95", 0xFF55FF, ConsoleColor.Magenta, "light_purple");

        public static readonly TextAttribute Yellow =
            new TextAttribute('e', "33", 0xFFFF55, ConsoleColor.Yellow, "yellow");

        public static readonly TextAttribute White = new TextAttribute('f', "0", 0xFFFFFF, ConsoleColor.White, "white");
        public static readonly TextAttribute Obfuscated = new TextAttribute('k', "5", "obfuscated");
        public static readonly TextAttribute Bold = new TextAttribute('l', "1", "bold");
        public static readonly TextAttribute Strikethrough = new TextAttribute('m', "9", "strikethrough");
        public static readonly TextAttribute Underline = new TextAttribute('n', "4", "underlined");
        public static readonly TextAttribute Italic = new TextAttribute('o', "3", "italic");
        public static readonly TextAttribute Reset = new TextAttribute('r', "0", "color", "reset");

        private static readonly Dictionary<char, TextAttribute> CharAttr = new Dictionary<char, TextAttribute>();

        static TextColor()
        {
            // Could've done this through reflection ig
            CharAttr.Add('0', Black);
            CharAttr.Add('1', DarkBlue);
            CharAttr.Add('2', DarkGreen);
            CharAttr.Add('3', DarkAqua);
            CharAttr.Add('4', DarkRed);
            CharAttr.Add('5', DarkPurple);
            CharAttr.Add('6', Gold);
            CharAttr.Add('7', Gray);
            CharAttr.Add('8', DarkGray);
            CharAttr.Add('9', Blue);
            CharAttr.Add('a', Aqua);
            CharAttr.Add('b', Green);
            CharAttr.Add('c', Red);
            CharAttr.Add('d', Purple);
            CharAttr.Add('e', Yellow);
            CharAttr.Add('f', White);
            CharAttr.Add('k', Obfuscated);
            CharAttr.Add('l', Bold);
            CharAttr.Add('m', Strikethrough);
            CharAttr.Add('n', Underline);
            CharAttr.Add('o', Italic);
            CharAttr.Add('r', Reset);
        }

        public static TextAttribute GetFromChar(char c)
        {
            return CharAttr[c];
        }

        public static bool IsColorChar(char c)
        {
            return "1234567890abcdefklmnor".IndexOf(c) != -1;
        }
    }
}