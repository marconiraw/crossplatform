using System;

namespace Lab2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Шляхи до файлів input.txt та output.txt
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
            string inputFile = Path.Combine(projectDirectory, "input.txt");
            string outputFile = Path.Combine(projectDirectory, "output.txt");

            // Створення об'єктів класів
            var fileHandler = new FileHandler();
            var coinCalculator = new CoinCalculator();

            try
            {
                // Читання та валідація вхідних даних
                var (N, coins, K) = fileHandler.ReadInput(inputFile);

                // Обчислення мінімальної кількості монет
                int result = coinCalculator.GetMinCoins(coins, K);

                // Запис результату у вихідний файл
                fileHandler.WriteOutput(outputFile, result);

                Console.WriteLine("Lab 2 executed successfully.");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                fileHandler.WriteOutput(outputFile, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                fileHandler.WriteOutput(outputFile, -1);
            }
        }
    }
}
