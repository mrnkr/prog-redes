using System;

namespace Subarashii.PseudoRx
{
    public class Subscription
    {
        private Action Action { get; set; }

        public Subscription(Action unsubscribe)
        {
            Action = unsubscribe;
        }

        public void Unsubscribe()
        {
            Action();
        }
    }
}
