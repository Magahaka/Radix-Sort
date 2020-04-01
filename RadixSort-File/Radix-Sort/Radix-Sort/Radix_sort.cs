using System;
using System.IO;

namespace Radix_Sort
{
    class Radix_sort
    {
        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            // Antras etapas
            Test_File_Array_List(seed);
        }

        public static void Sort(DataArray items, int exp)
        {
            int prevdata, currentdata;
            for (int i = items.Length - 1; i >= 0; i--)
            {
                currentdata = items[0];
                for (int j = 1; j <= i; j++)
                {
                    prevdata = currentdata;
                    currentdata = items[j];
                    if ((prevdata / exp % 10) > (currentdata / exp % 10))
                    {
                        items.Swap(j, currentdata, prevdata);
                        currentdata = prevdata;
                    }
                }
            }
        }

        public static void Radix_Sort(DataArray items)
        {
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                Sort(items, exp);
            }
        }

        public static void Test_File_Array_List(int seed)
        {
            int n = 20;
            string filename;
            filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n, seed);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open,
            FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE ARRAY \n");
                myfilearray.Print(n);
                Radix_Sort(myfilearray);
                myfilearray.Print(n);
            }
        }
    }
    abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract int this[int index] { get; }
        public abstract void Swap(int j, int a, int b);
        public void Print(int n)
        {
            for (int i = 0; i < n; i++)
            {
                string plate = ConvertToString(this[i]);
                Console.Write(" {0} ", plate);
            }
            Console.WriteLine();
        }

        public string ConvertToString(int number)
        {
            string plateString = number.ToString();
            string firstLetter = plateString.Substring(0, 2);
            string secondLetter = plateString.Substring(2, 2);
            string thirdLetter = plateString.Substring(4, 2);
            string numbers = plateString.Substring(6, 3);

            char one = (char)int.Parse(firstLetter);
            char two = (char)int.Parse(secondLetter);
            char three = (char)int.Parse(thirdLetter);

            string plateLetters = one.ToString() + two.ToString() + three.ToString();

            string result = plateLetters + " " + numbers;

            return result;
        }
    }
}
