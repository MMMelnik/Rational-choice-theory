using System;

namespace DiagnosticTest
{
    class Program
    {
        private static double _tp, _fp, _fn, _tn, _l1, _l2;

        static void Main()
        {
            //input martix
            GetMatrix();
            // втрата від пропуску цілі
            GetNegativeLoss();
            // хибна тривога
            GetPositiveLoss();
            var diagnostic = new Diagnostic(_tp, _fp, _fn, _tn, _l1, _l2);
            diagnostic.Calculate();
            Console.Write(diagnostic.ToString());
        }

        private static void GetNegativeLoss()
        {
            Console.Write("Введіть втрату для хибно негативного результату L1: ");
            Console.Write("\n");
            _l1 = double.Parse(Console.ReadLine() ?? "1.0");
        }

        private static void GetPositiveLoss()
        {
            Console.Write("Введіть втрату для хибно негативного результату L2: ");
            Console.Write("\n");
            _l2 = double.Parse(Console.ReadLine() ?? "1.0");
        }

        private static void GetMatrix()
        {
            Console.Write("Введіть TP: ");
            Console.Write("\n");
            _tp = double.Parse(Console.ReadLine() ?? "1.0");
            Console.Write("Введіть FP: ");
            Console.Write("\n");
            _fp = double.Parse(Console.ReadLine() ?? "1.0");
            Console.Write("Введіть FN: ");
            Console.Write("\n");
            _fn = double.Parse(Console.ReadLine() ?? "1.0");
            Console.Write("Введіть TN: ");
            Console.Write("\n");
            _tn = double.Parse(Console.ReadLine() ?? "1.0");
        }
    }
}
