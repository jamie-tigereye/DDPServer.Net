using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Net.DDP.Server;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    internal class ResultQueue<T>
    {
        private readonly ManualResetEvent _enqueuedEvent;
        private readonly IProcessQueues<T> _queueProcessor;
        private readonly Queue<T> _itemQueue;

        private Thread _workerThread;
        private T _currentItem;

        public ResultQueue(IProcessQueues<T> queueProcessor)
        {
            _itemQueue = new Queue<T>();
            _queueProcessor = queueProcessor;
            _enqueuedEvent = new ManualResetEvent(false);
            _workerThread = new Thread(DequeueItem);
            _workerThread.Start();
        }

        /// <summary>
        /// Adds an item to the queue for processing
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {
            lock (_itemQueue)
            {
                _itemQueue.Enqueue(item);
                _enqueuedEvent.Set();
            }
            RestartThread();
        }


        /// <summary>
        /// Processes an item from the queue
        /// </summary>
        /// <returns></returns>
        private bool Dequeue()
        {
            lock (_itemQueue)
            {
                if (_itemQueue.Count > 0)
                {
                    _enqueuedEvent.Reset();
                    _currentItem = _itemQueue.Dequeue();
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Restarts the thread used to dequeue items
        /// </summary>
        public void RestartThread()
        {
            if (_workerThread.ThreadState == ThreadState.Stopped)
            {
                _workerThread.Abort();
                _workerThread = new Thread(DequeueItem);
                _workerThread.Start();
            }
        }

        /// <summary>
        /// Processes an item from the queue
        /// </summary>
        /// <returns></returns>
        private void DequeueItem()
        {
            while (Dequeue())
            {
                _queueProcessor.ProcessItem(_currentItem);
            }
        }
    }
}

