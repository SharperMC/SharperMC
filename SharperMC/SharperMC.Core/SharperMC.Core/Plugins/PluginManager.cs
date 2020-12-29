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
using System.IO;
using System.Linq;
using System.Reflection;
using SharperMC.Core.Utils.Console;

namespace SharperMC.Core.Plugins
{
    public class PluginManager
    {
        public static Dictionary<IPlugin, PluginState> Plugins { get; } = new Dictionary<IPlugin, PluginState>();

        public static void RegisterPlugins()
        {
            var dir = Directory.CreateDirectory("plugins");
            foreach (var fileInfo in dir.GetFiles()) AddPlugin(fileInfo.FullName);
        }

        public static void AddPlugin(string filePath)
        {
            // todo: improve this when I learn more abt .NET reflection.
            var assembly = Assembly.LoadFrom(filePath);
            var attributes = assembly.GetCustomAttributes(typeof(PluginAttributes), false).Cast<PluginAttributes>();
            foreach (var attribute in attributes)
            {
                try
                {
                    var module = assembly.GetModule(attribute.ModuleName);
                    if (module == null)
                    {
                        ConsoleFunctions.WriteWarningLine(
                            $"No module found in \"{filePath}\" at \"{attribute.ModuleName}\".");
                        break;
                    }

                    var pluginType = assembly.GetType(attribute.PluginPath);
                    if (pluginType == null)
                    {
                        ConsoleFunctions.WriteWarningLine(
                            $"No module found in \"{filePath}\" at \"{attribute.ModuleName}\".");
                        break;
                    }

                    dynamic obj = Activator.CreateInstance(pluginType);

                    if (obj == null)
                    {
                        ConsoleFunctions.WriteWarningLine($"{pluginType} is null when creating instance!");
                        break;
                    }

                    var plugin = obj as SharperPlugin;

                    if (plugin == null)
                    {
                        ConsoleFunctions.WriteWarningLine($"{obj} is not SharperPlugin instance!");
                        break;
                    }

                    plugin.Name = attribute.Name;
                    plugin.Description = attribute.Description;
                    plugin.Author = attribute.Author;
                    plugin.Version = attribute.Version;

                    AddPlugin(plugin);
                }
                catch (Exception ex)
                {
                    ConsoleFunctions.WriteWarningLine(
                        $"Failed to load plugin {attribute.Name} {attribute.Version} at " +
                        $"\"{attribute.PluginPath}\" at \"{attribute.ModuleName}\". {ex}");
                }
            }
        }

        public static void AddPlugin(IPlugin plugin)
        {
            plugin.Load();
            Plugins.Add(plugin, PluginState.Loaded);
            if (!Server.Initiated) return;
            try
            {
                plugin.Enable();
                Plugins[plugin] = PluginState.Enabled;
            }
            catch (Exception ex)
            {
                ConsoleFunctions.WriteErrorLine(
                    $"Failed to enable plugin: {plugin.GetName()} {plugin.GetVersion()}. {ex}");
            }
        }

        public static void EnableAllPlugins()
        {
            foreach (var pair in Plugins.ToDictionary(x => x.Key, x => x.Value))
            {
                try
                {
                    pair.Key.Enable();
                    Plugins[pair.Key] = PluginState.Enabled;
                }
                catch (Exception ex)
                {
                    ConsoleFunctions.WriteErrorLine(
                        $"Failed to enable plugin: {pair.Key.GetName()} {pair.Key.GetVersion()}. {ex}");
                }
            }
        }

        public static void DisableAllPlugins()
        {
            foreach (var pair in Plugins.ToDictionary(x => x.Key, x => x.Value))
            {
                try
                {
                    pair.Key.Disable();
                }
                catch (Exception ex)
                {
                    ConsoleFunctions.WriteErrorLine(
                        $"Failed to disable plugin: {pair.Key.GetName()} {pair.Key.GetVersion()}. {ex}");
                }
                Plugins[pair.Key] = PluginState.Disabled;
            }
        }
    }
}