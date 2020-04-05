using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RadixSort
{
    class NumberPlate : IComparable<NumberPlate>, IEquatable<NumberPlate>
    {
        public string Letters { get; set; }
        public string Number { get; set; }
        public int Char { get; set; }

        public NumberPlate(string letters, string number)
        {
            Letters = letters;
            Number = number;
        }

        public NumberPlate()
        {

        }
        
        public string GetLettersCode(string result)
        {
            result = null;
            for (int i = 0; i < Letters.Length; i++)
            {
                Char = Letters[i];
                result += Char;
            }
            return result;
        } 

        public int GetPlateCode()
        {
            string plateString = null;
            int plate = 0;
            string codeString = null;
            codeString = GetLettersCode(codeString);
            plateString = codeString + Number;
            plate = int.Parse(plateString);
            
            return plate;
        }

        public void Generator()
        {
            string plate = null;
            char random;

            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                if (i < 2)
                {
                    random = (char)rnd.Next('A', '[');
                    plate += random;
                }
                else if (i == 2)
                {
                    random = (char)rnd.Next('A', '[');
                    plate += random + " ";
                }
                else
                {
                    random = (char)rnd.Next('0', ':');
                    plate += random;
                }
            }
            string[] values = plate.Split(" ");
            Letters = values[0];
            Number = values[1];               
        }

        public int CompareTo([AllowNull] NumberPlate other)
        {
            throw new NotImplementedException();
        }

        public bool Equals([AllowNull] NumberPlate other)
        {
            throw new NotImplementedException();
        }
    }
}
