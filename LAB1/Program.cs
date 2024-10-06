/*
 Дано рядок, що складається із N символів. Потрібно вивести всі перестановки символів цього рядка.

Вхідні дані
Вхідний файл INPUT.TXT містить рядок, що складається з N символів (1 ≤ N ≤ 8), символи – літери англійського алфавіту та цифри.

Вихідні дані
У вихідний файл OUTPUT.TXT виведіть у кожному рядку за однією перестановкою. Перестановки можна виводити у будь-якому порядку. Повторень і рядків, які є перестановками вихідної, не повинно бути.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LAB1
{
    public class Program
    {
        // Множина для зберігання унікальних перестановок
        static HashSet<string> uniquePermutations = new HashSet<string>();

        // Об'єкт для зберігання результатів
        static StringBuilder resultBuilder = new StringBuilder();

        // Функція для обміну символів у рядку
        static string Swap(string s, int i, int j)
        {
            char[] charArray = s.ToCharArray();
            char temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            return new string(charArray);
        }

        // Рекурсивна функція для генерації перестановок
        static void PermuteRec(string s, int idx)
        {
            // Базовий випадок
            if (idx == s.Length - 1)
            {
                // Додаємо перестановку в множину, якщо її там ще немає
                if (!uniquePermutations.Contains(s))
                {
                    uniquePermutations.Add(s);
                    resultBuilder.AppendLine(s); // Збираємо унікальні перестановки
                }
                return;
            }

            // Рекурсія і генерація перестановок
            for (int i = idx; i < s.Length; i++)
            {
                // Міняємо місцями символи
                s = Swap(s, idx, i);

                // Рекурсивно фіксуємо перші idx+1 символів
                PermuteRec(s, idx + 1);

                // Відкат змін (backtracking)
                s = Swap(s, idx, i);
            }
        }

        // Обгортка для виклику рекурсивної функції
        static void Permute(string s)
        {
            PermuteRec(s, 0);
        }

        public static string SolveProblem(string s)
        {
            uniquePermutations.Clear();
            resultBuilder.Clear();
            Permute(s);
            return resultBuilder.ToString();
        }

        public static void ValidateInput(string[] lines)
        {
            // Перевірка, що файл містить лише один рядок
            if (lines.Length != 1)
            {
                throw new InvalidOperationException("The input file must contain exactly one line.");
            }

            string line = lines[0].Trim();

            // Перевірка довжини рядка
            if (line.Length < 1 || line.Length > 8)
            {
                throw new InvalidOperationException("The input string length must be between 1 and 8 characters.");
            }

            // Перевірка на допустимі символи (тільки літери англійського алфавіту та цифри)
            foreach (char ch in line)
            {
                if (!(char.IsLetterOrDigit(ch) && ch <= 127)) // Символи лише ASCII (літери + цифри)
                {
                    throw new InvalidOperationException("The input string must only contain English letters and digits.");
                }
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;                
                string inputFilePath = args.Length > 0 ? args[0] : Path.Combine("LAB1", "INPUT.TXT");
                string outputFilePath = Path.Combine("LAB1", "OUTPUT.TXT");                

                // Зчитуємо всі рядки з файлу
                string[] lines = File.ReadAllLines(inputFilePath);

                // Валідація вхідних даних
                ValidateInput(lines);

                // Обробка єдиного рядка
                string result = SolveProblem(lines[0]); // Оскільки має бути лише один рядок
                File.WriteAllText(outputFilePath, result.Trim());
                    
                Console.WriteLine("File OUTPUT.TXT successfully created");
                Console.WriteLine("LAB #1");
                Console.WriteLine("Input data:");
                Console.WriteLine(lines[0].Trim());
                Console.WriteLine("Output data:");
                Console.WriteLine(result.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine('\n');
        }
    }
}
