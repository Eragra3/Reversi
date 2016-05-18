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

        public void Print()
        {
            Console.WriteLine("Printing tree");

            var sb = new StringBuilder();

            foreach (var child in Children)
            {
                sb.AppendLine(child.ToString(1));
            }

            Console.WriteLine(sb.ToString());
        }

        public string ToString(int depth)
        {
            var sb = new StringBuilder();

            var tab = new string('\t', depth);

            if (Children == null) return $"{tab}{Value}";

            foreach (var child in Children)
            {
                sb.AppendLine(child.ToString(depth + 1));
            }

            return $"{tab}{Value}\n{sb}";
        }
    }

}
