namespace PerspectiveSolver
{
    class Program
    {
        static void Main()
        {
            var data = new Data();
            var setter = new Setter(data);
            setter.Set();
            Solver calculator = new Solver(data);
            calculator.Calc();
            var printer = new Output(data);
            printer.Print();
        }
    }
}
