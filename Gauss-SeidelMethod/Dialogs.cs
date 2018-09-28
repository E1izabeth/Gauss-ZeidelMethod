using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gauss_SeidelMethod.App
{
    internal class Dialogs
    {
        internal bool ReadFromFile(FileInfo file, ref int size, ref double percision, ref double[,] matrix, int maxsize)
        {

            using (var reader = new StreamReader(file.OpenRead()))
            {
                var sizeFromFile = reader.ReadLine();
                if (!(ValidNumber(sizeFromFile) && Int32.Parse(sizeFromFile) <= maxsize)) { Console.WriteLine("Ivalid data!"); return false; }
                size = Int32.Parse(sizeFromFile);

                var precisionFromFile = reader.ReadLine();
                if (!ValidDouble(precisionFromFile)) { Console.WriteLine("Ivalid data!"); return false; }
                percision = Double.Parse(precisionFromFile);

                matrix = new double[size, size + 1];
                for (int i = 0; i < size; i++)
                {
                    string[] items;
                    try
                    {
                        items = reader.ReadLine().Split('\t', ' ');
                    }
                    catch (OutOfMemoryException)
                    {
                        Console.WriteLine("Ivalid data!");
                        return false;
                    }

                    if (items.Length < size + 1) { Console.WriteLine("Ivalid data!"); return false; }
                    for (int j = 0; j < size + 1; j++)
                    {
                        if (!ValidDouble(items[j])) { Console.WriteLine("Ivalid data!"); return false; }
                        matrix[i, j] = Double.Parse(items[j]);
                    }
                }


                if (reader.ReadToEnd().Length > 0) { Console.WriteLine("Ivalid data!"); return false; }
            }
            return true;
        }

        internal void InvalidPath()
        {
            Console.WriteLine("Invalid path!");
        }

        internal void CantMakeDiagonalStabilize()
        {
            Console.WriteLine("Can't make the diagonal stabilize!\n");
        }

        internal void WriteTypeOfMatrix(bool isDiagonal)
        {
            Console.Write($"Is matrix of diagonal view?\t");
            Console.WriteLine(isDiagonal);
        }

        internal FileInfo AskFile()
        {
            FileInfo file = null;
            Console.WriteLine("Please, input path to file");
            string fname = Console.ReadLine();
            if (File.Exists(fname)) file = new FileInfo(fname);
            return file;
        }

        public int ReadN(ref int succ, int maxsize)
        {
            Console.WriteLine("Please, input the number of variables");
            var n = Console.ReadLine();
            if (ValidNumber(n) && Int32.Parse(n) <= maxsize)
            {
                succ = 1;
                return Int32.Parse(n);
            }
            succ = this.AskTryAgain($"Invalid number. It should be in [1, {maxsize}]. Try again? (y/n)");
            return -1;
        }

        public double ReadPrecision(ref int succ)
        {
            Console.WriteLine("Please, input precision");
            return this.ReadDouble(ref succ);
        }

        private double ReadDouble(ref int succ)
        {
            var d = Console.ReadLine();
            if (ValidDouble(d))
            {
                succ = 1;
                return Double.Parse(d);
            }
            succ = this.AskTryAgain("Invalid double. Try again? (y/n)");
            return -1;
        }

        public double[,] ReadMatrix(ref int succ, int n)
        {
            Console.WriteLine("Please, input matrix items");
            double[,] matrix = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    do
                    {
                        matrix[i,j] = this.ReadMatrixItem(ref succ, i, j);
                    } while (succ == 2);
                    if (succ == 0) return null;
                }
            }
            return matrix;
        }
        
        internal void PrintInputData(int size, double precision, double[,] matrix)
        {
            Console.WriteLine("\n\n Input data:");
            Console.WriteLine($"\tnumber of variables: {size}");
            Console.WriteLine($"\tprecision: {precision}");
            Console.WriteLine("\tmatrix:");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size + 1; j++)
                {
                    Console.WriteLine($"\t[{i}, {j}]  {matrix[i, j]}");
                }
                Console.WriteLine();
            }
        }

        public void PrintMatrix(int size, double[,] matrix)
        {
            Console.WriteLine("\tmatrix:");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size + 1; j++)
                {
                    Console.WriteLine($"\t[{i}, {j}]  {matrix[i, j]}");
                }
                Console.WriteLine();
            }
        }

        public void PrintSolve(int size, double[] solve, double[] delts, int iterationsCount)
        {
            Console.WriteLine("\tsolve:");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"\tX{i} = {solve[i]}  +- {delts[i]}");
            }
            Console.WriteLine($"Count of iterations: {iterationsCount}");
        }

        public double ReadMatrixItem(ref int succ, int i, int j)
        {
            Console.WriteLine($"[{i}; {j}]");
            var m = Console.ReadLine();
            if (ValidDouble(m))
            {
                succ = 1;
                return Double.Parse(m);
            }
            succ = this.AskTryAgain("Invalid item. It shoul be double. Try again? (y/n)");
            return -1;
        }

        private int AskTryAgain(string massage)
        {
            Console.WriteLine(massage);
            var s = Console.ReadLine();
            if (s == "y")
                return 2;
            else return 0;
        }

        public static bool ValidDouble(string number)
        {
            Regex regex = new Regex("^[+-]?[0-9]*[,]?[0-9]+$");
            if (!regex.IsMatch(number))
            {
                return false;
            }
            return true;
        }

        public static bool ValidNumber(string number)
        {
            Regex regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(number))
            {
                return false;
            }
            return true;
        }
    }
}
