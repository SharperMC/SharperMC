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

using System.Collections.Generic;
using System.Text;

namespace SharperMC.Core.Utils.Text
{
    public static class TextUtils
    {
        public const char ColorChar = '\u00A7';

        public static string Colorize(string text)
        {
            return Colorize(text, "1234567890abcdefklmnor", 'r');
        }

        public static string Colorize(string text, char reset = 'r')
        {
            return Colorize(text, new[]
            {
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f', 'k', 'l', 'm', 'n', 'o',
                'r'
            }, reset);
        }

        public static string Colorize(string text, char[] allowed, char reset = 'r')
        {
            return Colorize(text, new string(allowed), reset);
        }

        public static string Colorize(string text, string allowed = "1234567890abcdefklmnor", char reset = 'r')
        {
            var output = text.ToCharArray();
            for (var i = 0; i < output.Length; i++)
            {
                if (output[i] == ColorChar) output[i] = '&';
                if (output[i] != '&') continue;
                var c = char.ToLower(text[i + 1]);
                if (!TextColor.IsColorChar(c) || allowed.IndexOf(c) == -1) continue;
                output[i] = ColorChar;
                if (c == 'r') output[i + 1] = reset;
            }

            return new string(output);
        }

        public static ChatText ToChatText(string text)
        {
            ChatText chatText = null;
            var list = new List<TextAttribute>();
            var sb = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c == ColorChar && i + 1 < text.Length && TextColor.IsColorChar(text[i + 1]))
                {
                    if (sb.Length > 0)
                    {
                        if (chatText == null) chatText = new ChatText(sb.ToString(), list.ToArray());
                        else chatText.SetNext(new ChatText(sb.ToString(), list.ToArray()));
                        sb.Clear();
                        list.Clear();
                    }
                    list.Add(TextColor.GetFromChar(text[++i]));
                }
                else sb.Append(c);
            }
            if (chatText == null) chatText = new ChatText(sb.ToString(), list.ToArray());
            else chatText.SetNext(new ChatText(sb.ToString(), list.ToArray()));

            return chatText;
        }
        
        public static string Format(string text, params object[] args)
        {
            return (args == null || args.Length == 0) ? text : string.Format(text, args);
        }

        public static ChatText NewFancyText(string prefix, TextAttribute color, string text)
        {
            return NewFancyText(prefix, color, new ChatText(text, TextColor.Reset));
        }

        public static ChatText NewFancyText(string prefix, TextAttribute color, ChatText text)
        {
            return new ChatText(prefix, color) {Next = text};
        }
    }
}