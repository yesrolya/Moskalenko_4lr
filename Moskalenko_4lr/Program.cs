using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moskalenko_4lr
{
    class Mask
    {
        int[] numbers;
        int length;

        public Mask(int[] nums)
        {
            length = nums.Length;
            numbers = new int[nums.Length];
            Array.Copy(nums, numbers, nums.Length);
        }

        public Mask(int i, int[,] nums)
        {
            length = nums.GetLength(1);
            numbers = new int[length];
            for (int j = 0; j < length; j++)
            {
                numbers[j] = nums[i, j];
            }
        }

        //возвращает количество совпадающих элементов
        public int Matched (Mask m) 
        {
            int r = 0;
            for (int i = 0; i < m.length; i++)
            {
                if (this.numbers[i] == m.numbers[i])
                {
                    r++;
                }
            }
            return r;
        }

        public int Matched(int i, int[,] nums)
        {
            int r = 0;
            for (int j = 0; j < length; j++)
            {
                if (numbers[j] == nums[i, j])
                    r++;
            }
            return r;
        }

        public int Mismatched(Mask m)
        {
            int r = 0;
            for (int i = 0; i < m.length; i++)
            {
                if (this.numbers[i] != m.numbers[i])
                {
                    r++;
                }
            }
            return r;
        }

        public void Print()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (i == 0) Console.Write("[");
                Console.Write(numbers[i]);
                if (i < numbers.Length - 1) Console.Write(", ");
                else Console.WriteLine("] ");
            }
        }

    }
    //кластеризация методом масок

    class Matrix
    {
        int rows;
        int columns;
        int R;
        int[,] numbers;
        List<Mask> masks;
        string[] codes;
        List<string> uniqueCodes;

        public Matrix (int rows, int columns, int[,] nums)
        {
            this.rows = rows;
            this.columns = columns;
            this.R = (columns / 2) + ( columns / 4);
            this.numbers = new int[rows, columns];
            
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    numbers[i, j] = nums[i, j];

            PrintSimilarity();
            CreateMaskMatrix();
            Encode();
            CreateClusters();
            //CompareMatrixWithMask();
        }

        public void PrintSimilarity ()
        {
            Console.WriteLine("Коэффициент сходства: " + R);
        }

        private void CreateMaskMatrix ()
        {
            //составление матрицы масок
            masks = new List<Mask>();

            //первая строка всегда становится первой маской
            masks.Add(new Mask(0, numbers));
            
            for (int i = 1; i < rows; i++)
            {
                var temp = new Mask(i, numbers);
                int k = 0;
                while (k < masks.Count() && masks[k].Mismatched(temp) >= R)
                    k++;
                if (k >= masks.Count())
                    masks.Add(temp);
            }

            Console.WriteLine("Количество масок: " + masks.Count());
            foreach (Mask m in masks)
                m.Print();
        }

        private void Encode ()
        {
            //закодировать каждую строку исходной матрицы
            codes = new string[numbers.GetLength(0)];
            uniqueCodes = new List<string>();

            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                codes[i] = "";
                foreach(Mask m in masks)
                {
                    codes[i] += (m.Matched(i, numbers) >= R) ? "1" : "0";
                }
                if (uniqueCodes.IndexOf(codes[i]) == -1) uniqueCodes.Add(codes[i]);
            }


            Console.WriteLine("Таблица кодировки строк исходной матрицы: ");
            for (int i = 0; i < codes.Length; i++)
                Console.WriteLine(i + ": " + codes[i]);
        }

        void CreateClusters()
        {
            int clNum = 0;
            foreach (var uc in uniqueCodes)
            {
                clNum++;
                Console.Write("Кластер №" + clNum + "    ");
                for (int i = 0; i < codes.Length; i++ )
                {
                    if (uc == codes[i]) Console.Write(i + " ");
                }
                Console.WriteLine();
            }
        }
        

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите количество строк в матрице: ");
            int rows = Int32.Parse(Console.ReadLine());
            Console.Write("Введите символов в строке: ");
            int columns = Int32.Parse(Console.ReadLine());
            Console.Write("Введите матрицу: ");
            int[,] numbers = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                var temp = Console.ReadLine();
                var t = temp.Split(' ');
                for (int j = 0; j < columns; j++)
                    numbers[i,j] = Int32.Parse(t[j]);
            }

            Matrix m = new Matrix(rows, columns, numbers);
            //int[,] number = { { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            //                  { 0, 1, 1, 0, 0, 0, 0, 0, 1 },
            //                  { 1, 1, 0, 0, 1, 1, 0, 0, 0 },
            //                  { 1, 1, 0, 0, 0, 0, 1, 1, 0 },
            //                  { 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            //                  { 1, 0, 0, 1, 1, 0, 0, 0, 0 }};
            //Matrix m = new Matrix(6, 9, number);
            Console.ReadKey();
        }
    }
}
