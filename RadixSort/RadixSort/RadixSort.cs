using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RadixSort
{
    class Radix_Sort
    {
        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;

            // Pirmas etapas
            Test_Array_List(seed);
        }

        public static List<NumberPlate> ReadData()
        {
            List<NumberPlate> plates = new List<NumberPlate>();
            string[] lines = File.ReadAllLines(@"Plates.csv");

            foreach (string line in lines)
            {
                string[] values = line.Split(" ");
                string plateLetters = values[0];
                string plateNumbers = values[1];

                NumberPlate data = new NumberPlate(plateLetters, plateNumbers);
                plates.Add(data);
            }
            return plates;
        }

        public static void Counting_sort(int[] A, int exp)
        {
            int[] C = new int[10];               // Skaiciu poziciju pasikartojimu (masyvas)
            int[] D = new int[A.Length];         // Surusiuotas masyvas

            // Skaiciu daznumas
            int number;
            for (int i = 0; i < A.Length; i++)
            {
                number = (A[i] / exp % 10);
                C[number] = C[number] + 1;
            }

            // Skaiciu kiekis "<=" nurodytam skaiciui
            for (int i = 1; i < 10; i++)
            {
                C[i] = C[i - 1] + C[i];
            }

            // Rusiavimo algoritmas
            int index;
            for (int i = A.Length-1; i >= 0; i--)
            {
                number = (A[i] / exp % 10);
                index = C[number];
                C[number] = index - 1;
                D[index-1] = A[i];
            }

            // Masyvo priskyrimas tolimesniam rusiavimui
            for (int i = 0; i < A.Length; i++)
            {
                A[i] = D[i];
            }
        }

        public static void Radix_sort(PlatesContainer items)
        {
            int[] arr = new int[items.Count];
            AddIntToArray(items, arr);
            for (int exp = 1; exp < Math.Pow(10,9); exp*=10)
            {
                Counting_sort(arr, exp);
            }

            string plates;
            NumberPlate plate;
            for (int i = 0; i < items.Size; i++)
            {
                plate = new NumberPlate();
                plates = BackToPlate(plate, arr[i]);
                items.Set(plate, i);
            }         
        }

        public static void Print(PlatesContainer plates)
        {
            for (int i = 0; i < plates.Size; i++)
            {
                Console.WriteLine(plates.Take(i).Letters + " " + plates.Take(i).Number);
            }
        }

        public static string BackToPlate(NumberPlate plate, int number)
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
            string plateNumber = numbers;

            plate.Letters = plateLetters;
            plate.Number = plateNumber;

            string result = plateLetters + " " + numbers;

            return result;
        }

        public static void AddIntToArray(PlatesContainer items, int[] array)
        {
            NumberPlate plate;
            for (int i = 0; i < items.Size; i++)
            {
                plate = items.Take(i);
                array[i] = plate.GetPlateCode();
            }
        }

        public static PlatesContainer DoContainer(PlatesContainer plates)
        {
            NumberPlate plate;
            for (int i = 0; i < plates.Size; i++)
            {
                plate = new NumberPlate();
                plate.Generator();
                plates.AddPlate(plate);
            }
            return plates;
        }

        public static void Test_Array_List(int seed)
        {
            int n = 500;

            //Console.WriteLine("\n ARRAY \n");
            //PlatesContainer container = new PlatesContainer(n);
            //container = DoContainer(container);
            //Print(container);
            //Console.WriteLine("");
            //Radix_sort(container);
            //Print(container);

            LinkedList<NumberPlate> plateList = new LinkedList<NumberPlate>(); 
            plateList = DoList(plateList, n);
            Radix_sort(plateList, n);
        }

        public static LinkedList<NumberPlate> DoList(LinkedList<NumberPlate> plateList, int n)
        {
            NumberPlate plate;
            for (int i = 0; i < n; i++)
            {
                plate = new NumberPlate();
                plate.Generator();
                plateList.InsertNodeAtEnd(plate);
            }
            return plateList;
        }

        public static void AddIntToLinkedList(LinkedList<NumberPlate> plateList, LinkedList<int> intList)
        {
            NumberPlate number;
            int plateCode;

            var currentNode = plateList.Head;
            while ((currentNode != null) && (currentNode.Data != null))
            {
                number = currentNode.Data;
                plateCode = number.GetPlateCode();
                intList.InsertNodeAtEnd(plateCode);
                currentNode = currentNode.Next;
            }
        }

        public static void Counting_sort(LinkedList<int> list, int exp)
        {
            LinkedList<int> D = new LinkedList<int>();
            LinkedList<int>[] lists = new LinkedList<int>[10];
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = new LinkedList<int>();
            }

            // Skaiciu daznumas
            int number;
            var currentNode = list.Head;
            while ((currentNode != null) && (currentNode.Data != 0))
            {
                number = (currentNode.Data / exp % 10);
                lists[number].InsertNodeAtEnd(currentNode.Data);
                list.DeleteFirstNode();
                currentNode = currentNode.Next;
            }

            // Sudeda isrikiuotas reiksmes atgal i originalu list'a
            for (int i = 0; i < lists.Length; i++)
            {
                currentNode = lists[i].Head;
                while ((currentNode != null) && (currentNode.Data != 0))
                {
                    list.InsertNodeAtEnd(currentNode.Data);
                    currentNode = currentNode.Next;
                }
            }
        }

        public static void Radix_sort(LinkedList<NumberPlate> list, int n)
        {
            LinkedList<int> intList = new LinkedList<int>();
            LinkedList<string> sortedList = new LinkedList<string>();
            AddIntToLinkedList(list, intList);
            for (int exp = 1; exp < Math.Pow(10, 9); exp *= 10)
            {
                Counting_sort(intList, exp);
            }

            string plates;
            NumberPlate plate;

            var currentNode = intList.Head;
            for (int i = 0; i < n; i++)
            {
                plate = new NumberPlate();
                plates = BackToPlate(plate, currentNode.Data);
                sortedList.InsertNodeAtEnd(plates);
                currentNode = currentNode.Next;
            }

            Print(sortedList);
        }

        public static void Print(LinkedList<string> sortedList)
        {
            var currentNode = sortedList.Head;
            while (currentNode != null)
            {
                Console.WriteLine("{0}", currentNode.Data);
                currentNode = currentNode.Next;
            }
        }
    }

    abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract double this[int index] { get; set; }
        public abstract void Swap(int j, double a, double b);
        public void Print(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write(" {0:F5} ", this[i]);
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
            Console.Write(" {0:F5} ", Head());
            for (int i = 0; i < n; i++)
            {
                Console.Write(" {0:F5} ", Next());
            }
            Console.WriteLine();
        }
    }
}
