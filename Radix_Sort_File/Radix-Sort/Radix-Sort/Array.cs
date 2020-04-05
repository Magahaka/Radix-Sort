using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Radix_Sort
{
    class MyFileArray : DataArray
    {
        public MyFileArray(string filename, int n)
        {
            NumberPlate[] Plates;
            int count = 0;
            length = n;
            Plates = new NumberPlate[n];
            NumberPlate plate;
            for (int i = 0; i < n; i++)
            {
                plate = new NumberPlate();
                plate.Generator();
                Plates[count++] = plate;
            }
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    string plateString = "";
                    for (int i = 0; i < count; i++)
                    {
                        plateString = Plates[i].Letters + " " + Plates[i].Number;
                        writer.Write(plateString);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream fs { get; set; }
        public override double this[int index]
        {
            get
            {
                Byte[] data = new Byte[8];
                fs.Seek(8 * index, SeekOrigin.Begin);
                fs.Read(data, 0, 8);
                double result = BitConverter.ToDouble(data, 0);
                return result;
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

        public override Byte[] TakeFromFile(int index)
        {
            Byte[] data = new Byte[8];
            fs.Seek(8 * index, SeekOrigin.Begin);
            fs.Read(data, 0, 8);
            return data;
        }

        public override void Set(int index)
        {
            Byte[] data = new byte[8];
            fs.Seek(8 * index, SeekOrigin.Begin);
            fs.Write(data, 0, 8);
        }
    }
}
