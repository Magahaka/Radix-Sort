using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Radix_Sort
{
    class MyFileList : DataList
    {
        int prevNode;
        int currentNode;
        int nextNode;
        public MyFileList(string filename, int n)
        {
            int[] plates = new int[n];
            length = n;
            for (int i = 0; i < length; i++)
            {
                plates[i] = Generator();
            }
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                { 
                    writer.Write(4);
                    for (int j = 0; j < length; j++)
                    {
                        writer.Write(plates[j]);
                        writer.Write((j + 1) * 8 + 4);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public MyFileList(int n, string filename)
        {
            length = n;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(4);
                    for (int j = 0; j < length; j++)
                    {
                        writer.Write(0);
                        writer.Write((j + 1) * 8 + 4);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public int Generator()
        {
            string lettersPlate = null;
            string numberPlate = null;
            char random;
            int result;

            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                if (i <= 2)
                {
                    random = (char)rnd.Next('A', '[');
                    lettersPlate += random;
                }
                else
                {
                    random = (char)rnd.Next('0', ':');
                    numberPlate += random;
                }
            }

            result = ConvertToInt(lettersPlate, numberPlate);

            return result;
        }

        public int ConvertToInt(string plate, string number)
        {
            int numberPlate = 0;
            string result = null;
            int first = (int)char.Parse(plate.Substring(0, 1));
            int second = (int)char.Parse(plate.Substring(1, 1));
            int third = (int)char.Parse(plate.Substring(2, 1));

            result = first.ToString() + second.ToString() + third.ToString() + number.ToString();
            numberPlate = int.Parse(result);

            return numberPlate;
        }

        public FileStream fs { get; set; }
        public override int Head()
        {
            Byte[] data = new Byte[8];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(data, 0, 4);
            currentNode = BitConverter.ToInt32(data, 0);
            prevNode = -1;
            fs.Seek(currentNode, SeekOrigin.Begin);
            fs.Read(data, 0, 8);
            int result = BitConverter.ToInt32(data, 0);
            nextNode = BitConverter.ToInt32(data, 4);
            return result;
        }
        public override int Next()
        {
            Byte[] data = new Byte[8];
            fs.Seek(nextNode, SeekOrigin.Begin);
            fs.Read(data, 0, 8);
            prevNode = currentNode;
            currentNode = nextNode;
            int result = BitConverter.ToInt32(data, 0);
            nextNode = BitConverter.ToInt32(data, 4);
            return result;

        }
        public override void Swap(double a, double b)
        {
            Byte[] data;
            fs.Seek(prevNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(a);
            fs.Write(data, 0, 8);
            fs.Seek(currentNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(b);
            fs.Write(data, 0, 8);

        }
        public override void Set(int index, int value)
        {
            Byte[] data = new Byte[4];
            fs.Seek((8 * index) + 4, SeekOrigin.Begin);
            BitConverter.GetBytes(value).CopyTo(data, 0);
            fs.Write(data, 0, 4);
        }
        public override int Value(int index)
        {
            int value = Head();

            for (int i = 1; i < Length; i++)
            {
                int next = Next();
                if (i == index)
                {
                    value = next;
                    return value;
                }
            }
            return value;
        }

        public override void OverWrite(DataList items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Byte[] data = new Byte[4];
                fs.Seek(8 * i + 4, SeekOrigin.Begin);
                BitConverter.GetBytes(items.Value(i)).CopyTo(data, 0);
                fs.Write(data, 0, 4);
            }
        }
    }
}
