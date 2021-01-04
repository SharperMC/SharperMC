using System;
using System.Collections.Generic;
using SharperMC.Core.Commands;
using SharperMC.Core.Events;
using SharperMC.Core.Events.DefaultEvents;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Console;

namespace ExamplePlugin
{
    /*
     * Be sure to have 
    
[assembly: PluginAttributes("ExamplePlugin.dll", "ExamplePlugin.PluginMain", "ExamplePlugin", "1.0", 
    "The SharperMC Team", "An example plugin for SharperMC.")]
    
     * in the AssemblyInfo.
     */
    public class PluginMain : SharperPlugin
    {
        private IPlugin _plugin;

        public override void Load()
        {
            ConsoleFunctions.WriteInfoLine("Loading!!!!!!");
            // EventManager.RegisterListener(new EventListener(), this);
        }

        public override void Enable()
        {
            ConsoleFunctions.WriteInfoLine("Enabling!!!!!!");
            CommandManager.AddCommand(new ExampleCommand("/"));
            var exampleSystem = new CommandSystem(new [] {"\\"});
            exampleSystem.AddCommand(new ExampleCommand("\\"));
            CommandManager.AddCommandSystem(exampleSystem);
        }

        public override void Disable()
        {
            ConsoleFunctions.WriteInfoLine("Disabling!!!!!!");
        }
    }

    public class EventListener
    {
        [EListener(typeof(CommandEvent))]
        public void OnCommand(CommandEvent e)
        {
            ConsoleFunctions.WriteInfoLine(
                $"OnCommand. Cmd: {e.Command.Name} EType: {e.Type} Label: {e.Label} Sender: {e.Sender.GetName()} " +
                $"Message: {{{e.Message}}}");
        }

        [EListener(typeof(CommandEvent), EventPriority.High)]
        public void HighOnCommand(CommandEvent e)
        {
            ConsoleFunctions.WriteInfoLine(
                $"HighOnCommand. Cmd: {e.Command.Name} EType: {e.Type} Label: {e.Label} Sender: {e.Sender.GetName()} " +
                $"Message: {{{e.Message}}}");
        }

        [EListener(typeof(CommandEvent), EventPriority.Low)]
        public void LowOnCommand(CommandEvent e)
        {
            ConsoleFunctions.WriteInfoLine(
                $"LowOnCommand. Cmd: {e.Command.Name} EType: {e.Type} Label: {e.Label} Sender: {e.Sender.GetName()} " +
                $"Message: {{{e.Message}}}");
        }

        [EListener(typeof(CommandPreExecutionEvent))]
        public void PreProcess(CommandPreExecutionEvent e)
        {
            ConsoleFunctions.WriteInfoLine(
                $"PreProcess. System: {e.System.GetType().Name} EType: {e.Type} Prefix: {e.Prefix} Sender: {e.Sender.GetName()} " +
                $"Message: {{{e.Message}}}");
        }

        [EListener(typeof(CommandPostExecutionEvent))]
        public void PostProcess(CommandPostExecutionEvent e)
        {
            ConsoleFunctions.WriteInfoLine(
                $"PostProcess. System: {e.System.GetType().Name} EType: {e.Type} Prefix: {e.Prefix} Sender: {e.Sender.GetName()} " +
                $"Message: {{{e.Message}}}");
        }
    }
    
    public class ExampleCommand : Command
    {
        public ExampleCommand(string prefix) :
            base("example", $"{prefix}example", "Example command")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args, string rawMessage)
        {
            sender.SendChat("Example command! RawMessage: " + rawMessage);
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args, string rawMessage)
        {
            return new[] {"example", "example2"};
        }
    }
}