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
using System.Collections.Generic;
using System.Linq;
using SharperMC.Core.Commands.DefaultCommands;
using SharperMC.Core.Enums;

namespace SharperMC.Core.Commands
{
    public static class CommandManager
    {
        public const string CommandStart = "/";
        public static readonly Dictionary<string[], Command> CommandMap = new Dictionary<string[], Command>();

        static CommandManager()
        {
            // todo: teleport command
            // player.Teleport(new PlayerLocation(player.KnownPosition.X, 80, player.KnownPosition.Z));
            AddCommand(new GamemodeCommand());
            AddCommand(new HelpCommand());
            AddCommand(new SaveCommand());
            AddCommand(new StopCommand());
            AddCommand(new TestCommand());
            AddCommand(new WorldCommand());
        }

        public static void AddCommand(Command command)
        {
            var aliases = new string[command.Aliases.Length + 1];
            command.Aliases.CopyTo(aliases, 0);
            aliases[aliases.Length - 1] = command.Name;
            for (var i = 0; i < aliases.Length; i++)
                aliases[i] = aliases[i].ToLower();
            CommandMap.Add(aliases, command);
        }

        public static bool IsCommand(string command)
        {
            return command.StartsWith(CommandStart);
        }

        public static Command GetCommand(string label)
        {
            label = label.ToLower();
            return CommandMap.FirstOrDefault((s) => s.Key.Contains(label)).Value;
        }

        public static void ParseCommand(ICommandSender sender, string message)
        {
            try
            {
                message = message.Trim();
                while (message.Contains("  ")) message = message.Replace("  ", " ");
                var split = message.Split(' ');
                var command = GetCommand(split[0]);
                if (command == default(Command))
                {
                    UnknownCommand(sender, split[0]);
                    return;
                }

                string[] args;
                if (split.Length > 1)
                {
                    args = new string[split.Length - 1];
                    Array.Copy(split, 1, args, 0, split.Length - 1);
                }
                else args = new string[0];

                command.Execute(sender, split[0], args);
            }
            catch (Exception ex)
            {
                ConsoleFunctions.WriteWarningLine(ex.ToString());
                sender.SendChat("An error occured when executing command.", ChatColor.Red);
            }
        }

        public static void UnknownCommand(ICommandSender sender, string command)
        {
            // todo: customizable
            sender.SendChat("Unknown command: " + command, ChatColor.Red);
        }
    }
}