using System;
using System.Collections.Generic;

namespace PerspectiveSolver
{
    class Setter
    {
        public Setter()
        {
        }
        public Setter(Data data)
        {
            Data = data;
        }
        public Data Data { get; set; }
        public void Set()
        {
            Data.F = F();
            Data.K = K();
            Data.P = P();
            Data.D = D();
        }
        private double K()
        {
            Console.WriteLine("Введіть значення K");
            try
            {
                return Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Введене значення не є числом! ");
                return K();
            }
        }
        private double P()
        {
            Console.WriteLine("Введіться значення ймовірності [0;1]");
            double p = -1;
            while (p < 0 || p > 1)
            {
                try
                {
                    p = Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    p = -1;
                }
            }
            return p;
        }
        private int F()
        {
            int f = 0;
            while (f < 1 || f > 3)
            {
                try
                {
                    Console.WriteLine("Оберіть функцію:\n 1 (не бажає ризикувати: SQRT(I)), 2 (нейтральна до ризику: I) or 3 (схильна до ризику: I^2)");
                    f = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    f = 0;
                }
            }
            return f;
        }
        private List<double> D()
        {
            // перевірка
            List<double> D = new List<double>() { 0, 0, 0 };
            while (!(D[1] > D[0] && D[0] > D[2] && D[2] > 0))
            {
                try
                {
                    Console.WriteLine("Dr+ > Ds > Dr- > 0");
                    Console.Write("стабільний дохід: Ds = ");
                    D[0] = Convert.ToDouble(Console.ReadLine());
                    Console.Write("ризикований дохід: Dr+ = ");
                    D[1] = Convert.ToDouble(Console.ReadLine());
                    Console.Write("(з ймовірністю 1-p) ризикований дохід: Dr- = ");
                    D[2] = Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    D[0] = 0;
                    D[1] = 0;
                    D[2] = 0;
                }
            }
            return D;
        }
    }
}
