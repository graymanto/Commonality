using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commonality.Main.Concurrency
{
    /// <summary>
    /// Implementation of a producer consumer queue utilising a blocking collection.
    /// </summary>
    public class ProducerConsumerQueue : IDisposable
    {
        private readonly BlockingCollection<Task> _taskQueue;

        private readonly List<Task> _workerTasks = new List<Task>();

        public ProducerConsumerQueue(int workerCount, TaskCreationOptions options = TaskCreationOptions.LongRunning)
        {
            _taskQueue = new BlockingCollection<Task>();
            StartConsumers(workerCount, options);
        }

        public int QueueLength
        {
            get
            {
                return _taskQueue.Count;
            }
        }

        public bool IsAddingCompleted
        {
            get
            {
                return _taskQueue.IsAddingCompleted;
            }
        }

        public ProducerConsumerQueue(
            int workerCount, 
            int queueLimit, 
            TaskCreationOptions options = TaskCreationOptions.LongRunning)
        {
            _taskQueue = new BlockingCollection<Task>(queueLimit);
            StartConsumers(workerCount, options);
        }

        public Task Enqueue(Action action, CancellationToken cancelToken = default (CancellationToken))
        {
            var task = new Task(action, cancelToken);
            _taskQueue.Add(task, cancelToken);
            return task;
        }

        public Task<TResult> Enqueue<TResult>(
            Func<TResult> func, 
            CancellationToken cancelToken = default (CancellationToken))
        {
            var task = new Task<TResult>(func, cancelToken);
            _taskQueue.Add(task, cancelToken);
            return task;
        }

        public Task<TResult> Enqueue<TResult>(Task<TResult> task)
        {
            _taskQueue.Add(task);
            return task;
        }

        public Task Enqueue(Task task)
        {
            _taskQueue.Add(task);
            return task;
        }

        public void BlockUntilQueueCompletion()
        {
            _taskQueue.CompleteAdding();

            Task.WaitAll(_workerTasks.ToArray());
            Task.WaitAll(_taskQueue.ToArray());
        }

        public void BlockAdding()
        {
            _taskQueue.CompleteAdding();
        }

        public void Dispose()
        {
            _taskQueue.CompleteAdding();
        }

        private void Consume()
        {
            foreach (var task in _taskQueue.GetConsumingEnumerable())
            {
                try
                {
                    if (!task.IsCanceled)
                    {
                        task.RunSynchronously();
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private void StartConsumers(int workerCount, TaskCreationOptions options)
        {
            for (var i = 0; i < workerCount; i++)
            {
                _workerTasks.Add(Task.Factory.StartNew(Consume, options));
            }
        }
    }
}