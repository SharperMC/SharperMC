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
using System.Linq;
using SharperMC.Core.Commands.DefaultCommands;
using SharperMC.Core.Events;
using SharperMC.Core.Events.DefaultEvents;

namespace SharperMC.Core.Commands
{
    public static class CommandManager
    {
        public static readonly ICommandSystem DefaultSystem = new CommandSystem(new[] {"/"});
        public static readonly List<ICommandSystem> CommandSystems = new List<ICommandSystem>();

        static CommandManager()
        {
            AddCommandSystem(DefaultSystem);
            AddCommand(new GamemodeCommand());
            AddCommand(new HelpCommand());
            AddCommand(new PluginCommand());
            AddCommand(new SaveCommand());
            AddCommand(new StopCommand());
            AddCommand(new TestCommand());
            AddCommand(new TPCommand());
            AddCommand(new TPSCommand());
            AddCommand(new WorldCommand());
            AddCommand(new ReloadCommand());
        }

        public static void AddCommandSystem(ICommandSystem commandSystem)
        {
            CommandSystems.Add(commandSystem);
        }

        public static void AddCommand(Command command)
        {
            DefaultSystem.AddCommand(command);
        }

        public static bool ShouldProcess(string command)
        {
            return ShouldProcess(command, DefaultSystem) ||
                   CommandSystems.Any(system => ShouldProcess(command, system));
        }

        public static bool ShouldProcess(string command, ICommandSystem system)
        {
            return system.GetPrefixes().Any(command.StartsWith);
        }

        public static void ParseCommand(ICommandSender sender, string message)
        {
            {
                var system = DefaultSystem;
                var prefix = system.GetPrefixes().FirstOrDefault(message.StartsWith);
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    var preE = new CommandPreExecutionEvent(sender, message, prefix, system);
                    EventManager.CallEvent(preE);
                    if (preE.Cancelled) return;
                    preE.System.ParseCommand(preE.Sender, preE.Message.Substring(prefix.Length));
                    var postE = new CommandPostExecutionEvent(preE.Sender, preE.Message, preE.Prefix, preE.System);
                    EventManager.CallEvent(postE);
                    return;
                }
            }
            foreach (var system in CommandSystems)
            {
                var prefix = system.GetPrefixes().FirstOrDefault(message.StartsWith);
                if (string.IsNullOrWhiteSpace(prefix)) continue;
                
                var preE = new CommandPreExecutionEvent(sender, message, prefix, system);
                EventManager.CallEvent(preE);
                if (preE.Cancelled) return;
                preE.System.ParseCommand(preE.Sender, preE.Message.Substring(prefix.Length));
                var postE = new CommandPostExecutionEvent(preE.Sender, preE.Message, preE.Prefix, preE.System);
                EventManager.CallEvent(postE);
                return;
            }
        }

        public static IEnumerable<string> ParseTab(ICommandSender sender, string message)
        {
            {
                var system = DefaultSystem;
                var prefix = system.GetPrefixes().FirstOrDefault(message.StartsWith);
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    return system.ParseTab(sender, message.Substring(prefix.Length));
                }
            }
            foreach (var system in CommandSystems)
            {
                var prefix = system.GetPrefixes().FirstOrDefault(message.StartsWith);
                if (string.IsNullOrWhiteSpace(prefix)) continue;
                return system.ParseTab(sender, message.Substring(prefix.Length));
            }

            return new string[0];
        }
    }
}