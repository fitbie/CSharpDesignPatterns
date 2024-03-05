// In this case i use .NET predefined IEnumerable. Original GOF iterator was implemented by classes & inheritance, 
// but 2024 and cyberpunk already here.

using System.Collections;

namespace GOF.Iterator
{
    public class IteratorGOF
    {
        public void Iterate()
        {
            ListNode root = new(1, new(2, new(3, new(4, new(5)))));
            foreach (var node in root)
            {
                Console.WriteLine(node.Val);
            }
        }
    }



    public class ListNode : IEnumerable<ListNode>, IEnumerable
    {
        public int Val { get; init; }
        public ListNode? Next { get; init; }

        public ListNode(int val, ListNode? next = null)
        {
            Val = val;
            Next = next;
        }

        public IEnumerator<ListNode> GetEnumerator()
        {
            ListNode current = this;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}