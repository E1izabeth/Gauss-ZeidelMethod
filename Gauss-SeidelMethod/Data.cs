using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss_SeidelMethod
{
    public class Data
    {
        public int Size { get; private set; }
        public double Precision { get; private set; }
        public double[,] ExtMatrix { get; private set; }
        public double[] Delta { get; private set; }
        public double[] Solve { get; private set; }
        public int IterationsCount { get; private set; }

        public Data(int size, double precision, double[,] matrix)
        {
            this.Size = size;
            this.Precision = precision;
            this.Delta = new double[size];
            this.Solve = new double[size];
            this.ExtMatrix = matrix;
            this.IterationsCount = 0;
        }

        public bool IsMatrixOfDiagonalView()
        {
            for (int i = 0; i < this.Size; i++)
            {
                double sum = 0;

                for (int j = 0; j < this.Size; j++)
                    if (i != j) sum += Math.Abs(this.ExtMatrix[i, j]);

                if (sum >= Math.Abs(this.ExtMatrix[i, i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsPossibleToDiagonal(out int[] indexes)
        {
            indexes = new int[this.Size];
            for (int i = 0; i < this.Size; i++)
            {
                indexes[i] = -1;
            }
            for (int i = 0; i < this.Size; i++)
            {
                double sum = 0, max = -1; int maxind = -1;
                for (int j = 0; j < this.Size; j++)
                {
                    sum += Math.Abs(this.ExtMatrix[i, j]);
                    if (Math.Abs(this.ExtMatrix[i, j]) > max)
                    {
                        max = Math.Abs(this.ExtMatrix[i, j]);
                        maxind = j;
                    }
                }
                sum -= Math.Abs(this.ExtMatrix[i, maxind]);
                if (indexes[maxind] != -1)
                {
                    indexes[maxind] = -1;
                    return false;
                }
                indexes[maxind] = i;
                
                if (sum >= max) return false;
            }

            for (int k = 0; k < this.Size; k++)
            {
                if (indexes[k] == -1)
                    return false;
            }
            return true;
        }

        public void MainDiagonalStabilize(int[] indexes)
        {
            double[,] NonDiagMatrix = new double[this.Size, this.Size + 1];
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size + 1; j++)
                {
                    NonDiagMatrix[i, j] = this.ExtMatrix[i, j];
                }
            }

            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size + 1; j++)
                {
                    this.ExtMatrix[i, j] = NonDiagMatrix[indexes[i], j];
                }
            }
        }

        public void Seidel()
        {
            var beta = new double[this.Size];
            var oldXVector = new double[this.Size];

            for (var i = 0; i < beta.Length; i++)
            {
                this.Solve[i] = beta[i] = (double)(this.ExtMatrix[i, this.Size] / this.ExtMatrix[i, i]);
                oldXVector[i] = Math.Abs(this.Solve[i]) + this.Precision * 2;
            }

            var alpha = new double[this.Size, this.Size];

            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    alpha[i, j] = i == j ? 0 : (double)(-this.ExtMatrix[i, j] / this.ExtMatrix[i, i]);
                }
            }
            
            while (Enumerable.Range(0, this.Size).Select(i => Math.Abs(this.Solve[i] - oldXVector[i])).Max() >= this.Precision && this.IterationsCount < 10000000)
            {
                this.Solve.CopyTo(oldXVector, 0);
                this.IterationsCount++;

                for (var i = 0; i < this.Size; i++)
                {
                    this.Solve[i] = beta[i]
                                 + Enumerable.Range(i, this.Size - i)
                                     .Select(j => oldXVector[j] * alpha[i, j])
                                     .Sum()
                                 + Enumerable.Range(0, i)
                                         .Select(j => this.Solve[j] * alpha[i, j])
                                         .Sum();
                                     
                }
            }

            for (var i = 0; i < this.Size; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < this.Size; j++)
                {
                    sum += this.ExtMatrix[i, j] * this.Solve[j];
                }

                this.Delta[i] = Math.Abs(sum - this.ExtMatrix[i, this.Size]);
            }
        }
    }
}
