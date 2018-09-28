using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gauss_SeidelMethod.App
{
    public class App
    {
        public int MaxSize { get; private set; }
        private readonly Dialogs _user = new Dialogs();
        private Data _data;

        public App(int maxsize)
        {
            this.MaxSize = maxsize;
        }

        static void Main(string[] args)
        {
            var app = new App(20);

            var menu = new Menu(
                    new MenuItem("Please, choose way to input data") 
                    {
                        new MenuItem("From file", app.GetFromFile),
                        new MenuItem("From keyboard", app.GetFromKeyboard),
                        new MenuItem("Random data", app.GetRandomData)
                    }
                 );

            menu.RunMenu();
        }

        private void GetFromFile()
        {
            FileInfo fname = _user.AskFile();
            try
            {
                if (fname.Exists)
                {
                    int size = 0; double percision = 0; double[,] matrix = null;
                    if (_user.ReadFromFile(fname, ref size, ref percision, ref matrix, this.MaxSize))
                    {
                        this._data = new Data(size, percision, matrix);
                        _user.PrintInputData(this._data.Size, this._data.Precision, this._data.ExtMatrix);
                    }
                    this.HandData();
                }
                else
                {
                    this._user.InvalidPath();
                }
            }
            catch(System.NullReferenceException)
            {
                this._user.InvalidPath();
                return;
            }
        }

        private void HandData()
        {
            bool isDiagonal = this._data.IsMatrixOfDiagonalView();
            _user.WriteTypeOfMatrix(isDiagonal);
            if (!isDiagonal)
            {
                if (!this._data.IsPossibleToDiagonal(out int[] indexes))
                {
                    _user.CantMakeDiagonalStabilize();
                    return;
                }
                else
                {
                    this._data.MainDiagonalStabilize(indexes);
                }
            }

            this._data.Seidel();
            _user.PrintMatrix(this._data.Size, this._data.ExtMatrix);
            _user.PrintSolve(this._data.Size, this._data.Solve, this._data.Delta, this._data.IterationsCount);

        }

        private void GetRandomData()
        {
            var rnd = new Random();
            var n = rnd.Next(1, 20);
            var precision = rnd.NextDouble();
            var matrix = this.GetRandomMatrix(rnd, n);
            this._data = new Data(n, precision, matrix);
            _user.PrintInputData(this._data.Size, this._data.Precision, this._data.ExtMatrix);
            this.HandData();
        }

        private double[,] GetRandomMatrix(Random rnd, int n)
        {
            double[,] matrix = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    matrix[i, j] = rnd.NextDouble() * rnd.Next(0, 10);
                }
            }
            return matrix;
        }

        private void GetFromKeyboard()
        {
            var succ = 0;
            var n = this.GetN(ref succ);
            if (succ == 0) return;
            var precision = this.GetPrecision(ref succ);
            if (succ == 0) return;
            var matrix = this.GetMatrix(ref succ, n);
            if (succ == 0) return;
            this._data = new Data(n, precision, matrix);
            _user.PrintInputData(this._data.Size, this._data.Precision, this._data.ExtMatrix);
            this.HandData();
        }
        

        private int GetN(ref int succ)
        {
            var n = _user.ReadN(ref succ, this.MaxSize);
            while (succ == 2)
            {
                n = _user.ReadN(ref succ, this.MaxSize);
            } 
            return n;
        }

        private double GetPrecision(ref int succ)
        {
            var p = _user.ReadPrecision(ref succ);
            while (succ == 2)
            {
                p = _user.ReadPrecision(ref succ);
            };
            return p;
        }

        private double[, ] GetMatrix(ref int succ, int n)
        {
            var matirx = _user.ReadMatrix(ref succ, n);
            return matirx;
        }
    }

}
