using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class OperaSystemAttribute:BaseAttribute
    {
        public string ActionName { get; }

        public OperaSystemAttribute(string actionName)
        {
            this.ActionName = actionName;
        }
        
    }
}
