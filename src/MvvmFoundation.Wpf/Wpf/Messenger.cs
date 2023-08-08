using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MvvmFoundation.Wpf
{
    /// <summary>
    /// Provides loosely-coupled messaging between various colleague objects.  All references
    /// to objects are stored weakly, to prevent memory leaks.
    /// </summary>
    public class Messenger
    {
        /// <summary>
        /// Registers a callback method, with no parameter, to be invoked when a specific message is broadcast.
        /// </summary>
        /// <param name="message">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcast.</param>
        public void Register(string message, Action callback)
        {
            this.Register(message, callback, null);
        }

        /// <summary>
        /// Registers a callback method, with a parameter, to be invoked when a specific message is broadcast.
        /// </summary>
        /// <param name="message">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcast.</param>
        public void Register<T>(string message, Action<T> callback)
        {
            this.Register(message, callback, typeof(T));
        }

        private void Register(string message, Delegate callback, Type parameterType)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("'message' cannot be null or empty.");
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            this.VerifyParameterType(message, parameterType);

            _messageToActionsMap.AddAction(message, callback.Target, callback.Method, parameterType);
        }

        [Conditional("DEBUG")]
        private void VerifyParameterType(string message, Type parameterType)
        {
            if (_messageToActionsMap.TryGetParameterType(message, out var previouslyRegisteredParameterType))
            {
                if (previouslyRegisteredParameterType != null && parameterType != null)
                {
                    if (!previouslyRegisteredParameterType.Equals(parameterType))
                    {
                        throw new InvalidOperationException(
                            $"The registered action's parameter type is inconsistent with the previously registered actions for message '{message}'.\nExpected: {previouslyRegisteredParameterType.FullName}\nAdding: {parameterType.FullName}");
                    }
                }
                else
                {
                    // One, or both, of previouslyRegisteredParameterType or callbackParameterType are null.
                    if (previouslyRegisteredParameterType != parameterType)   // not both null?
                    {
                        throw new TargetParameterCountException(
                            $"The registered action has a number of parameters inconsistent with the previously registered actions for message \"{message}\".\nExpected: {(previouslyRegisteredParameterType == null ? 0 : 1)}\nAdding: {(parameterType == null ? 0 : 1)}");
                    }
                }
            }
        }

        /// <summary>
        /// Notifies all registered parties that a message is being broadcast.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <param name="parameter">The parameter to pass together with the message.</param>
        public void NotifyColleagues(string message, object parameter)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("'message' cannot be null or empty.");
            }

            if (_messageToActionsMap.TryGetParameterType(message, out var registeredParameterType))
            {
                if (registeredParameterType == null)
                {
                    throw new TargetParameterCountException(
                        $"Cannot pass a parameter with message '{message}'. Registered action(s) expect no parameter.");
                }
            }

            var actions = _messageToActionsMap.GetActions(message);

            if (actions != null)
            {
                actions.ForEach(action => action.DynamicInvoke(parameter));
            }
        }

        /// <summary>
        /// Notifies all registered parties that a message is being broadcast.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        public void NotifyColleagues(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("'message' cannot be null or empty.");
            }

            if (_messageToActionsMap.TryGetParameterType(message, out var registeredParameterType))
            {
                if (registeredParameterType != null)
                {
                    throw new TargetParameterCountException(
                        $"Must pass a parameter of type {registeredParameterType.FullName} with this message. Registered action(s) expect it.");
                }
            }

            var actions = _messageToActionsMap.GetActions(message);

            if (actions != null)
            {
                actions.ForEach(action => action.DynamicInvoke());
            }
        }

        /// <summary>
        /// This class is an implementation detail of the Messenger class.
        /// </summary>
        private class MessageToActionsMap
        {
            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="message">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            /// <param name="method">The method to invoke.</param>
            /// <param name="actionType">The type of the Action delegate.</param>
            internal void AddAction(string message, object target, MethodInfo method, Type actionType)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                if (method == null)
                {
                    throw new ArgumentNullException(nameof(method));
                }

                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                    {
                        _map[message] = new List<WeakAction>();
                    }

                    _map[message].Add(new WeakAction(target, method, actionType));
                }
            }

            /// <summary>
            /// Gets the list of actions to be invoked for the specified message
            /// </summary>
            /// <param name="message">The message to get the actions for</param>
            /// <returns>Returns a list of actions that are registered to the specified message</returns>
            internal List<Delegate> GetActions(string message)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                List<Delegate> actions;

                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                    {
                        return null;
                    }

                    var weakActions = _map[message];
                    actions = new List<Delegate>(weakActions.Count);

                    for (int i = weakActions.Count - 1; i > -1; --i)
                    {
                        var weakAction = weakActions[i];

                        if (weakAction == null)
                        {
                            continue;
                        }

                        var action = weakAction.CreateAction();

                        if (action != null)
                        {
                            actions.Add(action);
                        }
                        else
                        {
                            // The target object is dead, so get rid of the weak action.
                            weakActions.Remove(weakAction);
                        }
                    }

                    // Delete the list from the map if it is now empty.
                    if (weakActions.Count == 0)
                    {
                        _map.Remove(message);
                    }
                }

                // Reverse the list to ensure the callbacks are invoked in the order they were registered.
                actions.Reverse();

                return actions;
            }

            /// <summary>
            /// Get the parameter type of the actions registered for the specified message.
            /// </summary>
            /// <param name="message">The message to check for actions.</param>
            /// <param name="parameterType">
            /// When this method returns, contains the type for parameters 
            /// for the registered actions associated with the specified message, if any; otherwise, null.
            /// This will also be null if the registered actions have no parameters.
            /// This parameter is passed uninitialized.
            /// </param>
            /// <returns>true if any actions were registered for the message</returns>
            internal bool TryGetParameterType(string message, out Type parameterType)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                parameterType = null;
                List<WeakAction> weakActions;

                lock (_map)
                {
                    if (!_map.TryGetValue(message, out weakActions) || weakActions.Count == 0)
                    {
                        return false;
                    }
                }

                parameterType = weakActions[0].ParameterType;
                return true;
            }

            // Stores a hash where the key is the message and the value is the list of callbacks to invoke.
            readonly Dictionary<string, List<WeakAction>> _map = new();
        }

        /// <summary>
        /// This class is an implementation detail of the MessageToActionsMap class.
        /// </summary>
        private class WeakAction
        {
            /// <summary>
            /// Constructs a WeakAction.
            /// </summary>
            /// <param name="target">The object on which the target method is invoked, or null if the method is static.</param>
            /// <param name="method">The MethodInfo used to create the Action.</param>
            /// <param name="parameterType">The type of parameter to be passed to the action. Pass null if there is no parameter.</param>
            internal WeakAction(object target, MethodInfo method, Type parameterType)
            {
                _targetRef = target == null ? null : new WeakReference(target);

                _method = method;

                this.ParameterType = parameterType;

                if (parameterType == null)
                {
                    _delegateType = typeof(Action);
                }
                else
                {
                    _delegateType = typeof(Action<>).MakeGenericType(parameterType);
                }
            }

            /// <summary>
            /// Creates a "throw away" delegate to invoke the method on the target, or null if the target object is dead.
            /// </summary>
            internal Delegate CreateAction()
            {
                // Rehydrate into a real Action object, so that the method can be invoked.
                if (_targetRef == null)
                {
                    return Delegate.CreateDelegate(_delegateType, _method);
                }

                try
                {
                    object? target = _targetRef.Target;

                    if (target != null)
                    {
                        return Delegate.CreateDelegate(_delegateType, target, _method);
                    }
                }
                catch
                {
                    // Eat exception.
                }

                return null;
            }

            internal readonly Type ParameterType;

            private readonly Type _delegateType;
            private readonly MethodInfo _method;
            private readonly WeakReference _targetRef;
        }

        readonly MessageToActionsMap _messageToActionsMap = new();
    }
}