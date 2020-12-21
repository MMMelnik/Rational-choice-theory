using System;

namespace PerspectiveSolver
{
    class Solver
    {
        public Solver()
        {
        }
        public Solver(Data data)
        {
            Data = data;
        }
        public Data Data { get; set; }
        public void Calc()
        {
            CaclUDs();
            CaclUDr();
        }
        private void CaclUDs()
        {
            Data.U[0] = (Data.K * ChooseF(Data.D[0]));
        }
        private void CaclUDr()
        {
            Data.U[1] = (Data.P * Data.K * ChooseF(Data.D[1])) + ((1 - Data.P) * Data.K * ChooseF(Data.D[2]));
        }
        private double ChooseF(double x)
        {
            if (Data.F == 1)
            {
                return Math.Sqrt(x);
            }
            if (Data.F == 2)
            {
                return x;
            }
            else
            {
                return x * x;
            }
        }
    }
}
