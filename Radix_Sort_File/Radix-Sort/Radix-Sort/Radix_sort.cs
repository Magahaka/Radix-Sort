using System;
using System.IO;
using System.Text;

namespace Radix_Sort
{
    class Radix_sort
    {
        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            // Parodo nesurusiuotus ir surusiuotus duomenys array ir list faile
            Test_File_Array_List(seed);
            Test_Array_List_File();
        }

        public static void Test_Array_List_File()
        {
            int[] amounts = { 100, 200, 300 };
            Console.WriteLine("\n----------------------------Sorting Tests in External Memory-----------------------------");
            Console.WriteLine("---------------------------------FILE ARRAY RADIX SORT---------------------------------");
            Console.WriteLine(string.Format("{0, -10} {1, -10}", "Amount", "Time (ms)"));
            foreach (int amount in amounts)
            {
                Test_Array_Radix_Sort_File(amount);
            }

            Console.WriteLine("---------------------------------FILE LIST RADIX SORT----------------------------------");
            Console.WriteLine(string.Format("{0, -10} {1, -10}", "Amount", "Time (ms)"));
            foreach (int amount in amounts)
            {
                Test_List_Radix_Sort_File(amount);
            }
        }

        public static void Test_Array_Radix_Sort_File(int n)
        {
            string filename;
            filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Radix_Sort(myfilearray);
                watch.Stop();
                Console.WriteLine(string.Format("{0, -10} {1, -10}", n, watch.Elapsed));
            }
        }

        public static void Test_List_Radix_Sort_File(int n)
        {
            string filename;
            filename = @"mydatalist.dat";
            MyFileList myfilelist = new MyFileList(filename, n);
            using (myfilelist.fs = new FileStream(filename, FileMode.Open,
            FileAccess.ReadWrite))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Radix_Sort(myfilelist);
                watch.Stop();
                Console.WriteLine(string.Format("{0, -10} {1, -10}", n, watch.Elapsed));
            }
        }

        public static void CountingSort(DataList items, int exp)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            double prevdata, currentdata;
            double first, second;
            Byte[] forChange1 = new byte[12];
            Byte[] forChange2 = new byte[12];
            string firstPlate = "";
            string secondPlate = "";
            string thirdPlate = "";
            NumberPlate plate1, plate2, plate3;
            string[] partsFirst;
            string[] partsSecond;
            for (int i = items.Length - 1; i >= 0; i--)
            {
                BitConverter.GetBytes(items.Head()).CopyTo(forChange1, 0);
                firstPlate = encoder.GetString(forChange1, 1, 7);
                partsFirst = firstPlate.Split(' ');
                plate1 = new NumberPlate(partsFirst[0], partsFirst[1]);

                currentdata = plate1.GetPlateCode();
                for (int j = 1; j <= i; j++)
                {
                    prevdata = currentdata;
                    plate3 = new NumberPlate();
                    plate3.BackToPlate(prevdata);
                    thirdPlate = "\a" + plate3.Letters + " " + plate3.Number + "\0\0\0\0";
                    forChange1 = Encoding.UTF8.GetBytes(thirdPlate);
                    BitConverter.GetBytes(items.Next()).CopyTo(forChange2, 0);
                    secondPlate = encoder.GetString(forChange2, 1, 7);
                    partsSecond = secondPlate.Split(' ');
                    plate2 = new NumberPlate(partsSecond[0], partsSecond[1]);

                    currentdata = plate2.GetPlateCode();

                    first = prevdata / exp % 10;
                    second = currentdata / exp % 10;
                    if (first > second)
                    {
                        items.Swap(BitConverter.ToDouble(forChange2), BitConverter.ToDouble(forChange1));
                        currentdata = prevdata;
                    }
                }
            }
        }

        public static void CountingSort(DataArray items, int exp)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] forChange = new byte[16];
            double first, second;
            int i, j;
            NumberPlate plate1;
            NumberPlate plate2;
            for (int z = 0; z < items.Length; z++)
            {
                i = 0;
                j = 1;
                while (j < items.Length)
                {
                    BitConverter.GetBytes(items[i]).CopyTo(forChange, 0);
                    BitConverter.GetBytes(items[j]).CopyTo(forChange, 8);
                    string firstPlate = encoder.GetString(forChange, 1, 7);
                    string[] partsFirst = firstPlate.Split(' ');
                    plate1 = new NumberPlate(partsFirst[0], partsFirst[1]);

                    string secondPlate = encoder.GetString(forChange, 9, 7);
                    string[] partsSecond = secondPlate.Split(' ');
                    plate2 = new NumberPlate(partsSecond[0], partsSecond[1]);

                    first = plate1.GetPlateCode() / exp % 10;
                    second = plate2.GetPlateCode() / exp % 10;
                    if (first > second)
                    {
                        items.Swap(j, BitConverter.ToDouble(forChange, 0), BitConverter.ToDouble(forChange, 8));
                    }
                    i++;
                    j++;
                }
            }
        }

        public static void Radix_Sort(DataArray items)
        {
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                CountingSort(items, exp);
            }
        }

        public static void Radix_Sort(DataList items)
        {
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                CountingSort(items, exp);
            }
        }

        public static void Test_File_Array_List(int seed)
        {
            int n = 5;
            string filename;
            filename = @"mydataarray.txt";
            //filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE ARRAY \n");
                myfilearray.Print(n);
                Radix_Sort(myfilearray);
                myfilearray.Print(n);
            }
            filename = @"mydatalist.dat";
            MyFileList myfilelist = new MyFileList(filename, n);
            using (myfilelist.fs = new FileStream(filename, FileMode.Open,
            FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE LIST \n");
                myfilelist.Print(n);
                Radix_Sort(myfilelist);
                myfilelist.Print(n);
            }
        }
    }
    abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract double this[int index] { get; }
        public abstract Byte[] TakeFromFile(int index);
        public abstract void Swap(int j, double a, double b);
        public abstract void Set(int index);
        public void Print(int n)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] data = new byte[8];
            for (int i = 0; i < n; i++)
            {
                BitConverter.GetBytes(this[i]).CopyTo(data, 0);
                Console.Write(" {0:F5} ", encoder.GetString(data));
            }
            Console.WriteLine();
        }
    }

    abstract class DataList
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract double Head();
        public abstract double Next();
        public abstract void Swap(double a, double b);
        public void Print(int n)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] data = new byte[12];
            BitConverter.GetBytes(Head()).CopyTo(data, 0);
            Console.Write(" {0:F5} ", encoder.GetString(data));
            for (int i = 0; i < n; i++)
            {
                BitConverter.GetBytes(Next()).CopyTo(data, 0);
                Console.Write(" {0:F5} ", encoder.GetString(data));
            }
            Console.WriteLine();
        }
    }
}