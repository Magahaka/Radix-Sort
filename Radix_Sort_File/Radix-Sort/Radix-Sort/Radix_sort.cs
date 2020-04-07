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
            //Test_File_Array_List(seed);
            Test_Array_List_File();
        }

        public static void Test_Array_List_File()
        {
            int[] amounts = { 100, 200, 400, 800, 1600, 3200, 6400 };
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
                Console.WriteLine(string.Format("{0, -10} {1, -10}", n, watch.Elapsed.TotalMilliseconds));
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
                Radix_Sort(myfilelist, n);
                watch.Stop();
                Console.WriteLine(string.Format("{0, -10} {1, -10}", n, watch.Elapsed.TotalMilliseconds));
            }
        }


        //public static void Radix_Sort(DataList items)
        //{
        //    for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
        //    {
        //        CountingSort(items, exp);
        //    }
        //}

        public static void Radix_Sort(DataArray items)
        {
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                Counting_Sort(items, exp);
            }
        }

        public static void Counting_Sort(DataArray items, int exp)
        {
            string resultsFile = "results.txt";
            string countFile = "count.txt";
            int n = items.Length;

            MyFileArray results = new MyFileArray(n, resultsFile);
            MyFileArray count = new MyFileArray(10, countFile);

            using (count.fs = new FileStream(countFile, FileMode.Open, FileAccess.ReadWrite))
            {
                using (results.fs = new FileStream(resultsFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    int value;
                    for (int i = 0; i < n; i++)
                    {
                        value = items[i] / exp % 10;
                        count[value] = count[value] + 1;
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        count[i] = count[i] + count[i - 1];
                    }

                    for (int i = n-1; i >= 0 ; i--)
                    {
                        value = items[i] / exp % 10;
                        results[count[value] - 1] = items[i];
                        count[value] = count[value] - 1;
                    }

                    for (int i = 0; i < n; i++)
                    {
                        items[i] = results[i];
                    }
                }
            }
        }

        public static void Radix_Sort(DataList items, int seed)
        {
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                Counting_Sort(items, exp, seed);
            }
        }

        public static void Counting_Sort(DataList items, int exp, int seed)
        {
            string resultsFile = "results.dat";
            string countFile = "count.dat";
            int n = items.Length;

            MyFileList results = new MyFileList(n, resultsFile);
            MyFileList count = new MyFileList(n, countFile);

            using (count.fs = new FileStream(countFile, FileMode.Open, FileAccess.ReadWrite))
            {
                using (results.fs = new FileStream(resultsFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    int temp;
                    var current = items.Head();
                    while (current != 0)
                    {
                        current = current / exp % 10;
                        count.Set(current, count.Value(current) + 1);
                        current = items.Next();
                    }

                    for (int i = 1; i < 10; i++)
                    {
                        count.Set(i, count.Value(i) + count.Value(i - 1));
                    }

                    for (int i = n - 1; i >= 0; i--)
                    {
                        temp = items.Value(i) / exp % 10;
                        results.Set(count.Value(temp) - 1, items.Value(i));
                        count.Set(temp, count.Value(temp) - 1);
                    }
                    items.OverWrite(results);
                }
            }
        }

        public static void Test_File_Array_List(int seed)
        {
            int n = 10;
            string filename;
            filename = @"mydataarraytest.txt";
            //filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE ARRAY \n");
                myfilearray.Print(n);
                Radix_Sort(myfilearray);
                myfilearray.Print(n);
            }
            filename = @"mydatalisttest.txt";
            MyFileList myfilelist = new MyFileList(filename, n);
            using (myfilelist.fs = new FileStream(filename, FileMode.Open,
            FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE LIST \n");
                myfilelist.Print(n);
                Radix_Sort(myfilelist, seed);
                myfilelist.Print(n);
            }
        }
    }
    abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract int this[int index] { get; set; }
        public abstract void Swap(int j, double a, double b);
        public void Print(int n)
        {
            for (int i = 0; i < n; i++)
            {
                string plate = ConvertToString(this[i]);
                Console.Write(" {0:F5} ", plate);
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

    abstract class DataList
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract int Head();
        public abstract int Next();
        public abstract void Swap(double a, double b);
        public abstract void OverWrite(DataList items);
        public abstract void Set(int index, int value);
        public abstract int Value(int index);
        public void Print(int n)
        {
            Console.Write(" {0:F5} ", ConvertToString(Head()));
            for (int i = 1; i < n; i++)
            {
                string plate = ConvertToString(Next());
                Console.Write(" {0:F5} ", plate);
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