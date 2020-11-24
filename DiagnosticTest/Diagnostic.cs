using System.Text;

namespace DiagnosticTest
{
    class Diagnostic
    {
        private double TP, FP, FN, TN, L12, L21;
        private double SE, SP, w, P, lowerLimit, higherLimit;
        private bool qualitative;

        public Diagnostic(double tp = 467.0, double fp=107.0, double fn=95.0, double tn=3468.0, double l12=100.0, double l21 = 5.0 )
        {
            TP = tp;
            FP = fp;
            FN = fn;
            TN = tn;
            L12 = l12;
            L21 = l21;
        }

        public void Calculate()
        {
            //розрахунок чутливості
            SE = TP/(TP+FN);
            //розрахунок специфічності
            SP = TN/(TN+FP);
            // преваленс
            double positive = TP + FN;
            double negative = FP + TN;
            P = positive / (positive + negative);
            //співвідношення втрат
            this.w = L12 / L21;
            //перевірка корисності
            qualitative = P * (1 + w) < 1 ? (1 - P) * (1 - SP) < w * P * SE
                : (1 - P) * SP > w * P * (1 - SE);
            // визначення інтервалу допустимого співвідношення втрат для тесту
            lowerLimit = (1 - P) * (1 - SP) / (P * SE);
            higherLimit = (1 - P) * SP / (P * (1 - SE));
        }

        public override string ToString()
        {
            var answer = new StringBuilder();

            answer.AppendLine("Розв'язок задачі діагностичного тестування:");
            // чутливість
            answer.AppendLine($"Чутливість: {SE:0.####}");
            // специфічність
            answer.AppendLine($"Специфічність: {SP:0.####}");
            // преваленс
            answer.AppendLine($"Преваленс: {P:0.####}");
            // співвідношення втрат
            answer.AppendLine($"Cпіввідношення втрат: {w:0.####}");
            // якість тесту
            answer.AppendLine($"Тест якісний?: {(qualitative ? "Так" : "Ні")}");
            // інтервал допустимого співвідношення втрат для тесту
            answer.AppendLine($"Інтервал допустимого співвідношення втрат для тесту: {lowerLimit:0.####}<=w<={higherLimit:0.####}");

            return answer.ToString();
        }
    }
}
