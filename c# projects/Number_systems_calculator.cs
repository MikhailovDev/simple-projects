using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Test_calculator
{
    internal class Program
    {
        // Задача: составить калькулятор, который будет переводить числа
        // из одной системы счисления (СС) в другую. Допустимые СС 2, 8, 10, 16.
        static void MistakeMessage(int systemNumber)
        {
            if (systemNumber != 16)
            {
                Console.WriteLine($"\n======= Ошибка: В {systemNumber}-ной СС должны быть цифры от 0 до {systemNumber - 1}. =======\n");
            }else
            {
                Console.WriteLine($"\n======= Ошибка: В {systemNumber}-ной СС должны быть цифры от 0 до 9 и буквы от A до F. =======\n");
            }
        }
        static bool CheckingNumber(string startNumber, int systemNumber)
        {
                bool flag = true;
                int lenOfNumber = startNumber.Length;
                switch (systemNumber)
                {
                    case 2:
                    case 8:
                    case 10:
                        if (systemNumber < 11)
                        {
                            try
                            {
                                for (; lenOfNumber > 0; lenOfNumber--)
                                {
                                    if (int.Parse(startNumber.Substring(lenOfNumber - 1, 1)) > systemNumber - 1)
                                    {
                                        flag = false; 
                                        break;
                                    }
                                }
                            }
                            catch (FormatException)
                            {
                                flag = false;
                                break;
                            }
                        }
                        break;
                    case 16:
                        for (; lenOfNumber > 0; lenOfNumber--)
                        {
                            if (startNumber.Substring(lenOfNumber - 1, 1) != "A" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "a" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "B" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "b" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "C" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "c" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "D" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "d" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "E" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "e" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "F" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "f" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "0" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "1" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "2" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "3" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "4" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "5" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "6" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "7" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "8" &&
                                startNumber.Substring(lenOfNumber - 1, 1) != "9")
                            {
                                flag = false;
                                break;
                            }
                        }
                        break;
                }
            return flag;
        }
        static string OctToBin(string startNumber)
        {
            string newNumber = default;
            int lenOfNumber = startNumber.Length;
            for (; lenOfNumber > 0; lenOfNumber--)
            {
                int remainder = int.Parse(startNumber.Substring(lenOfNumber - 1, 1));

                switch (remainder)
                {
                    case 0:
                        newNumber = lenOfNumber - 1 != 0 ? "000" + newNumber : newNumber;
                        break;
                    case 1:
                        newNumber = lenOfNumber - 1 != 0 ? "001" + newNumber : "1" + newNumber;
                        break;
                    case 2:
                        newNumber = lenOfNumber - 1 != 0 ? "010" + newNumber : "10" + newNumber;
                        break;
                    case 3:
                        newNumber = lenOfNumber - 1 != 0 ? "011" + newNumber : "11" + newNumber;
                        break;
                    case 4:
                        newNumber = "100" + newNumber;
                        break;
                    case 5:
                        newNumber = "101" + newNumber;
                        break;
                    case 6:
                        newNumber = "110" + newNumber;
                        break;
                    case 7:
                        newNumber = "111" + newNumber;
                        break;
                }
            }

            return newNumber;
        }
        static string HexToBin(string startNumber)
        {
            string newNumber = default;
            int lenOfNumber = startNumber.Length;
            for (; lenOfNumber > 0; lenOfNumber--)
            {
                string remainder = startNumber.Substring(lenOfNumber - 1, 1);

                switch (remainder)
                {
                    case "0":
                        newNumber = "0000" + newNumber;
                        break;
                    case "1":
                        newNumber = lenOfNumber - 1 != 0 ? "0001" + newNumber : "1" + newNumber;
                        break;
                    case "2":
                        newNumber = lenOfNumber - 1 != 0 ? "0010" + newNumber : "10" + newNumber;
                        break;
                    case "3":
                        newNumber = lenOfNumber - 1 != 0 ? "0011" + newNumber : "11" + newNumber;
                        break;
                    case "4":
                        newNumber = lenOfNumber - 1 != 0 ? "0100" + newNumber : "100" + newNumber;
                        break;
                    case "5":
                        newNumber = lenOfNumber - 1 != 0 ? "0101" + newNumber : "101" + newNumber;
                        break;
                    case "6":
                        newNumber = lenOfNumber - 1 != 0 ? "0110" + newNumber : "110" + newNumber;
                        break;
                    case "7":
                        newNumber = lenOfNumber - 1 != 0 ? "0111" + newNumber : "111" + newNumber;
                        break;
                    case "8":
                        newNumber = "1000" + newNumber;
                        break;
                    case "9":
                        newNumber = "1001" + newNumber;
                        break;
                    case "A": 
                    case "a": 
                        newNumber = "1010" + newNumber;
                        break;
                    case "B":
                    case "b":
                        newNumber = "1011" + newNumber;
                        break;
                    case "C":
                    case "c":
                        newNumber = "1100" + newNumber;
                        break;
                    case "D":
                    case "d":
                        newNumber = "1101" + newNumber;
                        break;
                    case "E":
                    case "e":
                        newNumber = "1110" + newNumber;
                        break;
                    case "F":
                    case "f":
                        newNumber = "1111" + newNumber;
                        break;
                }
            }

            return newNumber;
        }
        static string BinToOct(string startNumber)
        {
            int count = default,
                sumFirstThreeLeters = default,
                lenOfNumber = startNumber.Length;
            string newNumber = default,
                   shortNumber;
            if (startNumber.Substring(0, 3) == "000")
            {
                startNumber = startNumber.Substring(3, lenOfNumber - 3);
                lenOfNumber -= 3;
            }
            else
            {
                if (startNumber.Substring(0, 2) == "00")
                {
                    startNumber = startNumber.Substring(2, lenOfNumber - 2);
                    lenOfNumber -= 2;
                }
                else
                {
                    if (startNumber.Substring(0, 1) == "0")
                    {
                        startNumber = startNumber.Substring(1, lenOfNumber - 1);
                        lenOfNumber--;
                    }
                }
            }
            for (; lenOfNumber > 0; lenOfNumber -= 3)
            {
                shortNumber = lenOfNumber > 3 ?
                    startNumber.Substring(lenOfNumber - 3, 3) :
                    startNumber.Substring(0, lenOfNumber);
                int shortLenOfNumber = shortNumber.Length;

                for (; shortLenOfNumber > 0; shortLenOfNumber--, count++)
                {
                    sumFirstThreeLeters += int.Parse(shortNumber.Substring(shortLenOfNumber - 1, 1)) * (int)Math.Pow(2, count);
                }
                newNumber = sumFirstThreeLeters + newNumber;

                count = default;
                sumFirstThreeLeters = default;
            }
            return newNumber;
        }
        static string BinToHex(string startNumber)
        {
            int count = default,
                sumFirstFourLeters = default,
                lenOfNumber = startNumber.Length;
            string newNumber = default,
                shortNumber;
            for (; lenOfNumber > 0; lenOfNumber -= 4)
            {
                shortNumber = lenOfNumber > 4 ?
                    startNumber.Substring(lenOfNumber - 4, 4) :
                    startNumber.Substring(0, lenOfNumber);
                int shortLenOfNumber = shortNumber.Length;
                for (; shortLenOfNumber > 0; shortLenOfNumber--, count++)
                {
                    sumFirstFourLeters += int.Parse(shortNumber.Substring(shortLenOfNumber - 1, 1)) * (int)Math.Pow(2, count);
                }
                if (sumFirstFourLeters > 9)
                {
                    newNumber = SymbolOfHexNumber(sumFirstFourLeters) + newNumber;
                }
                else
                {
                    newNumber = sumFirstFourLeters + newNumber;
                }
                count = default;
                sumFirstFourLeters = default;
            }
            return newNumber;
        }
        static string NumberToDecimal(string startNumber, int systemNumber)
        {
            ulong sumOfDigits = default;
            int count = default,
                lenOfNumber = startNumber.Length;
            for (; lenOfNumber > 0; lenOfNumber--, count++)
            {
                if (startNumber.Substring(lenOfNumber - 1, 1) == "A" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "a" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "B" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "b" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "C" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "c" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "D" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "d" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "E" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "e" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "F" ||
                startNumber.Substring(lenOfNumber - 1, 1) == "f")
                {
                    sumOfDigits += HexSymbolInNumber(startNumber.Substring(lenOfNumber - 1, 1)) * (ulong)Math.Pow(systemNumber, count);
                }
                else
                {
                    sumOfDigits += ulong.Parse(startNumber.Substring(lenOfNumber - 1, 1)) * (ulong)Math.Pow(systemNumber, count);
                }
            }

            return Convert.ToString(sumOfDigits);
        }
        static string SymbolOfHexNumber(int symbol)
        {
            string symbolInString = default;
            switch (symbol)
            {
                case 10:
                    symbolInString = "A";
                    break;
                case 11:
                    symbolInString = "B";
                    break;
                case 12:
                    symbolInString = "C";
                    break;
                case 13:
                    symbolInString = "D";
                    break;
                case 14:
                    symbolInString = "E";
                    break;
                case 15:
                    symbolInString = "F";
                    break;
            }
            return symbolInString;
        }
        static ulong HexSymbolInNumber(string symbol)
        {
            ulong symbolInString = default;
            switch (symbol)
            {
                case "A":
                case "a":
                    symbolInString = 10;
                    break;
                case "B":
                case "b":
                    symbolInString = 11;
                    break;
                case "C":
                case "c":
                    symbolInString = 12;
                    break;
                case "D":
                case "d":
                    symbolInString = 13;
                    break;
                case "E":
                case "e":
                    symbolInString = 14;
                    break;
                case "F":
                case "f":
                    symbolInString = 15;
                    break;
            }
            return symbolInString;
        }
        static string ReversedNewNumber(string numberInString)
        {
            int lenOfStringNumber = numberInString.Length;
            string newNumberInStringReverse = default;
            while (lenOfStringNumber > 0)
            {
                newNumberInStringReverse += numberInString.Substring(lenOfStringNumber - 1, 1);
                numberInString = numberInString.Substring(0, lenOfStringNumber - 1);
                lenOfStringNumber--;
            }
            return newNumberInStringReverse;
        }
        static void ResultingNumber(int whichOfSystemNumber, string outputNumber, bool numberIsSigned)
        {
            outputNumber = numberIsSigned ? "-" + outputNumber : outputNumber;
            Console.WriteLine($"\n======= Полученное число в {whichOfSystemNumber}-ной СС: {outputNumber} =======\n");
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"\nЕсли вы хотите перевести числа из одной системы счисления (CC) в другую, то нажмите любую клавишу" +
                $", a если выйти, то нажмите Esc после завершения перевода чисел.\n");
            string theEndOfProgramm = default;
            while (theEndOfProgramm != "Escape")
            {
                if (theEndOfProgramm != default)
                {
                    Console.WriteLine("Нажмите любую клавишу для очистки консоли.\n");
                    Console.ReadKey(true);
                    Console.Clear();
                    Console.WriteLine($"Нажмите Esc, если хотите закончить выполнение программы, " +
                        $"или любую другую клавишу для продолжения.\n");
                }
                theEndOfProgramm = Convert.ToString(Console.ReadKey(true).Key);
                if (theEndOfProgramm == "Escape")
                {
                    break;
                }
                Console.Write("Выберете СС, допустимые: 2, 8, 10, 16. \nВаш выбор: ");
                int systemNumber = default;
                try
                {
                    systemNumber = Convert.ToInt32(Console.ReadLine());
                }catch (FormatException)
                {
                    Console.WriteLine("\n======= Вы не выбрали CC или ввели неверные символы! =======\n");
                    continue;
                }
                int whichOfSystemNumber = default;
                string number = default;
                switch (systemNumber)
                {
                    case 2:
                    case 8:
                    case 10:
                    case 16:
                        Console.WriteLine($"Вы выбрали {systemNumber}-ную СС. \n");
                        Console.Write("Выберете СС, в которую хотите перевести, допустимые: 2, 8, 10, 16. \nВаш выбор: ");
                        try
                        {
                            whichOfSystemNumber = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\n======= Вы не выбрали CC или ввели неверные символы! =======\n");
                            continue;
                        }
                        switch (whichOfSystemNumber)
                        {
                            case 2:
                            case 8:
                            case 10:
                            case 16:
                                Console.WriteLine($"Переводим в {whichOfSystemNumber}-ную СС.\n");
                                Console.Write($"Напишите число, которое хотите перевести из {systemNumber}-ной СС в {whichOfSystemNumber}-ную.\nВаш выбор: ");

                                number = Console.ReadLine();

                                break;
                            default:
                                Console.WriteLine($"\n======= Вы выбрали неправильную CC, которая" +
                                    $" не входит в допустимые значения! =======\n");
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine($"\n======= Вы выбрали неправильную CC, которая" +
                            $" не входит в допустимые значения! =======\n");
                        break;
                }
                if (number == default)
                {
                    continue;
                }else
                {
                    if (number == "")
                    {
                        Console.WriteLine("\n======= Вы не ввели число! =======\n");
                        continue;
                    }
                }

                int lenOfStartNumber = number.Length;
                bool error = false;

                for (; lenOfStartNumber > 0; lenOfStartNumber--)
                {
                    string checkOfNumber = number.Substring(lenOfStartNumber - 1, 1);
                    if (checkOfNumber == " ")
                    {
                        Console.WriteLine("\nЧисло не должно содеражать пробелы!\n");
                        error = true;
                        continue;
                    }
                }
                if (error)
                {
                    continue;
                }

                bool numberIsSigned = false; 

                if (number.Substring(0, 1) == "-")
                {
                    number = number.Substring(1, number.Length - 1);
                    numberIsSigned = true;
                }
                if (!CheckingNumber(number, systemNumber))
                {
                    MistakeMessage(systemNumber);
                    continue;
                }
                switch (systemNumber)
                {
                    case 16:
                        switch (whichOfSystemNumber)
                        {
                            case 16:
                                ResultingNumber(whichOfSystemNumber, number, numberIsSigned);
                                break;
                            case 10:
                                ResultingNumber(whichOfSystemNumber, NumberToDecimal(number, systemNumber), numberIsSigned);
                                break;
                            case 8:
                                ResultingNumber(whichOfSystemNumber, BinToOct(HexToBin(number)), numberIsSigned);
                                break;
                            case 2:
                                ResultingNumber(whichOfSystemNumber, HexToBin(number), numberIsSigned);
                                break;
                        }
                        break;
                    case 10:
                        ulong numberInUlong = ulong.Parse(number);
                        string newNumber = default;
                        switch (whichOfSystemNumber)
                        {
                            case 16:
                                for (; numberInUlong > 0; numberInUlong /= (ulong)whichOfSystemNumber)
                                {
                                    byte remainder = Convert.ToByte(numberInUlong % (ulong)whichOfSystemNumber);
                                    if (remainder > 9)
                                    {
                                        newNumber = SymbolOfHexNumber(remainder) + newNumber;
                                    }
                                    else
                                    {
                                        newNumber = Convert.ToString(remainder) + newNumber;
                                    }
                                }
                                ResultingNumber(whichOfSystemNumber, newNumber, numberIsSigned);
                                break;
                            case 10:
                                ResultingNumber(whichOfSystemNumber, number, numberIsSigned);
                                break;
                            case 8:
                            case 2:
                                int count = default,
                                    lenOfNumber = number.Length;
                                ulong remainderOfNumber;
                                for (; numberInUlong > 0; lenOfNumber--, count++, numberInUlong /= (ulong)whichOfSystemNumber)
                                {
                                    remainderOfNumber = numberInUlong % (ulong)whichOfSystemNumber;
                                    newNumber = remainderOfNumber + newNumber;
                                }
                                ResultingNumber(whichOfSystemNumber, newNumber, numberIsSigned);
                                break;
                        }
                        break;
                    case 8: 
                        switch (whichOfSystemNumber)
                        {
                            case 16:
                                ResultingNumber(whichOfSystemNumber, BinToHex(OctToBin(number)), numberIsSigned);
                                break;
                            case 10:
                                ResultingNumber(whichOfSystemNumber, NumberToDecimal(number, systemNumber), numberIsSigned);
                                break;
                            case 8:
                                ResultingNumber(whichOfSystemNumber, number, numberIsSigned);
                                break;
                            case 2:
                                ResultingNumber(whichOfSystemNumber, OctToBin(number), numberIsSigned);
                                break;
                        }
                        break;
                    case 2:
                        switch (whichOfSystemNumber)
                        {
                            case 16:
                                ResultingNumber(whichOfSystemNumber, BinToHex(number), numberIsSigned);
                                break;
                            case 10:
                                ResultingNumber(whichOfSystemNumber, NumberToDecimal(number, systemNumber), numberIsSigned);
                                break;
                            case 8:
                                ResultingNumber(whichOfSystemNumber, BinToOct(number), numberIsSigned);
                                break;
                            case 2:
                                ResultingNumber(whichOfSystemNumber, number, numberIsSigned);
                                break;
                        }
                        break;
                }
            }
        } 
    }
}

