using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace RadixSort
{
    public class LinkedList<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>
    {
        public sealed class Node<T>
        {
            public Node<T> Right;
            public Node<T> Left;

            public T data;

            public Node(T value, Node<T> left, Node<T> right)
            {
                Left = left;
                Right = right;
                data = value;
            }
        }

        private Node<T> first;
        private Node<T> last;
        private Node<T> current;
        public int size;

        public LinkedList()
        {
            first = null;
            last = null;
            current = null;
            size = 0;
        }

        public void Add(T element)
        {
            var node = new Node<T>(element, last, null);

            if (first != null)
            {
                last.Right = node;
                last = node;
            }
            else
            {
                first = node;
                last = node;
            }
            current = node;
            size++;
        }

        public void Remove(Node<T> node)
        {
            if (node == first)
            {
                first = first.Right;
                
            }
            else if (node == last)
            {
                last = last.Left;
            }
            else if (node.Left != null)
            {
                node.Left.Right = node.Right;
            }
            else if (node.Right != null)
            {
                node.Right.Left = node.Left;
            }
            size--;
        }

        public void Beginning()
        {
            current = first;
        }

        public void End()
        {
            current = last;
        }

        public void Next()
        {
            current = current.Right;
        }

        public void Previous()
        {
            current = current.Left;
        }

        public bool Exists()
        {
            return current != null;
        }

        public T Take()
        {
            return current.data;
        }

        public Node<T> TakeNode()
        {
            return current;
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
