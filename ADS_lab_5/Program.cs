using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ADS_lab_5
{
    public class Program
    {        
        private const string DefaultRoutePlainTextFile = "D:\\University\\7_term\\Application and data security\\Labs\\Labs\\Lab_5\\TestFolder\\PlainText\\";
        private const string DefaultRouteSignFile = "D:\\University\\7_term\\Application and data security\\Labs\\Labs\\Lab_5\\TestFolder\\SignText\\";

        static void Main()
        {
            // Зчитати з файлу чи ввести з консолі
            string inputData = GetInitialData();

            byte[] data = Encoding.UTF8.GetBytes(inputData);

            using (DSACryptoServiceProvider dsaProvider = new DSACryptoServiceProvider())
            {
                byte[] signature = SignData(dsaProvider, data);

                // Переводимо в шістнадцятковий формат
                string hexSign = string.Join("", signature.Select(b => b.ToString("X2")));

                Console.WriteLine("Digital Sign: " + hexSign);

                // Зберегти у файл
                SaveSignIntoFile(hexSign);

                // Витягуємо наш підпис
                Console.WriteLine("\n\nEnter file name, in which sign!");
                string fileName = Console.ReadLine();
                byte[] byteSign = ReadBytesFromFile(fileName);


                bool isVerified = VerifySignature(dsaProvider, data, byteSign);

                Console.WriteLine("Sign Verification: " + isVerified);
            }
        }

        // Метод для створення цифрового підпису
        static byte[] SignData(DSACryptoServiceProvider dsaProvider, byte[] data)
        {
            return dsaProvider.SignData(data);
        }

        // Метод для перевірки цифрового підпису
        static bool VerifySignature(DSACryptoServiceProvider dsaProvider, byte[] data, byte[] signature)
        {
            return dsaProvider.VerifyData(data, signature);
        }

        static string GetInitialData()
        {
            string inputData = string.Empty;

            int userChoice = 0;
            do
            {
                Console.WriteLine("How you would like to enter data? \n1 - Enter via Console, \n2 - Enter via file");
                bool isInputValueIntager = int.TryParse(Console.ReadLine(), out userChoice);

                if (!isInputValueIntager)
                {
                    Console.WriteLine("You entered not intager value!");
                }
                else if (userChoice != 1 && userChoice != 2)
                {
                    Console.WriteLine("You entered incorrect number (only 1 or 2 are available)!");
                    userChoice = 0;
                }

            } while (userChoice != 1 && userChoice != 2);

            if (userChoice == 1)
            {
                inputData = Console.ReadLine();
            }
            else
            {
                Console.Write("Enter file name: ");
                string fileName = Console.ReadLine();

                inputData = ReadTextFromFile(fileName);
            }

            return inputData;
        }

        static void SaveSignIntoFile(string sign)
        {
            int userChoice = 0;
            do
            {
                Console.WriteLine("Would you like to save this Sign into the file? \n1 - Yes, \n2 - No");
                bool isInputValueIntager = int.TryParse(Console.ReadLine(), out userChoice);

                if (!isInputValueIntager)
                {
                    Console.WriteLine("You entered not intager value!");
                }
                else if (userChoice != 1 && userChoice != 2)
                {
                    Console.WriteLine("You entered incorrect number (only 1 or 2 are available)!");
                    userChoice = 0;
                }

            } while (userChoice != 1 && userChoice != 2);

            if (userChoice == 1)
            {
                Console.Write("Enter file name: ");
                WriteTextIntoFile(sign, Console.ReadLine());
            }
        }

        static string ReadTextFromFile(string fileName)
        {
            string fileData = string.Empty;

            string filePath = DefaultRoutePlainTextFile + fileName;

            try
            {
                fileData = File.ReadAllText(filePath);
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(filePath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(fileName);
            }

            return fileData;
        }
        
        static void WriteTextIntoFile(string sign, string fileName)
        {
            string filePath = DefaultRoutePlainTextFile + fileName;

            try
            {
                File.WriteAllText(filePath, sign);
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(filePath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(fileName);
            }
        }

        static byte[] ReadBytesFromFile(string fileName)
        {
            string fileData = string.Empty;

            string filePath = DefaultRoutePlainTextFile + fileName;

            try
            {
                fileData = File.ReadAllText(filePath);
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(filePath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(fileName);
            }


            // Перетворення підпису в байти
            byte[] byteSign = null;

            if (fileData != string.Empty)
            {
                byteSign = new byte[fileData.Length / 2];

                for (int i = 0; i < byteSign.Length; i++)
                {
                    byteSign[i] = Convert.ToByte(fileData.Substring(i * 2, 2), 16);
                }
            }

            return byteSign;
        }
}
}
