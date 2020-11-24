using System;

namespace ELECTRE
{
	public class TaskIn
	{
		private void getNums()
		{
			Console.Write("Кiлькість альтернатив: ");
			aNum = Int32.Parse(Console.ReadLine());
			Console.Write("Кiлькість критеріїв: ");
			kNum = Int32.Parse(Console.ReadLine());
			Console.Write("Порогове значення індексу згоди: ");
			a0 = double.Parse(Console.ReadLine());
			Console.Write("Порогове значення індексу незгоди: ");
			b0 = double.Parse(Console.ReadLine());
		}
		// Метод для отримання ваг критеріїв
		private void getW()
		{
			w = new double[kNum];
			Console.Write("Введіть ваги критеріїв w");
			Console.Write("\n");
            w = Array.ConvertAll(Console.ReadLine().Split(" "), double.Parse);
        }

		private void getD()
		{
			d = new double[aNum, kNum];
            for (int i = 0; i < aNum; i++)
			{
				Console.Write("\n");
				Console.Write("A");
				Console.Write(i);
				Console.Write(" ");
                var tmp = Array.ConvertAll(Console.ReadLine().Split(" "), double.Parse);

                for (int j = 0; j < kNum; j++)
                {
                    d[i, j] = tmp[j];
                }
			}
		}

		public double[,] d;
		public double[] w;
		public int kNum;
		public int aNum;
		public double a0;
		public double b0;

		public void GetValue()
		{
			getNums();
			getW();
			getD();
		}
	}

    public class Electre
	{
		private int kNum;
		private int aNum;
		private double a0;
		private double b0;
		private double[] w;
        private double[,] d;
        private double[,] a;
        private double[,] b;

		public Electre(int _kNum, int _aNum, double _a0, double _b0)
		{
			a0 = _a0;
			b0 = _b0;
			kNum = _kNum;
			aNum = _aNum;
			w = new double[_kNum];
			d = new double[_aNum, _kNum];
            a = new double[_aNum, _aNum];
            b = new double[_aNum, _aNum];
        }
		
		public void SetMatrix(double[,] _d, double[] _w)
		{
			for (int i = 0; i < aNum; i++)
			{
				for (int j = 0; j < kNum; j++)
				{
                    d[i, j] = _d[i ,j];
				}
			}
			for (int j = 0; j < kNum; j++)
			{
				w[j] = _w[j];
			}
		}

        public void CalculateAB()
        {
            int[] I = new int[kNum];
            double wSum = 0;
            double wIsum = 0;
            for (int i = 0; i < aNum; i++)
            {
                for (int j = 0; j < aNum; j++)
                {
                    b[i,j] = 0;
                    for (int k = 0; k < kNum; k++)
                    {
                        if (d[i,k] > d[j,k])
                        {
                            I[k] = 1;
                        }
                        else if (d[i,k] == d[j,k])
                        {
                            I[k] = 0;
                        }
                        else
                        {
                            I[k] = -1;
                        }
                    }

                    for (int k = 0; k < kNum; k++)
                    {
                        wSum += w[k];
                        if (I[k] >= 0)
                        {
                            wIsum += w[k];
                        }
                        else
                        {
                            var temp = Math.Abs((d[i,k] - d[j,k]) / 8);
                            if (temp > b[i,j])
                            {
                                b[i,j] = temp;
                            }
                        }
                    }

                    a[i,j] = wIsum / wSum;
                    wSum = 0;
                    wIsum = 0;
                }
            }

            Console.Write("A:");
            Console.Write("\n");
            for (int i = 0; i < aNum; i++)
            {
                for (int j = 0; j < aNum; j++)
                {
                    Console.Write($"{a[i, j]:0.000}");
                    Console.Write(" ");
                }

                Console.Write("\n");
            }

            Console.Write("B:");
            Console.Write("\n");
            for (int i = 0; i < aNum; i++)
            {
                for (int j = 0; j < aNum; j++)
                {
                    Console.Write($"{b[i, j]:0.000}");
                    Console.Write(" ");
                }

                Console.Write("\n");
            }
        }

		public void PrintAnswer()
        {
            int[] ans = new int[aNum];
            Console.Write("\n");
            Console.Write("X ");
            for (int i = 0; i < aNum; i++)
            {
                Console.Write("A");
                Console.Write(i);
                Console.Write(" ");
            }
            Console.Write("\n");
            int max = 0;
            for (int i = 0; i < aNum; i++)
            {
                ans[i] = 0;
                Console.Write("A");
                Console.Write(i);
                Console.Write(" ");
                for (int j = 0; j < aNum; j++)
                {
                    if (i == j)
                    {
                        Console.Write("* ");
                    }
                    else if (a[i,j] >= a0 && b[i,j] <= b0)
                    {
                        ans[i] += 1;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("< ");
                    }
                }
                max = max < ans[i] ? ans[i] : max;
                Console.Write("\n");
            }
            Console.Write("kernel: ");
            for (int i = 0; i < aNum; i++)
            {
                if (ans[i] == max)
                {
                    Console.Write("A");
                    Console.Write(i);
                    Console.Write(" ");
                }
            }
        }
	}

	class Program
    {
        public double[,] d = {
            { 7, 8, 6, 7, 7},
            { 9, 6, 3, 1, 1},
            {5, 5, 9, 5, 5},
            { 5, 8, 9, 7, 6}
        };

        public double[] w = new double[5] { 9, 5, 3, 1, 5 };

		static void Main(string[] args)
        {
            TaskIn t = new TaskIn();
            t.GetValue();
            Electre e = new Electre(t.kNum, t.aNum, t.a0, t.b0);
            e.SetMatrix(t.d, t.w);
            e.CalculateAB();
            e.PrintAnswer();
        }
    }
}
