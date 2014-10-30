using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace Concurrency.Rx.MockProcesses
{
    public interface IIndipendentProcessesMock
    {
        void ProcessOne(ConcurrentQueue<string> outPutQueue);
        void ProcessTwo(ConcurrentQueue<string> outPutQueue);
        void ProcessThree(ConcurrentQueue<string> outPutQueue);
    }

    public class IndipendentProcessesMock : IIndipendentProcessesMock
    {

        public void ProcessOne(ConcurrentQueue<string> outPutQueue)
        {
            Enumerable.Range(0, 100).ForEach(item => outPutQueue.Enqueue(string.Format("{0} Process One item number: {1}", item, DateTime.Now.ToString("HH:mm:ss.ffffff"))));
        }

        public void ProcessTwo(ConcurrentQueue<string> outPutQueue)
        {
            Enumerable.Range(0, 100).ForEach(item => outPutQueue.Enqueue(string.Format("{0} Process Two item number: {1}", item, DateTime.Now.ToString("HH:mm:ss.ffffff"))));
        }

        public void ProcessThree(ConcurrentQueue<string> outPutQueue)
        {
            Enumerable.Range(0, 100).ForEach(item => outPutQueue.Enqueue(string.Format("{0} Process Three item number: {1}", item, DateTime.Now.ToString("HH:mm:ss.ffffff"))));
        }
    }
}
