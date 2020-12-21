using System.Collections.Generic;

namespace PerspectiveSolver
{
    class Data
    {
        public Data()
        {
            U.Add(0);
            U.Add(0);
        }
        public Data(List<double> d, double k, double p, int f) : this()
        {
            D = d;
            K = k;
            P = p;
            F = f;
        }
        public List<double> D { get; set; }
        public double K { get; set; }
        public double P { get; set; }
        public int F { get; set; }
        public List<double> U = new List<double>();
    }
}
