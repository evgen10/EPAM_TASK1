using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{

    class MyEventArgs: EventArgs
    {
        public string extension;
        public bool flag;
    }

    class Test
    {
        event EventHandler<MyEventArgs> MyEvent;

        


    }
}
