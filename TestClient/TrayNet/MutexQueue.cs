using System;
using System.Collections.Generic;

namespace tray.net
{
    public class MutexQueue<T> 
    {
        public MutexQueue()
        {
            dataQueue = new Queue<T>();
            mutex = new Mutex(false, "BufferMutex");
        }

        public void Enqueue(T data)
        {
            mutex.WaitOne();    
            dataQueue.Enqueue(data); 
            mutex.ReleaseMutex();
        }

        public T Dequeue()
        {
            mutex.WaitOne();
            T data = dataQueue.Dequeue();
            mutex.ReleaseMutex();   
            return data;
        }

        public int Count()
        {
            return dataQueue.Count; 
        }

        private Queue<T> dataQueue;
        private Mutex mutex;
    }
}
