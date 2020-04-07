using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Radix_Sort
{
    class MyFileArray : DataArray
    {
        public MyFileArray(int n, string filename)
        {
            int[] data = new int[n];
            length = n;

            for (int i = 0; i < length; i++)
                data[i] = 0;

            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    for (int j = 0; j < length; j++)
                        writer.Write(data[j]);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public MyFileArray(string filename, int n)
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
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
                FileMode.Create)))
                {
                    for (int j = 0; j < length; j++)
                        writer.Write(plates[j]);
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
        public override int this[int index]
        {
            get
            {
                Byte[] data = new Byte[8];
                fs.Seek(4 * index, SeekOrigin.Begin);
                fs.Read(data, 0, 4);
                int result = BitConverter.ToInt32(data, 0);
                return result;
            }
            set
            {
                Byte[] data = new byte[4];
                BitConverter.GetBytes(value).CopyTo(data, 0);
                BitConverter.ToInt32(data, 0);
                fs.Seek(4 * index, SeekOrigin.Begin);
                fs.Write(data, 0, 4);
            }
        }
        public override void Swap(int j, double a, double b)
        {
            Byte[] data = new Byte[16];
            BitConverter.GetBytes(b).CopyTo(data, 0);
            BitConverter.GetBytes(a).CopyTo(data, 8);
            fs.Seek(8 * (j - 1), SeekOrigin.Begin);
            fs.Write(data, 0, 16);
        }
    }
}
