using System;
using System.Collections.Generic;


namespace OOO.Utils
{
    public class EventHub
    {
        /* Singleton */
        private static EventHub instance = null;
        public static EventHub Instance {
            get {
                if (instance == null) {
                    instance = new EventHub();
                }
                return instance;
            }
        }

        public delegate void EventDelegate<T> (T e) where T : GameEvent;
        private Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();

        public void AddListener<T> (EventDelegate<T> listener) where T: GameEvent
        {
            if (delegates.TryGetValue(typeof(T), out var containedDelegate)) {
                delegates[typeof(T)] = Delegate.Combine(containedDelegate, listener);
            }  else {
                delegates[typeof(T)] = listener;
            }
        }

        public void RemoveListener<T> (EventDelegate<T> listener) where T: GameEvent
        {
            if (delegates.TryGetValue(typeof(T), out var containedDelagate)) {
                var current = Delegate.Remove(containedDelagate, listener);

                if (current != null) {
                    delegates.Remove(typeof(T));
                } else {
                    delegates[typeof(T)] = current;
                }
            }
        }

        public void FireEvent<T> (T gameEvent) where T : GameEvent
        {
            if (gameEvent == null) {
                throw new NullReferenceException();
            }

            if (delegates.TryGetValue(typeof(T), out Delegate containedDelegate)) {

                if (containedDelegate is EventDelegate<T> callback) {
                    callback(gameEvent);
                }
            }
        }
    }}