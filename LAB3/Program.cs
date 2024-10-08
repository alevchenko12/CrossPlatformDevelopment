/*
  У парку флори та фауни затіяли масштабну перебудову. 
  Організатори запланували розширення площі парку, збільшення кількості екзотичних тварин та будівництво нових вольєрів. 
  Після затвердження плану будівельники та зоологи взялися до роботи.
  Зоологи зі своїм завданням впоралися: привезли нових жирафів, довгоочікуваних слонів, ігуан з карибських островів та багатьох інших тварин та птахів. 
  А ось будівельники не встигли добудувати нові вольєри, тож привезених тварин було вирішено тимчасово розмістити у клітках.
  Однак і це завдання виявилося непростим, оскільки клітин може не вистачити для привезених тварин. 
  А в одну клітинку можна помістити лише сумісних тварин. 
  Зоологи склали таблицю сумісності тварин, представивши її як матриці A={aij} розміром N×N. 
  Якщо тварини з номерами i і j сумісні, то aij = 0, і якщо - ні, то aij = 1. 
  Необхідно визначити мінімальну кількість клітин для безпечного розміщення тварин, коли у всіх клітинах знаходяться лише сумісні між собою тварини. 
  При цьому в клітині може бути одна, дві і більше тварин.

  Вхідні дані
    Перший рядок вхідного файлу INPUT.TXT містить число N - кількість тварин (1 ≤ N ≤ 18). 
    Далі йде N рядків по N чисел у кожному – матриця сумісності тварин.

Вихідні дані
    У вихідний файл OUTPUT.TXT виведіть одне ціле число – мінімальна кількість клітин, яка потрібна для безпечного розміщення тварин.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Headers;

namespace LAB3
{
    public class Program
    {
        public static void ValidateInput(string[] lines)
        {
            // Перевірка, що перший рядок містить одне число N - кількість тварин
            if (lines.Length < 1)
            {
                throw new InvalidOperationException("The input file must contain at least one line.");
            }

            string firstLine = lines[0].Trim();

            // Перевірка, що перший рядок не порожній і містить ціле число
            if (!int.TryParse(firstLine, out int N))
            {
                throw new InvalidOperationException("The first line must contain a valid natural number.");
            }

            // Перевірка, що число N знаходиться в межах [1, 18]
            if (N < 1 || N > 18)
            {
                throw new InvalidOperationException("The number N must be between 1 and 18.");
            }

            // Перевірка наявності N рядків 
            if (lines.Length != N + 1)
            {
                throw new InvalidOperationException("The number of lines must match N + 1.");
            }

            for (int i = 1; i <= N; i++)
            {
                string[] row = lines[i].Trim().Split(' ');

                // Перевірка, що кожен рядок містить N елементів
                if (row.Length != N)
                {
                    throw new InvalidOperationException($"Line {i} must contain exactly {N} elements.");
                }

                // Перевірка, що кожен елемент є або 0, або 1
                foreach (string element in row)
                {
                    if (element != "0" && element != "1")
                    {
                        throw new InvalidOperationException($"Each element in line {i} must be either 0 or 1.");
                    }
                }
            }
        }

        public static (int N, int[][] compatibilityMatrix) ParseInput(string[] lines)
        {
            int N = int.Parse(lines[0].Trim());

            int[][] compatibilityMatrix = new int[N][];

            for (int i = 0; i < N; i++)
            {
                string[] row = lines[i+1].Trim().Split(' ');
                compatibilityMatrix[i] = Array.ConvertAll(row, int.Parse);
            }

            return (N, compatibilityMatrix);
        }

        public static int SolveProblem(int N, int[][] compatibilitymatrix)
        {
           return FindMinCages(N, compatibilitymatrix);   
        }

        // Функція для знаходження мінімальної кількості кліток
        public static int FindMinCages(int N, int[][] compatibilityMatrix)
        {
            // dp[mask] буде містити мінімальну кількість кліток для підмножини тварин, позначених маскою mask
            int[] dp = new int[1 << N];

            // Готуємо масив, який міститиме інформацію про те, які підмножини тварин можна розмістити в одній клітці
            bool[] canBeInOneCage = new bool[1 << N];

            for (int mask = 0; mask < (1 << N); mask++)
            {
                bool isValid = true;
                for (int i = 0; i < N && isValid; i++)
                {
                    if ((mask & (1 << i)) == 0) continue;

                    for (int j = i + 1; j < N; j++)
                    {
                        if ((mask & (1 << j)) != 0 && compatibilityMatrix[i][j] == 1)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
                canBeInOneCage[mask] = isValid;
            }

            // Ініціалізація dp-масиву великими значеннями
            for (int mask = 0; mask < (1 << N); mask++)
            {
                dp[mask] = N; // Максимум - кожна тварина в окремій клітці
            }

            dp[0] = 0; // Нульова підмножина не потребує кліток

            // Динамічне програмування для обчислення мінімальної кількості кліток
            for (int mask = 0; mask < (1 << N); mask++)
            {
                int submask = mask;
                while (submask > 0)
                {
                    if (canBeInOneCage[submask])
                    {
                        dp[mask] = Math.Min(dp[mask], dp[mask ^ submask] + 1);
                    }
                    submask = (submask - 1) & mask; // Перебираємо всі підмножини
                }
            }

            return dp[(1 << N) - 1];
        }

        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                string inputFilePath = args.Length > 0 ? args[0] : Path.Combine("LAB3", "INPUT.TXT");
                string outputFilePath = Path.Combine("LAB3", "OUTPUT.TXT");               
                
                string[] lines = File.ReadAllLines(inputFilePath);
               
                ValidateInput(lines);
                
                var (N, compatibilityMatrix) = ParseInput(lines);
                
                int result = SolveProblem(N, compatibilityMatrix);
                
                File.WriteAllText(outputFilePath, result.ToString());
                
                Console.WriteLine("File OUTPUT.TXT successfully created");
                Console.WriteLine("LAB #3");
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