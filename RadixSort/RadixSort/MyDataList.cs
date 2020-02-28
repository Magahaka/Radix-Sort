using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RadixSort
{
    class MyDataList : DataList
    {
        class MyLinkedListNode
        {
            public MyLinkedListNode nextNode { get; set; }
            public double data { get; set; }
            public MyLinkedListNode(double data)
            {
                this.data = data;
            }
        }
        MyLinkedListNode headNode;
        MyLinkedListNode prevNode;
        MyLinkedListNode currentNode;

        public MyDataList(int n, int seed)
        {
            length = n;
            Random rand = new Random(seed);
            headNode = new MyLinkedListNode(rand.NextDouble());
            currentNode = headNode;
            for (int i = 0; i < length; i++)
            {
                prevNode = currentNode;
                currentNode.nextNode = new MyLinkedListNode(rand.NextDouble());
                currentNode = currentNode.nextNode;
            }
            currentNode.nextNode = null;
        }

        public override double Head()
        {
            currentNode = headNode;
            prevNode = null;
            return currentNode.data;
        }

        public override double Next()
        {
            prevNode = currentNode;
            currentNode = currentNode.nextNode;
            return currentNode.data;
        }

        public override void Swap(double a, double b)
        {
            prevNode.data = a;
            currentNode.data = b;
        }
    }
}
