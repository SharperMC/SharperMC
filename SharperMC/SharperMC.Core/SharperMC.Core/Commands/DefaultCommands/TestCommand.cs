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
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class TestCommand : Command
    {
        public TestCommand() : base("test", new[] {"tester", "testing"}, "test", "Test command.")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args, string origMessage)
        {
            if (args.Length > 0 && args[0].ToLower().Equals("send"))
            {
                sender.SendChat(new ChatText("Black ", TextColor.Black)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow)
                                                                        {
                                                                            Next = new ChatText("White ",
                                                                                TextColor.White)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                sender.SendChat(new ChatText("Black ", TextColor.Black, TextColor.Bold)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue, TextColor.Bold)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen, TextColor.Bold)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua, TextColor.Bold)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed, TextColor.Bold)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple, TextColor.Bold)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold, TextColor.Bold)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray, TextColor.Bold)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray, TextColor.Bold)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue, TextColor.Bold)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green, TextColor.Bold)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua, TextColor.Bold)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red,
                                                                    TextColor.Bold)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple,
                                                                        TextColor.Bold)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow,
                                                                            TextColor.Bold)
                                                                        {
                                                                            Next = new ChatText("White Bold",
                                                                                TextColor.White, TextColor.Bold)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                sender.SendChat(new ChatText("Black ", TextColor.Black, TextColor.Italic)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue, TextColor.Italic)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen, TextColor.Italic)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua, TextColor.Italic)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed, TextColor.Italic)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple, TextColor.Italic)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold, TextColor.Italic)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray, TextColor.Italic)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray, TextColor.Italic)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue, TextColor.Italic)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green, TextColor.Italic)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua,
                                                                TextColor.Italic)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red,
                                                                    TextColor.Italic)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple,
                                                                        TextColor.Italic)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow,
                                                                            TextColor.Italic)
                                                                        {
                                                                            Next = new ChatText("White Italic",
                                                                                TextColor.White, TextColor.Italic)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                sender.SendChat(new ChatText("Black ", TextColor.Black, TextColor.Underline)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue, TextColor.Underline)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen, TextColor.Underline)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua, TextColor.Underline)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed, TextColor.Underline)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple, TextColor.Underline)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold, TextColor.Underline)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray, TextColor.Underline)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray,
                                                    TextColor.Underline)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue, TextColor.Underline)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green,
                                                            TextColor.Underline)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua,
                                                                TextColor.Underline)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red,
                                                                    TextColor.Underline)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple,
                                                                        TextColor.Underline)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow,
                                                                            TextColor.Underline)
                                                                        {
                                                                            Next = new ChatText("White Underline",
                                                                                TextColor.White, TextColor.Underline)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                sender.SendChat(new ChatText("Black ", TextColor.Black, TextColor.Strikethrough)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue, TextColor.Strikethrough)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen, TextColor.Strikethrough)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua, TextColor.Strikethrough)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed, TextColor.Strikethrough)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple, TextColor.Strikethrough)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold, TextColor.Strikethrough)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray, TextColor.Strikethrough)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray,
                                                    TextColor.Strikethrough)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue,
                                                        TextColor.Strikethrough)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green,
                                                            TextColor.Strikethrough)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua,
                                                                TextColor.Strikethrough)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red,
                                                                    TextColor.Strikethrough)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple,
                                                                        TextColor.Strikethrough)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow,
                                                                            TextColor.Strikethrough)
                                                                        {
                                                                            Next = new ChatText("White Strikethrough",
                                                                                TextColor.White,
                                                                                TextColor.Strikethrough)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                sender.SendChat(new ChatText("Black ", TextColor.Black, TextColor.Obfuscated)
                {
                    Next = new ChatText("DarkBlue ", TextColor.DarkBlue, TextColor.Obfuscated)
                    {
                        Next = new ChatText("DarkGreen ", TextColor.DarkGreen, TextColor.Obfuscated)
                        {
                            Next = new ChatText("DarkAqua ", TextColor.DarkAqua, TextColor.Obfuscated)
                            {
                                Next = new ChatText("DarkRed ", TextColor.DarkRed, TextColor.Obfuscated)
                                {
                                    Next = new ChatText("DarkPurple ", TextColor.DarkPurple, TextColor.Obfuscated)
                                    {
                                        Next = new ChatText("Gold ", TextColor.Gold, TextColor.Obfuscated)
                                        {
                                            Next = new ChatText("Gray ", TextColor.Gray, TextColor.Obfuscated)
                                            {
                                                Next = new ChatText("DarkGray ", TextColor.DarkGray,
                                                    TextColor.Obfuscated)
                                                {
                                                    Next = new ChatText("Blue ", TextColor.Blue, TextColor.Obfuscated)
                                                    {
                                                        Next = new ChatText("Green ", TextColor.Green,
                                                            TextColor.Obfuscated)
                                                        {
                                                            Next = new ChatText("Aqua ", TextColor.Aqua,
                                                                TextColor.Obfuscated)
                                                            {
                                                                Next = new ChatText("Red ", TextColor.Red,
                                                                    TextColor.Obfuscated)
                                                                {
                                                                    Next = new ChatText("Purple ", TextColor.Purple,
                                                                        TextColor.Obfuscated)
                                                                    {
                                                                        Next = new ChatText("Yellow ", TextColor.Yellow,
                                                                            TextColor.Obfuscated)
                                                                        {
                                                                            Next = new ChatText("White Obfuscated",
                                                                                TextColor.White, TextColor.Obfuscated)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }

            var joinedArgs = string.Join(",", args);
            sender.SendChat($"Label: {label} Args: {{{joinedArgs}}}");
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args,
            string origMessage)
        {
            return args.Length > 0 ? args : new[] {"Hey", "Hello", "Howdy"};
        }
    }
}