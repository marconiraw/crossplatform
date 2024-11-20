using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using lab3;

namespace lab3.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestCase1()
        {
            string input = "2 2\n1 2 3\n1 2 4";
            string expectedOutput = "3 1\n1";

            RunTest(input, expectedOutput);
        }

        [TestMethod]
        public void TestCase2()
        {
            string input = "3 3\n1 2 5\n1 3 10\n3 2 4";
            string expectedOutput = "14 2\n2 3";

            RunTest(input, expectedOutput);
        }

        [TestMethod]
        public void TestCase3()
        {
            string input = "4 4\n1 2 5\n2 3 5\n3 4 5\n4 2 5";
            string expectedOutput = "15 3\n1 2 3";

            RunTest(input, expectedOutput);
        }

        [TestMethod]
        public void TestCase4()
        {
            string input = "3 2\n1 2 0\n2 3 0";
            string expectedOutput = "0 2\n1 2";

            RunTest(input, expectedOutput);
        }

        [TestMethod]
        public void TestCase5()
        {
            string input = "5 5\n1 2 1\n2 3 2\n3 4 3\n4 5 4\n5 1 5";
            string expectedOutput = "10 4\n1 2 3 4";

            RunTest(input, expectedOutput);
        }

        private void RunTest(string input, string expectedOutput)
        {
            // Путь к директории lab3
            string lab3Directory = Path.Combine("..", "..", "lab3");

            // Запись input.txt в директорию lab3
            File.WriteAllText(Path.Combine(lab3Directory, "input.txt"), input);

            // Запуск основного приложения
            Program.Main(new string[0]);

            // Чтение output.txt из директории lab3
            string output = File.ReadAllText(Path.Combine(lab3Directory, "output.txt")).Trim();

            Assert.AreEqual(expectedOutput, output);
        }
    }
}
