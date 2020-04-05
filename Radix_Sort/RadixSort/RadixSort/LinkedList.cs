using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Text;


namespace RadixSort
{
    public class LinkedList<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>
    {
        public class Node<T>
        {
            public T Data { get; }
            public Node<T> Next { get; set; }
            public Node(T data)
            {
                Data = data;
            }
        }

        public Node<T> Head { get; set; }

        public void InsertNodeAtEnd(T element)
        {
            Node<T> temp = Head;
            var node = new Node<T>(element);

            if (Head == null)
            {
                Head = node;
            }
            else
            {
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = node;
            }
        }

        public void InsertNodeAtFirst(Node<T> node)
        {
            node.Next = Head;
            Head = node;
        }

        public void InsertNodeAtAGivenPosition(Node<T> node, int indexPosition)
        {
            int counter = 0;
            Node<T> temp = Head;

            while (temp.Next != null)
            {
                if (indexPosition == counter)
                {
                    node.Next = temp.Next;
                    temp.Next = node;

                    return;
                }
                temp = temp.Next;
                counter++;
            }
        }

        public void DeleteFirstNode()
        {
            Head = Head.Next;
        }

        public void DeleteLastNode()
        {
            Node<T> temp = Head;
            Node<T> previousNode = Head;

            while (temp.Next != null)
            {
                previousNode = temp;
                temp = temp.Next;
            }
            previousNode.Next = null;
        }

        public void DeleteNodeFromMiddle(int nodeIndexPosition)
        {
            int counter = 0;
            Node<T> temp = Head;
            Node<T> previousNode = Head;

            while (temp.Next != null)
            {
                if (nodeIndexPosition == counter)
                {
                    previousNode.Next = temp.Next;
                    return;
                }
                previousNode = temp;
                temp = temp.Next;
                counter++;
            }
        }

        public void Traverse()
        {
            while (Head.Next != null)
            {
                Head = Head.Next;
                Debug.WriteLine(Head.Data);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
