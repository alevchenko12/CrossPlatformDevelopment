/*
 Автомобільні пробки трапляються скрізь, навіть у нашому маленькому місті. 
 Дороги у нас мають по дві смуги в одному напрямку, а автомобілі лише двох видів: 
    легкові (у пробці займають квадратне місце 1×1 від ширини однієї смуги) 
    вантажні (займають прямокутне місце 1×2). 
 Автомобілісти дуже дисципліновані: не стають упоперек смуги, не займають чужої площі, але й не залишають вільних місць.
 Потрібно написати програму, яка визначить кількість різних автомобільних заторів довжини N.

 Вхідні дані
 Вхідний файл INPUT.TXT містить одне натуральне число N (N ≤ 1000).

 Вихідні дані
 Вихідний файл OUTPUT.TXT повинен мати знайдену кількість автомобільних пробок.

 */

using System;
using System.IO;
using System.Text;

namespace LAB2
{
    public class Program
    {
        public static void ValidateInput(string[] lines)
        {
            // Перевірка, що файл містить лише один рядок
            if (lines.Length != 1)
            {
                throw new InvalidOperationException("The input file must contain exactly one line.");
            }

            string line = lines[0].Trim();

            // Перевірка, що рядок не порожній
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new InvalidOperationException("The input string must not be empty.");
            }

            // Перевірка, що рядок містить лише цифри
            if (!int.TryParse(line, out int N))
            {
                throw new InvalidOperationException("The input string must be a valid natural number.");
            }

            // Перевірка, що число N є натуральним (тобто N ≥ 1) і не перевищує 1000
            if (N < 1 || N > 1000)
            {
                throw new InvalidOperationException("The number N must be a natural number between 1 and 1000.");
            }
        }

        public static int CountTrafficJams(int N)
        {
            // Base cases
            if (N == 0) return 1;
            if (N == 1) return 1;

            // DP array for the number of ways to fill a 2×N grid
            int[] dp = new int[N + 1];

            dp[0] = 1; // No space, 1 way to fill (do nothing)
            dp[1] = 1; // One block, only 1 way (one car)

            // Fill the dp array using the recurrence relation
            for (int i = 2; i <= N; i++)
            {
                dp[i] = dp[i - 1] + dp[i - 2];
            }

            // Since the number of configurations for two directions is independent, the result is dp[N]^2
            int result = dp[N] * dp[N];            
            return result;
        }

        public static int SolveProblem(string line)
        {
            // Convert input string to integer
            int N = int.Parse(line);

            // Call the function to calculate traffic jams
            int result = CountTrafficJams(N);

            // Return the result as an integer
            return result;
        }

        public static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                string inputFilePath = args.Length > 0 ? args[0] : Path.Combine("LAB2", "INPUT.TXT");
                string outputFilePath = Path.Combine("LAB2", "OUTPUT.TXT");

                // Read input from file
                string[] lines = File.ReadAllLines(inputFilePath);

                // Validate input
                ValidateInput(lines);
                string line = lines[0].Trim();

                // Solve problem and get result
                int result = SolveProblem(line);

                // Write result to OUTPUT.TXT
                File.WriteAllText(outputFilePath, result.ToString());

                // Output to console
                Console.WriteLine("File OUTPUT.TXT successfully created");
                Console.WriteLine("LAB #2");
                Console.WriteLine("Input data:");
                Console.WriteLine(string.Join(Environment.NewLine, lines).Trim());
                Console.WriteLine("Output data:");
                Console.WriteLine(result.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine('\n');
        }
    }
}