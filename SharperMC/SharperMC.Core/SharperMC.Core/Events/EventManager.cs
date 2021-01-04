using System;
using System.Collections.Generic;
using System.Reflection;
using SharperMC.Core.Events.DefaultEvents;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.CustomTypes;

namespace SharperMC.Core.Events
{
    public static class EventManager
    {
        public static readonly Dictionary<IPlugin, HashSet<RegisteredListener>> PluginListeners =
            new Dictionary<IPlugin, HashSet<RegisteredListener>>();

        public static readonly Dictionary<Type, OrderedDictionary<EventPriority, HashSet<MethodListener>>> Events =
            new Dictionary<Type, OrderedDictionary<EventPriority, HashSet<MethodListener>>>();

        internal static void RegisterDefaultEvents()
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

        public static OrderedDictionary<EventPriority, HashSet<MethodListener>> NewOrderedDict()
        {
            var dict = new OrderedDictionary<EventPriority, HashSet<MethodListener>>
            {
                {EventPriority.ReallyLow, new HashSet<MethodListener>()},
                {EventPriority.Low, new HashSet<MethodListener>()},
                {EventPriority.Medium, new HashSet<MethodListener>()},
                {EventPriority.High, new HashSet<MethodListener>()},
                {EventPriority.ReallyHigh, new HashSet<MethodListener>()},
                {EventPriority.Monitor, new HashSet<MethodListener>()}
            };
            return dict;
        }

        public static RegisteredListener RegisterListener(object obj, IPlugin plugin)
        {
            var registeredListener = new RegisteredListener(obj, plugin);
            if (PluginListeners.TryGetValue(plugin, out var list)) list.Add(registeredListener);
            else PluginListeners.Add(plugin, new HashSet<RegisteredListener> {registeredListener});
            foreach (var (type, listeners) in registeredListener.Listeners)
            {
                foreach (var listener in listeners)
                {
                    if (Events.TryGetValue(type, out var dict))
                    {
                        dict[listener.Priority].Add(listener);
                    }
                    else
                    {
                        ConsoleFunctions.WriteErrorLine($"Type {type.Name} is not registered!");
                        break;
                    }
                }
            }
            return registeredListener;
        }

        public static void UnregisterListener(RegisteredListener listener)
        {
            PluginListeners[listener.Plugin].Remove(listener);
            foreach (var (type, listeners) in listener.Listeners)
            foreach (var methodListener in listeners)
                Events[type][methodListener.Priority].Remove(methodListener);
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

        public static Dictionary<Type, List<MethodListener>> GetListeners(object obj, RegisteredListener listener)
        {
            var result = new Dictionary<Type, List<MethodListener>>();
            foreach (var method in obj.GetType().GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            {
                foreach (var attribute1 in method.GetCustomAttributes(typeof(EListener)))
                {
                    var attribute = (EListener) attribute1;
                    if (Events.ContainsKey(attribute.EventType))
                    {
                        if (result.TryGetValue(attribute.EventType, out var val))
                            val.Add(new MethodListener(attribute.Priority, method, listener));
                        else
                            result.Add(attribute.EventType,
                                new List<MethodListener>()
                                {
                                    new MethodListener(attribute.Priority, method, listener)
                                });
                    }
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
            foreach (var listeners in dict.Values)
            {
                foreach (var listener in listeners)
                {
                    listener.Method.Invoke(listener.Listener.Object, new object[] {e});
                }
            }
        }

        public class RegisteredListener
        {
            public readonly object Object;
            public readonly IPlugin Plugin;
            public readonly Dictionary<Type, List<MethodListener>> Listeners;

            public RegisteredListener(object obj, IPlugin plugin)
            {
                Object = obj;
                Plugin = plugin;
                Listeners = GetListeners(obj, this);
            }
        }

        public class MethodListener
        {
            public readonly EventPriority Priority;
            public readonly MethodInfo Method;
            public readonly RegisteredListener Listener;

            public MethodListener(EventPriority priority, MethodInfo method, RegisteredListener listener)
            {
                Priority = priority;
                Method = method;
                Listener = listener;
            }
        }
    }
}