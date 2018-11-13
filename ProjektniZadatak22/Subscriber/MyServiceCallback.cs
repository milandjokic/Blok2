using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class MyServiceCallback : IMyServiceCallBack
    {
        public void OnCallBack()
        {
            Console.WriteLine("CALLBACK AAA");
        }
    }
}
