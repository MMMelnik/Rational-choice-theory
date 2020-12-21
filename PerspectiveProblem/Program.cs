using System;
using System.Linq;

namespace PerspectiveProblem
{
    class Program
    {
        static void Main()
        {
            var dm = new DecisionTreeManager();
            
        }
    }

    public enum TypeOfUsefulnesFunction
    {
        noRisk,
        indifferentToRisk,
        proneToRisk
    }
    public class DataTreeDecision
    {
        public TypeOfUsefulnesFunction TypeOfUsefulnessFunction { get; set; }
        public double[] Incomes { get; set; }
        public double Coefficient { get; set; }
        private double _probability;
        public double Probability
        {
            get { return _probability; }
            set
            {
                if (value > 0 && value < 1)
                {
                    _probability = value;
                }
            }
        }
        public double[] ValuesOfAlternatives { get; set; }
        public double ExpectedUsefulnessFromValuesOfAlternatives { get; set; }
        public double ExpectedUsefulnessOfRiskIncome { get; set; }
        public double ExpectedUsefulnessOfStableIncome { get; set; }
    }
    public class DecisionTreeManager
    {
        public const int ValuesOfStableAndRiskIncomes = 3;
        public static string[] typesOfUserInputForUsefullnessFunction = new string[] { "1", "2", "3" };
        public void DataTreeDecisionProcess(DataTreeDecision data = default)
        {
            if (data == default)
            {
                data = InitDataTreeDecision(data);
            }
            if (data == null)
            {
                Console.WriteLine("Data for decision not initialized");
                return;
            }
            Console.Write("Stable and risk incomes Ds, Dr+, Dr- = ");
            data.Incomes.ToList().ForEach(i => Console.Write($"{i}, "));
            Console.Write("\n");
            Console.WriteLine($"Probability = {data.Probability}");
            Console.Write($"Values of alternatives = ");
            data.ValuesOfAlternatives.ToList().ForEach(a => Console.Write($"{a}, "));
            Console.Write("\n");
            data = ExpectedUsefulnessFromValuesOfAlternatives(data);
            data = ExpectedUsefulnessCalculations(data);
            Console.WriteLine($"Expected usefulness of stable income = {data.ExpectedUsefulnessOfStableIncome}");
            Console.WriteLine($"Expected usefulness of risk income = {data.ExpectedUsefulnessOfRiskIncome}");
            if (data.ExpectedUsefulnessOfStableIncome > data.ExpectedUsefulnessOfRiskIncome)
            {
                Console.WriteLine($"Max is {data.ExpectedUsefulnessOfStableIncome}. Expected usefulness of stable income is better");
            }
            else
            {
                Console.WriteLine($"Max is {data.ExpectedUsefulnessOfRiskIncome}. Expected usefulness of risk income is better");
            }
        }
        public DataTreeDecision InitDataTreeDecision(DataTreeDecision data = default)
        {
            if (data == default)
            {
                data = new DataTreeDecision();
                Console.WriteLine("Enter values of incomes");
                var incomes = Array.ConvertAll(Console.ReadLine()?.Split(' '), double.Parse);
                if (incomes.Length == 0)
                {
                    Console.WriteLine("Incomes values not entered correctly");
                    return null;
                }
                if (incomes[1] > incomes[0] && incomes[2] < incomes[0])
                {
                    data.Incomes = incomes;
                }
                else
                {
                    Console.WriteLine("Incomes values are not correct");
                    return null;
                }
                Console.WriteLine("Enter values of alternatives");
                var valuesOfAlternatives = Array.ConvertAll(Console.ReadLine()?.Split(' '), double.Parse);
                if (valuesOfAlternatives.Length == 0 || !valuesOfAlternatives.All(i => i > 0 && i < 1) || valuesOfAlternatives.Sum() != 1)
                {
                    Console.WriteLine("values Of Alternatives are not correct");
                    return null;
                }
                data.ValuesOfAlternatives = valuesOfAlternatives;
                Console.WriteLine("Enter type of function of usefulness. No risk - 1, " +
                "indifferent to risk - 2, prone to risk - 3, all types - a. Enter 1 or 2 or 3 ");
                var valueString = Console.ReadLine().Trim();
                if (!typesOfUserInputForUsefullnessFunction.ToList().Contains(valueString))
                {
                    Console.WriteLine("The type of function of usefullnes is not correct");
                    return null;
                }
                if (valueString.Equals(typesOfUserInputForUsefullnessFunction[0]))
                {
                    data.TypeOfUsefulnessFunction = TypeOfUsefulnesFunction.noRisk;
                }
                else if (valueString.Equals(typesOfUserInputForUsefullnessFunction[1]))
                {
                    data.TypeOfUsefulnessFunction = TypeOfUsefulnesFunction.indifferentToRisk;
                }
                else if (valueString.Equals(typesOfUserInputForUsefullnessFunction[2]))
                {
                    data.TypeOfUsefulnessFunction = TypeOfUsefulnesFunction.proneToRisk;
                }
                Console.WriteLine("Enter coefficient for function of usefullnes");
                var coefficient = Double.Parse(Console.ReadLine());
                if (coefficient == default)
                {
                    Console.WriteLine("Coefficient for function of usefullnes is not correct");
                    return null;
                }
                data.Coefficient = coefficient;
                Console.WriteLine("Enter probability P risk income");
                var probability = Double.Parse(Console.ReadLine());
                if (probability == default || probability < 0 || probability > 1)
                {
                    Console.WriteLine("Probability P risk income is not correct");
                    return null;
                }
                data.Probability = probability;
            }
            return data;
        }
        public DataTreeDecision ExpectedUsefulnessFromValuesOfAlternatives(DataTreeDecision data)
        {
            if (data.TypeOfUsefulnessFunction.Equals(TypeOfUsefulnesFunction.noRisk))
            {
                Console.WriteLine($"User's choice of usefulness function is no risk. Coefficient = {data.Coefficient}");
                data.ExpectedUsefulnessFromValuesOfAlternatives = GetExpectedUsefulnessFromValuesOfAlternatives(data.Incomes, data.ValuesOfAlternatives, TypeOfUsefulnesFunction.noRisk, data.Coefficient);
            }
            else if (data.TypeOfUsefulnessFunction.Equals(TypeOfUsefulnesFunction.indifferentToRisk))
            {
                Console.WriteLine($"User's choice of usefulness function is indifferent to risk. Coefficient = {data.Coefficient} ");
                data.ExpectedUsefulnessFromValuesOfAlternatives = GetExpectedUsefulnessFromValuesOfAlternatives(data.Incomes, data.ValuesOfAlternatives, TypeOfUsefulnesFunction.indifferentToRisk, data.Coefficient);
            }
            else if (data.TypeOfUsefulnessFunction.Equals(TypeOfUsefulnesFunction.proneToRisk))
            {
                Console.WriteLine($"User's choice of usefulness function is prone to risk. Coefficient = {data.Coefficient} ");
                data.ExpectedUsefulnessFromValuesOfAlternatives = GetExpectedUsefulnessFromValuesOfAlternatives(data.Incomes, data.ValuesOfAlternatives, TypeOfUsefulnesFunction.proneToRisk, data.Coefficient);
            }
            Console.WriteLine($"Expected Usefulness From Values Of Alternatives {data.ExpectedUsefulnessFromValuesOfAlternatives}");
            return data;
        }
        public double GetExpectedUsefulnessFromValuesOfAlternatives(double[] incomes, double[] valuesOfAlternatives, TypeOfUsefulnesFunction typeOfUsefullnesFunction, double coefficient)
        {
            Console.WriteLine("Expected usefulness of values of alternatives calculations ...");
            var res = 0.0;
            for (int i = 0; i < ValuesOfStableAndRiskIncomes; i++)
            {
                var step = UsefulnessStep(typeOfUsefullnesFunction, coefficient, incomes[i]);
                Console.WriteLine($"result = {res} + {valuesOfAlternatives[i]} * {step}");
                res = Math.Round(res + (valuesOfAlternatives[i] * step), 3);
            }
            return res;
        }
        public double UsefulnessStep(TypeOfUsefulnesFunction typeOfUsefullnesFunction, double coefficient, double income)
        {
            switch (typeOfUsefullnesFunction)
            {
                case TypeOfUsefulnesFunction.noRisk:
                    var res1 = Math.Round(coefficient * Math.Sqrt(income), 3);
                    Console.WriteLine($"{coefficient} * sqrt( {income} ) = {res1}");
                    return res1;
                case TypeOfUsefulnesFunction.indifferentToRisk:
                    var res2 = Math.Round(coefficient * income, 3);
                    Console.WriteLine($"{coefficient} * {income} = {res2}");
                    return res2;
                case TypeOfUsefulnesFunction.proneToRisk:
                    var res3 = Math.Round(coefficient * Math.Pow(income, 2), 3);
                    Console.WriteLine($"{coefficient} * {income}^2 = {res3}");
                    return res3;
                default:
                    throw new Exception("Type of usefullnes function not correct");
            }
        }
        public DataTreeDecision ExpectedUsefulnessCalculations(DataTreeDecision data)
        {
            data.ExpectedUsefulnessOfRiskIncome = GetExpectedUsefulnessOfRiskIncome(data.TypeOfUsefulnessFunction, data.Incomes, data.Probability, data.Coefficient);
            data.ExpectedUsefulnessOfStableIncome = GetExpectedUsefulnessOfStableIncome(data.TypeOfUsefulnessFunction, data.Incomes, data.Probability, data.Coefficient);
            return data;
        }
        public double GetExpectedUsefulnessOfRiskIncome(TypeOfUsefulnesFunction typeOfUsefullnesFunction, double[] incomes, double probability, double coefficient)
        {
            Console.WriteLine("Expected usefulness of risk income calculations ... ");
            var Udrplus = UsefulnessStep(typeOfUsefullnesFunction, coefficient, incomes[1]);
            var Udrminus = UsefulnessStep(typeOfUsefullnesFunction, coefficient, incomes[2]);
            var res = Math.Round(probability * Udrplus + (1 - probability) * Udrminus, 3);
            Console.WriteLine($"{probability} * {Udrplus} + ({1 - probability}) * {Udrminus} = {res}");
            return res;
        }
        public double GetExpectedUsefulnessOfStableIncome(TypeOfUsefulnesFunction typeOfUsefullnesFunction, double[] incomes, double probability, double coefficient)
        {
            Console.WriteLine("Expected usefulness of stable income calculations ... ");
            var Uds = UsefulnessStep(typeOfUsefullnesFunction, coefficient, incomes[0]);
            Console.WriteLine($"{probability} * {Uds} + ({1 - probability}) * {Uds} = {Uds}");
            return Uds;
        }
    }
}
