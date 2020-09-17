using System;
using System.Linq;

namespace AnalyticHierarchyProcess
{
    internal class Program
    {
        // https://www.youtube.com/watch?v=RLvDgwQZBoQ&ab_channel=VitarkaKadapa
        // Input criteria matrix
        private static double[,] _a = new double[4,4];
        private static double[] _y = new double[4];
        private static double[] _w = new double[4];
        private static double[] _x = new double[4];
        private static double _lamda;
        private static readonly double RiMatrix4 = 0.9; // random consistency index for x4 matrix Saaty, 1980 
        private static double[,] Options = new double[4,4];

        static void Main()
        {
            // Get the Matrix
            //GetData();
            GetTestData();
            // Calculate column vector value
            _w = CalculateColumnVector(_a);
            Console.WriteLine($"Priority vector =\n [{string.Join(", ", _w)}]");

            _x = NormalizeVector(_w);
            Console.WriteLine($"Normalized priority vector =\n [{string.Join(", ", _x)}]");

            _y = MultiplyMatrixByVector(_a, _x);
            Console.WriteLine($"Multiplication of priority vector by priority comparison matrix =\n [{string.Join(", ", _y)}]");

            _lamda = GetLambda(_y, _x);
            Console.WriteLine($"Lambda max = [{string.Join(", ", _lamda)}]");

            if (ConsistencyTest(_lamda))
            {
                Console.WriteLine("Consistency Test is passed!");
            }
            else
            {
                Console.WriteLine("Consistency Test is failed!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            var optionsVector = NormalizeVector(GetOptionsVector(Options, _x));
            for (int i=1; i <= 4; i++)
            {
               
                Console.WriteLine($"Option {i} has weight of { optionsVector[i-1]}");
            }

            Console.WriteLine("Options " + (1+Array.IndexOf(optionsVector, optionsVector.Max())) + " топ за свої деньгі!");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void GetData()
        {
            Console.Write("Input elements for the pair-wise criteria comparison matrix :\n");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write("element - [{0}],[{1}] : ", i, j);
                    _a[i, j] = Convert.ToDouble(Console.ReadLine());
                }
            }

            Console.Write("Input elements for the options matrix :\n");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write("element - Option - [{0}], Criteria - [{1}] : ", i, j);
                    Options[i, j] = Convert.ToDouble(Console.ReadLine());
                }
            }
        }

        static void GetTestData()
        {
            _a = new[,] {
                { 1, 1d/7, 1d/5, 1d/3},
                { 7, 1, 2, 3},
                { 5, 1d/2, 1, 3},
                { 3, 1d/3, 1d/3, 1}
            };

            Options = new[,] {
                { 0.1428, 1, 0.6, 1},
                { 1, 0.8823, 0.2, 0.0003},
                { 0.0666, 0.7988, 1, 0.5},
                { 0.0666, 1, 1, 1}
            };
        }

        static double[] CalculateColumnVector(double[,] a1)
        {
            var columnVector = new double[4];
            for (int i = 0; i < 4; i++)
            {
                columnVector[i] = Math.Pow((a1[i, 0] * a1[i, 1] * a1[i, 2] * a1[i, 3]), 1d/4);
            }

            return columnVector;
        }

        static double[] NormalizeVector(double[] w)
        {
            double sum = 0;
            foreach (var value in w)
            {
                sum += value;
            }

            for (int i = 0; i < 4; i++)
            {
                w[i] /= sum;
            }

            return w;
        }

        static double[] MultiplyMatrixByVector(double[,] a1, double[] w)
        {
            var resultingVector = new double[4];

            for (int i = 0; i < 4; i++)
            {
                double rowElement = 0;

                for (int j = 0; j < 4; j++)
                {
                    rowElement += a1[i, j] * w[j];
                }

                resultingVector[i] = rowElement;
            }

            return resultingVector;
        }

        static double GetLambda(double[] vectorY, double[] vectorX)
        {
            int n = 4;
            double tmp = 0;

            for (int i = 0; i < n; i++)
            {
                tmp += vectorY[i] / vectorX[i];
            }

            return tmp / n;
        }

        static bool ConsistencyTest(double lamda)
        {
            var n = 4;
            var I = ((lamda-n)/(n-1))/RiMatrix4;
            Console.WriteLine($"Consistency index = {I}");
            return I < 0.3 ;
            // return I < 0.1; // use it if for higher accuracy 
        }

        static double[] GetOptionsVector(double[,] optionsMatrix, double[] criteriaVector)
        {
            return MultiplyMatrixByVector(optionsMatrix, criteriaVector);
        }
    }
}