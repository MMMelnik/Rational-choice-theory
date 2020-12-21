using System;

namespace PerspectiveSolver
{
    class Output
    {
        public Output() { }
        public Output(Data data)
        {
            Data = data;
        }
        public Data Data { get; set; }
        public void Print()
        {
            if (Data.U[0] >= Data.U[1])
            {
                Console.WriteLine("Рішення: краще не ризикувати");
            }
            else
            {
                Console.WriteLine("Рішення: краще ризикувати");
            }
            Console.WriteLine("Safe {0}", Data.U[0]);
            Console.WriteLine("Risk {0}", Data.U[1]);
        }
    }
}
