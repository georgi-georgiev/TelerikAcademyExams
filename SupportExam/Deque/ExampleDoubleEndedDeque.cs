using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deque
{
    class ExampleDoubleEndedDeque
    {
        static void Main()
        {
            IDeque<int> dequeExample = new Deque<int>();
            dequeExample.PushFirst(5);
            dequeExample.PushFirst(4);
            dequeExample.PushLast(6);
            dequeExample.PushLast(7);
            Console.WriteLine("Get last: {0}", dequeExample.PeekLast());

            dequeExample.PopFirst();
            dequeExample.PopLast();
            Console.WriteLine("Get first: {0}", dequeExample.PeekFirst());

            dequeExample.Clear();
            Console.WriteLine("Count: {0}", dequeExample.Count);
        }
    }
}
