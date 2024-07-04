using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class IOCContainer
    {
        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = instance.GetType();

            if (_instances.ContainsKey(key))
            {
                _instances[key] = instance;
            }
            else
            {
                _instances.Add(key, instance);
            }
        }

        public void UnRegister<T>(T instance)
        {
            var key = instance.GetType();

            if (_instances.ContainsKey(key))
                _instances.Remove(key);
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (_instances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }

        public List<T> GetAll<T>() where T : class
        {
            var key = typeof(T);
            List<T> found = new List<T>();

            foreach (var kvp in _instances)
            {
                if(kvp.Value is T value)
                    found.Add(value);
            }

            return found;
        }
    }
}