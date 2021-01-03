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
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class GamemodeCommand : Command
    {

        public GamemodeCommand() : base("gamemode", new []{"gm"}, "gamemode [gamemode]", "Sets gamemode.")
        {
        }
        
        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            if (sender.IsPlayer())
            {
                if(args.Length == 0)
                    sender.SendChat("Your current gamemode is [" + ((Player) sender).Gamemode + "]", TextColor.Gray);
                else if(args.Length >= 1)
                    switch (args[0].ToLower())
                    {
                        case "0":
                        case "s":
                        case "survival":
                            ((Player) sender).SetGamemode(Gamemode.Survival);
                            break;
                        case "3":
                        case "sp":
                        case "spectator":
                            ((Player) sender).SetGamemode(Gamemode.Spectator);
                            break;
                        case "2":
                        case "a":
                        case "adventure":
                            ((Player) sender).SetGamemode(Gamemode.Adventure);
                            break;
                        default:
                            ((Player) sender).SetGamemode(Gamemode.Creative);
                            break;
                    }
                sender.SendChat($"Set your gamemode to [{((Player) sender).Gamemode}]", TextColor.Green);
            }
            else
            {
                ConsoleFunctions.WriteInfoLine("Cannot change gamemode through console.");
            }
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args)
        {
            return args.Length == 0 ? new[] {"survival", "spectator", "adventure", "creative"} : new string[0];
        }
    }
}