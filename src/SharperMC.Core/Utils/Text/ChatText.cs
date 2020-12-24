using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SharperMC.Core.Utils.Text
{
    public class ChatText //TODO: Add minecraft components like hover, click action, etc...
    {
        public string Text;

        public TextAttribute[] Colors;

        public ChatText Next;

        public ChatText(string text, params TextAttribute[] colors)
        {
            Text = text;
            Colors = colors;
        }

        public Dictionary<string, object> DictSerialize()
        {
            var data = Colors.ToDictionary(attr => attr.JsonKey, attr => attr.JsonValue);
            if (!string.IsNullOrEmpty(Text)) data.Add("text", Text);
            if (Next != null) data.Add("extra", Next.DictSerialize());
            return data;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(DictSerialize());
        }

        public void SetConsoleColor()
        {
            System.Console.ResetColor();
            foreach (var color in Colors)
                if (color.IsColor)
                    System.Console.ForegroundColor = color.ConsoleColor;
        }

        public List<ChatText> Split(int width, int len = 0)
        {
            var list = new List<ChatText>();
            var str = "";
            foreach (var c in Text)
            {
                if (c == '\n')
                {
                    len = 0;
                    list.Add(new ChatText(str, Colors));
                    str = "";
                }
                else
                {
                    len++;
                    if (len > width)
                    {
                        len = 0;
                        list.Add(new ChatText(str, Colors));
                        str = c.ToString();
                    }
                    else str += c;
                }
            }

            if (!string.IsNullOrEmpty(str)) list.Add(new ChatText(str, Colors));

            return list;
        }

        public List<ChatText> GetLines(int width)
        {
            var lines = new List<ChatText>();
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

        public void SetNext(ChatText next)
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
            return Colors.Aggregate("", (current, item) => current + item.AnsiCode) + Text;
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