using System;
using System.Collections.Generic;

namespace Stacker.Extensions.Utils
{

    class ChainEventQueue<T> where T : IChainEventable
    {

        #region Private variables

        private Queue<T> chainEventQueue;

        private Action onChainDoneCallback;

        #endregion

        #region Public properties

        public bool IsChainDone { get; private set; }

        #endregion

        #region Constructors

        public ChainEventQueue(int size)
        {
            chainEventQueue = new Queue<T>(size + 5); // Add a fixed buffer size.
        }

        public ChainEventQueue(IList<T> list)
        {
            chainEventQueue = new Queue<T>(list.Count + 5);

            AddRange(list);
        }

        #endregion

        #region Chain event methods

        public void AddRange(IList<T> list)
        {
            foreach (T item in list)
            {
                chainEventQueue.Enqueue(item);
            }
        }

        public void StartChain(Action onChainDoneCallback = null)
        {
            this.onChainDoneCallback = onChainDoneCallback;

            IsChainDone = false;

            NextInChain();
        }

        private void NextInChain()
        {
            if (chainEventQueue.Count > 0)
            {
                chainEventQueue.Dequeue().TriggerChainEvent(NextInChain);
            }
            else
            {
                FinishChain();
            }
        }

        public void FinishChain()
        {
            IsChainDone = true;

            chainEventQueue.Clear();

            onChainDoneCallback?.Invoke();
        }

        #endregion

    }

}
