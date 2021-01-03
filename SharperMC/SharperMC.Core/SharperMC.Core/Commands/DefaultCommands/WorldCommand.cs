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
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class WorldCommand : Command
    {
        public WorldCommand() : base("world", "world [world]", "Teleports you to a world.")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args, string origMessage)
        {
            if (sender.IsPlayer())
            {
                var player = (Player) sender;
                if (args.Length == 0)
                {
                    sender.SendChat($"Current level: {player.Level.LvlName}");
                    return;
                }
                var level = Globals.LevelManager.GetLevel(args[0]);
                if (level == null)
                {
                    sender.SendChat("That level doesn't exist!", TextColor.Red);
                    return;
                }
                
                Globals.LevelManager.TeleportToLevel(player, level);
            }
            else
            {
                ConsoleFunctions.WriteInfoLine("Cannot teleport console.");
            }
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args,
            string origMessage)
        {
            return new string[0];
        }
    }
}