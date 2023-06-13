using System;

namespace ET
{
    public interface IOpera
    {
        
    }
    
    public interface IOperaSystem:ISystemType
    {
        void Run(object o,string actionName);
    }

    [ObjectSystem]
    public abstract class OperaSystem<T>: IOperaSystem where T : IOpera
    {
        public Type Type()
        {
            return TypeInfo<T>.Type;
        }

        public Type SystemType()
        {
            return TypeInfo<IOperaSystem>.Type;
        }

        public int GetInstanceQueueIndex()
        {
            return 0;
        }

        public void Run(object o, string actionName)
        {
           this.Run((T)o,actionName);
        }

        protected abstract void Run(T self,string actionName);
    }
}

