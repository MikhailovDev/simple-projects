using System;

namespace Codewars
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random randomNumber = new Random();

            int amountOfRows = default,
                amountOfColumns = default;

            Console.Write("Введите количество рядов поля: ");
            try
            {
                amountOfRows = int.Parse(Console.ReadLine());
                Console.Write("Введите количество столбцов поля: ");
                try
                {
                    amountOfColumns = int.Parse(Console.ReadLine());
                    Console.WriteLine("\nДля прохождения данной игры необходимо заполнить все поле нулями.\nУдачной игры!\n");

                    string[,] field = new string[amountOfRows, amountOfColumns];

                    for (int i = 0; i < field.GetLength(0); i++) // Заполнение массива x-ми
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            field[i, j] = "x";
                            if (i == amountOfRows - 1 && j == amountOfColumns - 1)
                                field[randomNumber.Next(amountOfRows), randomNumber.Next(amountOfColumns)] = "V";
                        }
                    }

                    int positionY = default,
                        positionX = default;

                    for (int i = 0; i < field.GetLength(0); i++) // Нахождение индексов стартового символа по осям X и Y 
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            if (field[i, j] == "V")
                            {
                                positionY = i;
                                positionX = j;
                            }
                        }
                    }

                    string pressedKey = default,
                        nextSymbol = "x";
                    int score;

                    while (pressedKey != "Escape")
                    {
                        score = 1;
                        for (int i = 0; i < field.GetLength(0); i++) // Score
                        {
                            for (int j = 0; j < field.GetLength(1); j++)
                            {
                                score = field[i, j] == "o" ? score + 1 : score;
                            }
                        }

                        Console.WriteLine($"positionX = {positionX}\npositionY = {positionY}");
                        Console.WriteLine($"\n\tScore: {score} ");
                        Console.WriteLine($"\tLeft to fill: {amountOfColumns * amountOfRows - score}\n");

                        for (int i = 0; i < field.GetLength(0); i++) // Вывод поля на консоль
                        {
                            for (int j = 0; j < field.GetLength(1); j++)
                            {
                                if (j == 0)
                                    Console.Write($"\t{field[i, j]} ");
                                else
                                    Console.Write($"{field[i, j]} ");
                            }
                            Console.WriteLine();
                        }

                        Console.WriteLine("\nНажмите Esc, чтобы завершить игру.");

                        pressedKey = Convert.ToString(Console.ReadKey(true).Key);
                        switch (pressedKey)
                        {
                            case "DownArrow":
                                try
                                {
                                    field[positionY, positionX] = nextSymbol == "x" ? "o" : "x";
                                    nextSymbol = field[positionY + 1, positionX];
                                    field[positionY + 1, positionX] = "v";
                                    positionY++;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    positionY = 0;
                                    nextSymbol = field[positionY, positionX];
                                    field[positionY, positionX] = "v";
                                }
                                break;
                            case "RightArrow":
                                try
                                {
                                    field[positionY, positionX] = nextSymbol == "x" ? "o" : "x";
                                    nextSymbol = field[positionY, positionX + 1];
                                    field[positionY, positionX + 1] = ">";
                                    positionX++;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    positionX = 0;
                                    nextSymbol = field[positionY, positionX];
                                    field[positionY, positionX] = ">";
                                }
                                break;
                            case "UpArrow":
                                try
                                {
                                    field[positionY, positionX] = nextSymbol == "x" ? "o" : "x";
                                    nextSymbol = field[positionY - 1, positionX];
                                    field[positionY - 1, positionX] = "^";
                                    positionY--;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    positionY = amountOfRows - 1;
                                    nextSymbol = field[positionY, positionX];
                                    field[positionY, positionX] = "^";
                                }
                                break;
                            case "LeftArrow":
                                try
                                {
                                    field[positionY, positionX] = nextSymbol == "x" ? "o" : "x";
                                    nextSymbol = field[positionY, positionX - 1];
                                    field[positionY, positionX - 1] = "<";
                                    positionX--;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    positionX = amountOfColumns - 1;
                                    nextSymbol = field[positionY, positionX];
                                    field[positionY, positionX] = "<";
                                }
                                break;
                            default:
                                field[positionY, positionX] = "*";
                                break;
                        }
                        if (score == amountOfRows * amountOfColumns)
                        {
                            Console.Clear();
                            Console.WriteLine("\n====== Ура! У вас получилось заполнить все поле нулями! =) ======\n");
                            break;
                        }
                        Console.Clear();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n====== Упс, кажется вы пропустили это поле или ввели неверные входные данные... ======\n");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n====== Упс, кажется вы пропустили это поле или ввели неверные входные данные... ======\n");
            }
        }
    }
}
