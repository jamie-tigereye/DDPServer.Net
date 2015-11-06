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
        private static ManualResetEvent _enqueuedEvent;
        private static Thread _workerThread;
        private readonly Queue<T> _itemQueue;
        private T _currentItem;
        private readonly IProcessQueues<T> _queueProcessor;

        public ResultQueue(IProcessQueues<T> queueProcessor)
        {
            _itemQueue = new Queue<T>();
            _queueProcessor = queueProcessor;
            _enqueuedEvent = new ManualResetEvent(false);
            _workerThread = new Thread(DequeueItem);
            _workerThread.Start();
        }

        public void AddItem(T jsonItem)
        {
            lock (_itemQueue)
            {
                _itemQueue.Enqueue(jsonItem);
                _enqueuedEvent.Set();
            }
            RestartThread();
        }


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

        public void RestartThread()
        {
            if (_workerThread.ThreadState == ThreadState.Stopped)
            {
                _workerThread.Abort();
                _workerThread = new Thread(DequeueItem);
                _workerThread.Start();
            }
        }

        private void DequeueItem()
        {
            while (Dequeue())
            {
                _queueProcessor.ProcessItem(_currentItem);
            }
        }
    }
}

