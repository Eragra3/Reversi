using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Logic
{
    public class Node<T>
    {
        public Node<T>[] Children;

        public T Value;

        public Node(T value)
        {
            Value = value;
        }
    }

}
