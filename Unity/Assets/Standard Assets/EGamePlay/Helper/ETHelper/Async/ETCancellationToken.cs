using System;
using System.Collections.Generic;

namespace EGamePlay
{
    public class ETCancellationToken
    {
        private Action action;

        public void Register(Action callback)
        {
            this.action = callback;
        }

        public void Cancel()
        {
            action.Invoke();
        }
    }
}