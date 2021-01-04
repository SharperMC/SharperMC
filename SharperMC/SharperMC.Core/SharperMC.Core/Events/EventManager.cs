using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using SharperMC.Core.Events.DefaultEvents;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.CustomTypes;

namespace SharperMC.Core.Events
{
    public class EventManager
    {
        // TODO: Test
        public static readonly Dictionary<IPlugin, HashSet<RegisteredListener>> PluginListeners =
            new Dictionary<IPlugin, HashSet<RegisteredListener>>();

        public static readonly Dictionary<Type, OrderedDictionary<EventPriority, HashSet<RegisteredListener>>> Events =
            new Dictionary<Type, OrderedDictionary<EventPriority, HashSet<RegisteredListener>>>();

        public static void RegisterDefaultEvents()
        {
            RegisterEvent(typeof(PreChatEvent));
            RegisterEvent(typeof(ChatEvent));
            RegisterEvent(typeof(CommandPreExecutionEvent));
            RegisterEvent(typeof(CommandPostExecutionEvent));
            RegisterEvent(typeof(CommandEvent));
        }

        // Doesn't *have* to be an IEvent, but it should be. No support will be given if it's not an IEvent.
        public static void RegisterEvent(Type eventType)
        {
            Events.Add(eventType, NewOrderedDict());
        }

        public static OrderedDictionary<EventPriority, HashSet<RegisteredListener>> NewOrderedDict()
        {
            var dict = new OrderedDictionary<EventPriority, HashSet<RegisteredListener>>
            {
                {EventPriority.ReallyHigh, new HashSet<RegisteredListener>()},
                {EventPriority.High, new HashSet<RegisteredListener>()},
                {EventPriority.Medium, new HashSet<RegisteredListener>()},
                {EventPriority.Low, new HashSet<RegisteredListener>()},
                {EventPriority.ReallyLow, new HashSet<RegisteredListener>()},
                {EventPriority.Monitor, new HashSet<RegisteredListener>()}
            };
            return dict;
        }

        public static RegisteredListener RegisterListener(object listener, IPlugin plugin)
        {
            var registeredListener = new RegisteredListener(listener, plugin);
            if (PluginListeners.TryGetValue(plugin, out var list)) list.Add(registeredListener);
            else PluginListeners.Add(plugin, new HashSet<RegisteredListener> {registeredListener});
            return registeredListener;
        }

        public static void UnregisterListener(RegisteredListener listener)
        {
            PluginListeners[listener.Plugin].Remove(listener);
            foreach (var (key, value) in listener.Listeners) Events[key][value.Key].Remove(listener);
        }

        public static void UnregisterAllListeners()
        {
            foreach (var (key, _) in Events) Events[key] = NewOrderedDict();
        }

        public static void UnregisterListeners(IPlugin plugin)
        {
            if (!PluginListeners.TryGetValue(plugin, out var listeners)) return;
            foreach (var registeredListener in listeners) UnregisterListener(registeredListener);
        }

        public static Dictionary<Type, KeyValuePair<EventPriority, MethodInfo>> GetListeners(object listener)
        {
            var result = new Dictionary<Type, KeyValuePair<EventPriority, MethodInfo>>();
            foreach (var method in listener.GetType().GetMethods())
            {
                foreach (var attribute1 in method.GetCustomAttributes(typeof(EListener)))
                {
                    var attribute = (EListener) attribute1;
                    if (Events.ContainsKey(attribute.EventType))
                        result.Add(attribute.EventType,
                            new KeyValuePair<EventPriority, MethodInfo>(attribute.Priority, method));
                    else ConsoleFunctions.WriteWarningLine($"{attribute.EventType.FullName} is not registered!");
                }
            }

            return result;
        }

        public static void CallEvent(IEvent e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var type = e.GetType();
            if (!Events.TryGetValue(type, out var dict))
                throw new ArgumentException($"{e.GetType().FullName} is not registered!");
            foreach (var listener in dict.SelectMany(pair => pair.Value))
                listener.Listeners[type].Value.Invoke(listener.Listener, new object[] {e});
        }

        public class RegisteredListener
        {
            public readonly object Listener;
            public readonly IPlugin Plugin;
            public readonly Dictionary<Type, KeyValuePair<EventPriority, MethodInfo>> Listeners;

            public RegisteredListener(object listener, IPlugin plugin)
            {
                Listener = listener;
                Plugin = plugin;
                Listeners = GetListeners(listener);
            }
        }
    }
}