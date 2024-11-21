using System;

namespace Lab3
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Шляхи до файлів input.txt та output.txt
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
            string inputFile = System.IO.Path.Combine(projectDirectory, "input.txt");
            string outputFile = System.IO.Path.Combine(projectDirectory, "output.txt");

            // Створення об'єктів класів
            var fileHandler = new FileHandler();
            var calculator = new ArborescenceCalculator();

            try
            {
                // Читання та валідація вхідних даних
                var (n, edges, root) = fileHandler.ReadInput(inputFile);

                // Обчислення мінімальної арбореції
                var result = calculator.GetMinimumArborescence(n, edges, root);

                // Запис результату у вихідний файл
                fileHandler.WriteOutput(outputFile, result);

                if (result != null)
                {
                    Console.WriteLine("Lab 3 executed successfully.");
                }
                else
                {
                    Console.WriteLine("Lab 3 executed with no valid arborescence found.");
                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                fileHandler.WriteOutput(outputFile, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                fileHandler.WriteOutput(outputFile, null);
            }
        }
    }
}
