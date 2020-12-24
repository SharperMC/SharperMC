using System.Collections.Generic;
using System.Linq;

namespace SharperMC.Core.Utils.Console.Utils
{
    public class FancyText //TODO: Add minecraft components like hover, click action, etc...
    {
        public string Text;

        public FancyColor[] Colors;

        public FancyText Next;

        public FancyText(string text, params FancyColor[] colors)
        {
            Text = text;
            Colors = colors;
        }

        public void SetConsoleColor()
        {
            System.Console.ResetColor();
            foreach (var color in Colors)
                if (color.IsColor)
                    System.Console.ForegroundColor = color.ConsoleColor;
        }

        public List<FancyText> Split(int width, int len = 0)
        {
            var list = new List<FancyText>();
            var str = "";
            foreach (var c in Text)
            {
                if (c == '\n')
                {
                    len = 0;
                    list.Add(new FancyText(str, Colors));
                    str = "";
                }
                else
                {
                    len++;
                    if (len > width)
                    {
                        len = 0;
                        list.Add(new FancyText(str, Colors));
                        str = c.ToString();
                    }
                    else str += c;
                }
            }

            if (!string.IsNullOrEmpty(str)) list.Add(new FancyText(str, Colors));

            return list;
        }

        public List<FancyText> GetLines(int width)
        {
            var lines = new List<FancyText>();
            var text = this;
            do
            {
                var split = text.Split(width, lines.Count > 0 ? lines[lines.Count - 1].GetWidthRaw() : 0);
                if (split.Count == 0) continue;
                var first = split[0];

                if (lines.Count > 0)
                {
                    lines[lines.Count - 1].SetNext(first);
                    split.Remove(first);
                    lines.AddRange(split);
                }
                else lines.AddRange(split);
            } while ((text = text.Next) != null);

            return lines;
        }

        public void SetNext(FancyText next)
        {
            var t = this;
            while (t.Next != null) t = t.Next;
            t.Next = next;
        }

        public int GetWidth(int maxWidth)
        {
            return GetLines(maxWidth).Select(fancyText => fancyText.GetWidthRaw()).Concat(new[] {0}).Max();
        }

        public int GetWidthRaw()
        {
            var w = Text.Length;
            var t = this;
            while (t.Next != null)
            {
                t = t.Next;
                w += t.Text.Length;
            }

            return w;
        }

        public void PrintNext(bool consoleColors = false)
        {
            PrintText(consoleColors);
            Next?.PrintNext(consoleColors);
        }

        public void PrintText(bool consoleColors = false)
        {
            if (consoleColors) SetConsoleColor();
            System.Console.Write(consoleColors ? Text : GetConsoleStringRaw());
        }

        public string GetConsoleStringRaw()
        {
            return Colors.Aggregate("", (current, item) => current + item.PrintFunc) + Text;
        }

        public string GetConsoleString()
        {
            var s = "";
            var txt = this;
            do
            {
                s += txt.GetConsoleStringRaw();
            } while ((txt = txt.Next) != null);

            return s;
        }
    }
}