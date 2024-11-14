
using LAB5.Models;
using Microsoft.AspNetCore.Mvc;


namespace LAB5.Controllers
{
    public class LabController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public LabController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Lab1()
        {
            var model = new LabViewModel
            {
                TaskNumber = "1",
                TaskVariant = "64",
                TaskDescription = "Дано рядок, що складається із N символів.\r\nПотрібно вивести всі перестановки символів цього рядка.\r\n",
                InputDescription = "Вхідний файл INPUT.TXT містить рядок, що складається з N символів (1 ≤ N ≤ 8), символи – літери англійського алфавіту та цифри.",
                OutputDescription = "У вихідний файл OUTPUT.TXT виведіть у кожному рядку за однією перестановкою.\r\nПерестановки можна виводити у будь-якому порядку.\r\nПовторень і рядків, які є перестановками вихідної, не повинно бути.",
                TestCases = new List<TestCase>
            {
                new TestCase { Input = "AB", Output = "AB\nBA" },
                new TestCase { Input = "122", Output = "122\n212\n221" }
            }
            };
            return View(model);
        }

        public IActionResult Lab2()
        {
            var model = new LabViewModel
            {
                TaskNumber = "2",
                TaskVariant = "64",
                TaskDescription = "Автомобільні пробки трапляються скрізь, навіть у нашому маленькому місті.\r\nДороги у нас мають по дві смуги в одному напрямку, а автомобілі лише двох видів:\r\nлегкові (у пробці займають квадратне місце 1×1 від ширини однієї смуги)\r\nта вантажні (займають прямокутне місце 1×2).\r\nАвтомобілісти дуже дисципліновані: не стають упоперек смуги, не займають чужої площі, але й не залишають вільних місць.\r\nПотрібно написати програму, яка визначить кількість різних автомобільних заторів довжини N.\r\n",
                InputDescription = "Вхідний файл INPUT.TXT містить одне натуральне число N (N ≤ 1000).\r\n",
                OutputDescription = "Вихідний файл OUTPUT.TXT повинен мати знайдену кількість автомобільних пробок.",
                TestCases = new List<TestCase>
            {
                new TestCase { Input = "2", Output = "4" },
                new TestCase { Input = "3", Output = "9" }
            }
            };
            return View(model);
        }

        public IActionResult Lab3()
        {
            var model = new LabViewModel
            {
                TaskNumber = "3",
                TaskVariant = "64",
                TaskDescription = "У парку флори та фауни затіяли масштабну перебудову.\r\nОрганізатори запланували розширення площі парку, збільшення кількості екзотичних тварин та будівництво нових вольєрів.\r\nПісля затвердження плану будівельники та зоологи взялися до роботи.\r\nЗоологи зі своїм завданням впоралися: привезли нових жирафів, довгоочікуваних слонів, ігуан з карибських островів та багатьох інших тварин та птахів.\r\nА ось будівельники не встигли добудувати нові вольєри, тож привезених тварин було вирішено тимчасово розмістити у клітках.\r\nОднак і це завдання виявилося непростим, оскільки клітин може не вистачити для привезених тварин.\r\nА в одну клітинку можна помістити лише сумісних тварин.\r\nЗоологи склали таблицю сумісності тварин, представивши її як матриці A={aij} розміром N×N.\r\nЯкщо тварини з номерами i і j сумісні, то aij = 0, і якщо - ні, то aij = 1.\r\nНеобхідно визначити мінімальну кількість клітин для безпечного розміщення тварин, коли у всіх клітинах знаходяться лише сумісні між собою тварини.\r\nПри цьому в клітині може бути одна, дві і більше тварин.\r\n",
                InputDescription = "Перший рядок вхідного файлу INPUT.TXT містить число N - кількість тварин (1 ≤ N ≤ 18).\r\nДалі йде N рядків по N чисел у кожному – матриця сумісності тварин.\r\n",
                OutputDescription = "У вихідний файл OUTPUT.TXT виведіть одне ціле число – мінімальна кількість клітин, яка потрібна для безпечного розміщення тварин.",
                TestCases = new List<TestCase>
            {
                new TestCase
                {
                    Input = "5\r\n0 1 1 1 1\r\n1 0 1 1 1 \r\n1 1 0 1 1\r\n1 1 1 0 1\r\n1 1 1 1 0",
                    Output = "5"
                },
                new TestCase 
                { Input = "1\r\n0", Output = "1" }
            }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLab(int labNumber, IFormFile inputFile)
        {
            if (inputFile == null || inputFile.Length == 0)
                return BadRequest("Please upload a file");

            // Read file contents into a string array
            string[] lines;
            using (var reader = new StreamReader(inputFile.OpenReadStream()))
            {
                var fileContent = await reader.ReadToEndAsync();
                lines = fileContent.Split(Environment.NewLine); // Split into lines
            }

            // Variable to store the processed result
            string output = null;

            // Execute the lab processing method based on lab number
            switch (labNumber)
            {
                case 1:
                    LAB1.Program.ValidateInput(lines);                    
                    output = LAB1.Program.SolveProblem(lines[0]);                    
                    break;
                case 2:
                    LAB2.Program.ValidateInput(lines);
                    string line = lines[0].Trim();                    
                    int result2 = LAB2.Program.SolveProblem(line);
                    output = result2.ToString();
                    break;
                case 3:
                    LAB3.Program.ValidateInput(lines);
                    var (N, compatibilityMatrix) = LAB3.Program.ParseInput(lines);
                    int result3 = LAB3.Program.SolveProblem(N, compatibilityMatrix);
                    output = result3.ToString();
                    break;
                default:
                    return BadRequest("Invalid lab number");
            }

            // Return result as JSON
            var result = new { output = output };
            return Json(result);
        }

    }
}
