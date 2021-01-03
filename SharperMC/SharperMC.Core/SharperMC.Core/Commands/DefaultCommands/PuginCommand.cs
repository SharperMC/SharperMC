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
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class PluginCommand : Command
    {
        public PluginCommand() : base("pl", new[] {"plugin", "plugins"}, "pl", "Gets plugins.")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            var enabledPlugins = PluginManager.Plugins.Where(o => o.Value == PluginState.Enabled);
            var enabledText = new ChatText($"Enabled Plugins: ", TextColor.Green);
            var enabledList = enabledPlugins as KeyValuePair<IPlugin, PluginState>[] ?? enabledPlugins.ToArray();
            enabledText.SetNext(new ChatText($"({enabledList.Count()}) ", TextColor.Reset));
            ChatText last = null;
            foreach (var pair in enabledList)
            {
                enabledText.SetNext(last = new ChatText($"{pair.Key.GetName()}, "));
            }
            if (last != null) last.Text = last.Text.Substring(0, last.Text.Length - 2) + ".";
            sender.SendChat(enabledText);
            
            
            var loadedPlugins = PluginManager.Plugins.Where(o => o.Value == PluginState.Loaded);
            var loadedText = new ChatText($"Loaded Plugins: ", TextColor.Gray);
            var loadedList = loadedPlugins as KeyValuePair<IPlugin, PluginState>[] ?? loadedPlugins.ToArray();
            loadedText.SetNext(new ChatText($"({loadedList.Count()}) ", TextColor.Reset));
            last = null;
            foreach (var pair in loadedList)
            {
                loadedText.SetNext(last = new ChatText($"{pair.Key.GetName()}, "));
            }
            if (last != null) last.Text = last.Text.Substring(0, last.Text.Length - 2) + ".";
            sender.SendChat(loadedText);
            
            
            var disabledPlugins = PluginManager.Plugins.Where(o => o.Value == PluginState.Disabled);
            var disabledText = new ChatText($"Disabled Plugins: ", TextColor.Red);
            var disabledList = disabledPlugins as KeyValuePair<IPlugin, PluginState>[] ?? disabledPlugins.ToArray();
            disabledText.SetNext(new ChatText($"({disabledList.Count()}) ", TextColor.Reset));
            last = null;
            foreach (var pair in disabledList)
            {
                disabledText.SetNext(last = new ChatText($"{pair.Key.GetName()}, "));
            }
            if (last != null) last.Text = last.Text.Substring(0, last.Text.Length - 2) + ".";
            sender.SendChat(disabledText);
            
            
            var failedPlugins = PluginManager.Plugins.Where(o => o.Value == PluginState.Failed);
            var failedText = new ChatText($"Failed Plugins: ", TextColor.DarkRed);
            var failedList = failedPlugins as KeyValuePair<IPlugin, PluginState>[] ?? failedPlugins.ToArray();
            failedText.SetNext(new ChatText($"({failedList.Count()}) ", TextColor.Reset));
            last = null;
            foreach (var pair in failedList)
            {
                failedText.SetNext(last = new ChatText($"{pair.Key.GetName()}, "));
            }
            if (last != null) last.Text = last.Text.Substring(0, last.Text.Length - 2) + ".";
            sender.SendChat(failedText);
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args)
        {
            return new string[0];
        }
    }
}