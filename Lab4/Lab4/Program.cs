using System;
using System.IO;
using System.Text;
using Library;
using McMaster.Extensions.CommandLineUtils;

namespace Lab4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string DefaultInput = "input.txt";
            const string DefaultOutput = "output.txt";

            Console.OutputEncoding = Encoding.UTF8;

            var commandApp = new CommandLineApplication
            {
                Name = "M_Kostiuk",
                Description = "Cross-platform Laboratory Application - Version 4",
            };

            commandApp.HelpOption("-h|--help");

            commandApp.Command("version", versionCmd =>
            {
                versionCmd.Description = "Display application information";
                versionCmd.OnExecute(() =>
                {
                    Console.WriteLine("Author: Marko_Kostiuk, Group: IPZ-34ms");
                    Console.WriteLine("App Version: 1.0.0");
                    return 0;
                });
            });

            commandApp.Command("set-path", setPath =>
            {
                setPath.Description = "Configure the directory path for input and output files";
                var pathOption = setPath.Option("-p|--path", "Directory path", CommandOptionType.SingleValue)
                                       .IsRequired();

                setPath.OnExecute(() =>
                {
                    string folderPath = pathOption.Value();

                    // Проверка, если путь относительный, объединяем с профилем пользователя
                    if (!Path.IsPathRooted(folderPath))
                    {
                        string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        folderPath = Path.Combine(userProfilePath, folderPath);
                    }

                    // Создаем директорию, если она не существует
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                        Console.WriteLine($"Directory '{folderPath}' has been created.");
                    }
                    else
                    {
                        Console.WriteLine($"Directory '{folderPath}' already exists.");
                    }

                    Environment.SetEnvironmentVariable("LAB_PATH", folderPath, EnvironmentVariableTarget.User);
                    Console.WriteLine($"LAB_PATH has been set to: {folderPath}");
                    return 0;
                });
            });

            commandApp.Command("run", runCmd =>
            {
                runCmd.Description = "Execute specified laboratory task";
                runCmd.OnExecute(() =>
                {
                    Console.WriteLine("Please specify a laboratory task to run.");
                    runCmd.ShowHelp();
                    return 0;
                });

                var inputOption = runCmd.Option("-i|--input", "Path to input file", CommandOptionType.SingleValue, true);
                var outputOption = runCmd.Option("-o|--output", "Path to output file", CommandOptionType.SingleValue, true);

                runCmd.Command("lab1", lab1Cmd =>
                {
                    lab1Cmd.Description = "Execute Lab 1";
                    lab1Cmd.OnExecute(() =>
                    {
                        string dirPath = Environment.GetEnvironmentVariable("LAB_PATH") ??
                                         Path.Combine(
                                             Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                             "source",
                                             "repos",
                                             "marconiraw",
                                             "crossplatform",
                                             "Lab1"
                                         );

                        string inputPath = inputOption.HasValue() ? inputOption.Value() : Path.Combine(dirPath, DefaultInput);
                        string outputPath = outputOption.HasValue() ? outputOption.Value() : Path.Combine(dirPath, DefaultOutput);

                        Lab1.Run(inputPath, outputPath);
                        Console.WriteLine("Lab 1 executed successfully.");
                        return 0;
                    });
                });

                runCmd.Command("lab2", lab2Cmd =>
                {
                    lab2Cmd.Description = "Execute Lab 2";
                    lab2Cmd.OnExecute(() =>
                    {
                        string dirPath = Environment.GetEnvironmentVariable("LAB_PATH") ??
                                         Path.Combine(
                                             Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                             "source",
                                             "repos",
                                             "marconiraw",
                                             "crossplatform",
                                             "Lab2"
                                         );

                        string inputPath = inputOption.HasValue() ? inputOption.Value() : Path.Combine(dirPath, DefaultInput);
                        string outputPath = outputOption.HasValue() ? outputOption.Value() : Path.Combine(dirPath, DefaultOutput);

                        Lab2.Run(inputPath, outputPath);
                        Console.WriteLine("Lab 2 executed successfully.");
                        return 0;
                    });
                });

                //runCmd.Command("lab3", lab3Cmd =>
                //{
                //    lab3Cmd.Description = "Execute Lab 3";
                //    lab3Cmd.OnExecute(() =>
                //    {
                //        string dirPath = Environment.GetEnvironmentVariable("LAB_PATH") ??
                //                         Path.Combine(
                //                             Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                //                             "source",
                //                             "repos",
                //                             "marconiraw",
                //                             "crossplatform",
                //                             "Lab3"
                //                         );

                //        string inputPath = inputOption.HasValue() ? inputOption.Value() : Path.Combine(dirPath, DefaultInput);
                //        string outputPath = outputOption.HasValue() ? outputOption.Value() : Path.Combine(dirPath, DefaultOutput);

                //        Lab3.Run(inputPath, outputPath);
                //        Console.WriteLine("Lab 3 executed successfully.");
                //        return 0;
                //    });
                //});
            });

            commandApp.OnExecute(() =>
            {
                Console.WriteLine("A subcommand is required to proceed.");
                commandApp.ShowHelp();
                return 0;
            });

            try
            {
                commandApp.Execute(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
