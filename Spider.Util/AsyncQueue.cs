using System;
using System.Collections.Generic;
using Spider.Util;

namespace Spider.Util
{
    public class AsyncQueue
    {
        private IList<AsyncObject> queue = new List<AsyncObject>();
        private class AsyncObject
        {
            private IAsyncResult _result;
            private AsyncCallback _callback;
            public IAsyncResult Result
            {
                get
                {
                    return _result;
                }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("result");
                    }
                    _result = value;
                }
            }
            public AsyncCallback Callback
            {
                get { return _callback; }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("callback");
                    }
                    _callback = value;
                }
            }
            public bool IsProcessed { get; set; }
        }
        public void Attach(IAsyncResult ar, AsyncCallback callback)
        {
            queue.Add(new AsyncObject() { Result = ar, Callback = callback, IsProcessed = false });
        }
        public void ExecuteCallback()
        {
            int count = 0;
            while (count < queue.Count)
            {
                foreach (AsyncObject ao in queue)
                {
                    if (ao.Result.IsCompleted && !ao.IsProcessed)
                    {
                        try
                        {
                            ao.Callback(ao.Result);
                            ao.IsProcessed = true;
                        }
                        catch (Exception e)
                        {
                        }
                        finally
                        {
                            count++;
                        }
                    }
                }
            }
            queue.Clear();
        }
    }
}
