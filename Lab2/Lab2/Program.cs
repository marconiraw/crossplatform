using System;
using System.IO;

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

            try
            {
                // Чтение и валидация входных данных
                InputData inputData = FileHandler.ReadInput(inputFile);

                // Вычисление минимального количества монет
                int result = CoinCalculator.GetMinCoins(inputData.Coins, inputData.K);

                // Запись результата в выходной файл
                FileHandler.WriteOutput(outputFile, result);

                Console.WriteLine("Lab 2 executed successfully.");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                FileHandler.WriteOutput(outputFile, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                FileHandler.WriteOutput(outputFile, -1);
            }
        }
    }
}
