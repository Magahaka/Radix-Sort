using System;
using System.Collections.Generic;
using System.Text;

namespace RadixSort
{
    class PlatesContainer
    {
        private NumberPlate[] Plates;
        public int Count { get; private set; }
        public int Size { get; private set; }

        public PlatesContainer(int size)
        {
            Plates = new NumberPlate[size];
            Count = 0;
            Size = size;
        }

        public PlatesContainer()
        {

        }

        public void AddPlate(NumberPlate numberPlate)
        {
            Plates[Count++] = numberPlate; 
        }

        public NumberPlate Take(int index)
        {
            return Plates[index];
        }

        public void Set(NumberPlate plate, int index)
        {
            if (index < Plates.Length)
            {
                Plates[index] = plate;
            }
        }
    }
}
