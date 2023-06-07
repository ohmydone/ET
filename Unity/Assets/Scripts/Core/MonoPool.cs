using System;
using System.Collections.Generic;

namespace ET
{
    public class MonoPool: IDisposable
    {
        private readonly Dictionary<Type, Queue<object>> pool = new Dictionary<Type, Queue<object>>();
        [StaticField]
        public static MonoPool Instance = new MonoPool();
        
        private MonoPool()
        {
        }

        public object Fetch(Type type)
        {
            Queue<object> queue = null;
            if (!pool.TryGetValue(type, out queue))
            {
                return Activator.CreateInstance(type);
            }

            if (queue.Count == 0)
            {
                return Activator.CreateInstance(type);
            }
            return queue.Dequeue();
        }

        public T Fetch<T>() where T: class
        {
            return Fetch(TypeInfo<T>.Type) as T;
        }
        
        
        public void Recycle(object obj)
        {
            Type type = obj.GetType();
            Queue<object> queue = null;
            if (!pool.TryGetValue(type, out queue))
            {
                queue = new Queue<object>();
                pool.Add(type, queue);
            }
            queue.Enqueue(obj);
        }

        public void Dispose()
        {
            this.pool.Clear();
        }
    }
}