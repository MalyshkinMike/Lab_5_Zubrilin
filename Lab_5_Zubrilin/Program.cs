using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;


namespace Lab_5_Zubrilin
{
    internal class Program
    {
        const int SIDE_FIELD = 10;
        private const int NUMBER_OF_SINGLE_DECK = 4;
        private const int NUMBER_OF_DOUBLE_DECK = 3;
        private const int NUMBER_OF_TRIPLE_DECK = 2;
        private const int NUMBER_OF_FOUR_DECK = 1;
        private const int SINGLE_DECK_SHIP = 1;
        private const int DOUBLE_DECK_SHIP = 2;
        private const int TRIPLE_DECK_SHIP = 3;
        private const int FOUR_DECK_SHIP = 4;
        const int FIELD_SIZE = 100;

        static int[] RandomArray(int[] a, int n)
        {
            // Создание экземпляра класса Random
            Random rnd = new Random();
            // Цикл, проходящий по всем элементам массива
            for (int i = 0; i < n; i++)
                // Присваивание i-ому элементу массива случайного значения
                a[i] = rnd.Next(-10, 10);
            return a;
        }

        static void OutputArray(int[] a)
        {
            foreach (int i in a)
                Console.Write(i + " ");
            Console.WriteLine();
        }

        static int[] BubbleSort(int[] a)
        {
            int t;
            for (int i = 0; i < a.Length; i++)
            for (int j = 0; j < a.Length - i - 1; j++)
                if (a[j] > a[j + 1])
                {
                    t = a[j + 1];
                    a[j + 1] = a[j];
                    a[j] = t;
                }

            return a;
        }

        static int[] SelectionSort(int[] a)
        {
            int t, min;
            for (int i = 0; i < a.Length - 1; i++)
            {
                min = a[i];
                for (int j = i + 1; j < a.Length; j++)
                    if (a[j] < min)
                        min = a[j];
                t = min;
                min = a[i];
                a[i] = t;
            }

            return a;
        }

        static int[,] CreateEmptyField()
        {
            int[,] field = new int [SIDE_FIELD, SIDE_FIELD];
            for (int i = 0; i < SIDE_FIELD; i++)
            for (int j = 0; j < SIDE_FIELD; j++)
            {
                field[i, j] = 0;
            }

            return field;
        }

        static int[] GenerateSingleDeckCoords(ref int[] available)
        {
            Random rnd = new Random();
            int pos = rnd.Next(0, available.Length);
            int coords = available[pos];

            bool needToDel(int n)
            {
                bool i = n != coords;
                bool up = coords > 9;
                bool left = coords % 10 != 0;
                bool down = coords < 90;
                bool right = coords % 10 != 9;
                if (up)
                    i = i && n != coords - 10;
                if (left)
                    i = i && n != coords - 1;
                if (right)
                    i = i && n != coords + 1;
                if (down)
                    i = i && n != coords + 10;
                if (left && up)
                    i = i && n != coords - 11;
                if (left && down)
                    i = i && n != coords + 9;
                if (right && up)
                    i = i && n != coords - 9;
                if (right && down)
                    i = i && n != coords + 11;
                return i;
            }

            available = Array.FindAll(available, needToDel).ToArray();
            return new int[] {coords / 10, coords % 10};
        }

        static int[,] GenerateDoubleDeckCoords(ref int[] available)
        {
            Random rnd = new Random();
            int possibleWays = 0;
            int[] possibleCoords = new int[5];
            int dot;
            do
            {
                int pos = rnd.Next(0, available.Length);
                possibleCoords[0] = available[pos];
                dot = possibleCoords[0];
                int temp = 1;
                if (Array.Exists(available, value => value == dot - 1) && dot % 10 > 0)
                {
                    possibleCoords[temp] = dot - 1;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot - 10))
                {
                    possibleCoords[temp] = dot - 10;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 1) && dot % 10 < 9)
                {
                    possibleCoords[temp] = dot + 1;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 10))
                {
                    possibleCoords[temp] = dot + 10;
                    possibleWays++;
                    temp++;
                }
            } while (possibleWays == 0);

            int way = rnd.Next(1, possibleWays + 1);
            possibleCoords = possibleCoords.Where(value => value != -1).ToArray();
            int[] coords = {dot, possibleCoords[way]};

            bool needToDel(int n)
            {
                bool i = n != coords[0] && n != coords[1];
                bool up = coords[0] > 9 && coords[1] > 9;
                bool left = coords[0] % 10 != 0 && coords[1] % 10 != 0;
                bool down = coords[0] < 90 && coords[1] < 90;
                bool right = coords[0] % 10 != 9 && coords[1] % 10 != 9;
                if (up)
                    i = i && n != coords[0] - 10 && n != coords[1] - 10;
                if (left)
                    i = i && n != coords[0] - 1 && n != coords[1] - 1;
                if (right)
                    i = i && n != coords[0] + 1 && n != coords[1] + 1;
                if (down)
                    i = i && n != coords[0] + 10 && n != coords[1] + 10;
                if (left && up)
                    i = i && n != coords[0] - 11 && n != coords[1] - 11;
                if (left && down)
                    i = i && n != coords[0] + 9 && n != coords[1] + 9;
                if (right && up)
                    i = i && n != coords[0] - 9 && n != coords[1] - 9;
                if (right && down)
                    i = i && n != coords[0] + 11 && n != coords[1] + 11;
                return i;
            }

            available = Array.FindAll(available, needToDel).ToArray();
            return new int[,] {{coords[0] / 10, coords[0] % 10}, {coords[1] / 10, coords[1] % 10}};
        }

        static int[,] GenerateTripleDeckCoords(ref int[] available)
        {
            Random rnd = new Random();
            int possibleWays = 0;
            int dot;
            int[,] possibleCoords = new int[5, 2];
            do
            {
                int pos = rnd.Next(0, available.Length);
                possibleCoords[0, 0] = available[pos];
                dot = possibleCoords[0, 0];
                int temp = 1;
                if (Array.Exists(available, value => value == dot - 1) &&
                    Array.Exists(available, value => value == dot - 2) && dot % 10 > 1)
                {
                    possibleCoords[temp, 0] = dot - 1;
                    possibleCoords[temp, 1] = dot - 2;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot - 10) &&
                    Array.Exists(available, value => value == dot - 20))
                {
                    possibleCoords[temp, 0] = dot - 10;
                    possibleCoords[temp, 1] = dot - 20;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 1) &&
                    Array.Exists(available, value => value == dot + 2) && dot % 10 < 8)
                {
                    possibleCoords[temp, 0] = dot + 1;
                    possibleCoords[temp, 1] = dot + 2;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 10) &&
                    Array.Exists(available, value => value == dot + 20))
                {
                    possibleCoords[temp, 0] = dot + 10;
                    possibleCoords[temp, 1] = dot + 20;
                    possibleWays++;
                    temp++;
                }
            } while (possibleWays == 0);

            int way = rnd.Next(1, possibleWays + 1);

            int[] coords = {dot, possibleCoords[way, 0], possibleCoords[way, 1]};

            bool needToDel(int n)
            {
                bool i = n != coords[0] && n != coords[1] && n != coords[2];
                bool up = coords[0] > 9 && coords[1] > 9 && coords[2] > 9;
                bool left = coords[0] % 10 != 0 && coords[1] % 10 != 0 && coords[2] % 10 != 0;
                bool down = coords[0] < 90 && coords[1] < 90 && coords[2] < 90;
                bool right = coords[0] % 10 != 9 && coords[1] % 10 != 9 && coords[2] % 10 != 9;
                if (up)
                    i = i && n != coords[0] - 10 && n != coords[1] - 10 && n != coords[2] - 10;
                if (left)
                    i = i && n != coords[0] - 1 && n != coords[1] - 1 && n != coords[2] - 1;
                if (right)
                    i = i && n != coords[0] + 1 && n != coords[1] + 1 && n != coords[2] + 1;
                if (down)
                    i = i && n != coords[0] + 10 && n != coords[1] + 10 && n != coords[2] + 10;
                if (left && up)
                    i = i && n != coords[0] - 11 && n != coords[1] - 11 && n != coords[2] - 11;
                if (left && down)
                    i = i && n != coords[0] + 9 && n != coords[1] + 9 && n != coords[2] + 9;
                if (right && up)
                    i = i && n != coords[0] - 9 && n != coords[1] - 9 && n != coords[2] - 9;
                if (right && down)
                    i = i && n != coords[0] + 11 && n != coords[1] + 11 && n != coords[2] + 11;
                return i;
            }

            available = Array.FindAll(available, needToDel).ToArray();
            return new int[,]
                {{coords[0] / 10, coords[0] % 10}, {coords[1] / 10, coords[1] % 10}, {coords[2] / 10, coords[2] % 10}};
        }

        static int[,] GenerateFourDeckCoords(ref int[] available)
        {
            Random rnd = new Random();
            int possibleWays = 0;
            int dot;
            int[,] possibleCoords = new int[5, 3];
            do
            {
                int pos = rnd.Next(0, available.Length);
                possibleCoords[0, 0] = available[pos];
                dot = possibleCoords[0, 0];
                int temp = 1;
                if (Array.Exists(available, value => value == dot - 1) &&
                    Array.Exists(available, value => value == dot - 2) &&
                    Array.Exists(available, value => value == dot - 3) && dot % 10 > 2)
                {
                    possibleCoords[temp, 0] = dot - 1;
                    possibleCoords[temp, 1] = dot - 2;
                    possibleCoords[temp, 2] = dot - 3;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot - 10) &&
                    Array.Exists(available, value => value == dot - 20) &&
                    Array.Exists(available, value => value == dot - 30))
                {
                    possibleCoords[temp, 0] = dot - 10;
                    possibleCoords[temp, 1] = dot - 20;
                    possibleCoords[temp, 2] = dot - 30;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 1) &&
                    Array.Exists(available, value => value == dot + 2) &&
                    Array.Exists(available, value => value == dot + 3) && dot % 10 < 7)
                {
                    possibleCoords[temp, 0] = dot + 1;
                    possibleCoords[temp, 1] = dot + 2;
                    possibleCoords[temp, 2] = dot + 3;
                    possibleWays++;
                    temp++;
                }

                if (Array.Exists(available, value => value == dot + 10) &&
                    Array.Exists(available, value => value == dot + 20) &&
                    Array.Exists(available, value => value == dot + 30))
                {
                    possibleCoords[temp, 0] = dot + 10;
                    possibleCoords[temp, 1] = dot + 20;
                    possibleCoords[temp, 2] = dot + 30;
                    possibleWays++;
                    temp++;
                }
            } while (possibleWays == 0);

            int way = rnd.Next(1, possibleWays + 1);
            int[] coords = {dot, possibleCoords[way, 0], possibleCoords[way, 1], possibleCoords[way, 2]};

            bool needToDel(int n)
            {
                bool toDel = n != coords[0] && n != coords[1] && n != coords[2] && n != coords[3];
                bool up = coords[0] > 9 && coords[1] > 9 && coords[2] > 9 && coords[3] > 9;
                bool left = coords[0] % 10 != 0 && coords[1] % 10 != 0 && coords[2] % 10 != 0 && coords[3] % 10 != 0;
                bool down = coords[0] < 90 && coords[1] < 90 && coords[2] < 90 && coords[3] < 90;
                bool right = coords[0] % 10 != 9 && coords[1] % 10 != 9 && coords[2] % 10 != 9 && coords[3] % 10 != 9;
                if (up)
                    toDel = toDel && n != coords[0] - 10 && n != coords[1] - 10 && n != coords[2] - 10 &&
                            n != coords[3] - 10;
                if (left)
                    toDel = toDel && n != coords[0] - 1 && n != coords[1] - 1 && n != coords[2] - 1 &&
                            n != coords[3] - 1;
                if (right)
                    toDel = toDel && n != coords[0] + 1 && n != coords[1] + 1 && n != coords[2] + 1 &&
                            n != coords[3] + 1;
                if (down)
                    toDel = toDel && n != coords[0] + 10 && n != coords[1] + 10 && n != coords[2] + 10 &&
                            n != coords[3] + 10;
                if (left && up)
                    toDel = toDel && n != coords[0] - 11 && n != coords[1] - 11 && n != coords[2] - 11 &&
                            n != coords[3] - 11;
                if (left && down)
                    toDel = toDel && n != coords[0] + 9 && n != coords[1] + 9 && n != coords[2] + 9 &&
                            n != coords[3] + 9;
                if (right && up)
                    toDel = toDel && n != coords[0] - 9 && n != coords[1] - 9 && n != coords[2] - 9 &&
                            n != coords[3] - 9;
                if (right && down)
                    toDel = toDel && n != coords[0] + 11 && n != coords[1] + 11 && n != coords[2] + 11 &&
                            n != coords[3] + 11;
                return toDel;
            }

            available = Array.FindAll(available, needToDel).ToArray();
            return new int[,]
            {
                {coords[0] / 10, coords[0] % 10}, {coords[1] / 10, coords[1] % 10}, {coords[2] / 10, coords[2] % 10},
                {coords[3] / 10, coords[3] % 10}
            };
        }

        static int[,] SingleDeckPlacementAlgorithm(int[,] field, ref int[] available)
        {
            for (int i = 0; i < NUMBER_OF_SINGLE_DECK; i++)
            {
                int[] singleDeckCoords = GenerateSingleDeckCoords(ref available);
                field[singleDeckCoords[0], singleDeckCoords[1]] = SINGLE_DECK_SHIP;
            }

            return field;
        }

        static int[,] DoubleDeckPlacementAlgorithm(int[,] field, ref int[] available)
        {
            for (int i = 0; i < NUMBER_OF_DOUBLE_DECK; i++)
            {
                int[,] doubleDeckCoords = GenerateDoubleDeckCoords(ref available);
                field[doubleDeckCoords[0, 0], doubleDeckCoords[0, 1]] = DOUBLE_DECK_SHIP;
                field[doubleDeckCoords[1, 0], doubleDeckCoords[1, 1]] = DOUBLE_DECK_SHIP;
            }

            return field;
        }

        static int[,] TripleDeckPlacementAlgorithm(int[,] field, ref int[] available)
        {
            for (int i = 0; i < NUMBER_OF_TRIPLE_DECK; i++)
            {
                int[,] tripleDeckCoords = GenerateTripleDeckCoords(ref available);
                field[tripleDeckCoords[0, 0], tripleDeckCoords[0, 1]] = TRIPLE_DECK_SHIP;
                field[tripleDeckCoords[1, 0], tripleDeckCoords[1, 1]] = TRIPLE_DECK_SHIP;
                field[tripleDeckCoords[2, 0], tripleDeckCoords[2, 1]] = TRIPLE_DECK_SHIP;
            }

            return field;
        }

        static int[,] FourDeckPlacementAlgorithm(int[,] field, ref int[] available)
        {
            for (int i = 0; i < NUMBER_OF_FOUR_DECK; i++)
            {
                int[,] fourDeckCoords = GenerateFourDeckCoords(ref available);
                field[fourDeckCoords[0, 0], fourDeckCoords[0, 1]] = FOUR_DECK_SHIP;
                field[fourDeckCoords[1, 0], fourDeckCoords[1, 1]] = FOUR_DECK_SHIP;
                field[fourDeckCoords[2, 0], fourDeckCoords[2, 1]] = FOUR_DECK_SHIP;
                field[fourDeckCoords[3, 0], fourDeckCoords[3, 1]] = FOUR_DECK_SHIP;
            }

            return field;
        }

        static int[,] ShipPlacementAlgorithm(int[,] field)
        {
            Random rnd = new Random();
            int[] available = new int[FIELD_SIZE];
            for (int i = 0; i < FIELD_SIZE; i++)
                available[i] = i;
            field = FourDeckPlacementAlgorithm(field, ref available);
            field = TripleDeckPlacementAlgorithm(field, ref available);
            field = DoubleDeckPlacementAlgorithm(field, ref available);
            field = SingleDeckPlacementAlgorithm(field, ref available);
            return field;
        }

        static void DisplayField(String[,] field)
        {
            for (int i = 0; i < SIDE_FIELD; ++i)
            {
                for (int j = 0; j < SIDE_FIELD; j++)
                {
                    Console.Write(field[i, j] + ' ');
                }

                Console.WriteLine();
            }
        }
        //вывод поля компьютера
        static void DisplayField(int[,] field)
        {
            for (int i = 0; i < SIDE_FIELD; ++i)
            {
                for (int j = 0; j < SIDE_FIELD; j++)
                {
                    Console.Write(field[i, j].ToString() + ' ');
                }

                Console.WriteLine();
            }
        }
        

        
        static void GameAlgorithm(int[,] field)
        {
            int[,] intField = new int[SIDE_FIELD, SIDE_FIELD];
            String[,] stringField = new string[SIDE_FIELD, SIDE_FIELD];
            for (int i = 0; i < SIDE_FIELD; ++i)
            for (int j = 0; j < SIDE_FIELD; ++j)
            {
                intField[i, j] = -1;
                stringField[i, j] = "_";
            }

            DisplayField(field);
            Console.WriteLine();
            Console.WriteLine();
            int deadShips = 0;
            int fourDeckCount = 0;
            while (deadShips != 10)
            {
                DisplayField(stringField);

                Console.WriteLine(
                    "Введите координаты, по которым хотите стрелять (сначала х, затем у).\n" +
                    "(0 - промах, 7 - неизвестно, 1..4 - подбитый/раненый корабль. Число указывает на размер корабля\n" +
                    " -2 для выхода");
                int x = -1, y = -1;
                while (!int.TryParse(Console.ReadLine(), out x) || !((x >= 0 && x < 10) || x == -2))
                {
                    Console.WriteLine("Введите х ");
                }

                if (x == -2)
                    return;
                while (!int.TryParse(Console.ReadLine(), out y) || y > 10)
                {
                    Console.WriteLine("Введите y ");
                }

                if (intField[x, y] == field[x, y])
                    continue;
                intField[x, y] = field[x, y];
                stringField[x, y] = field[x, y].ToString();
                switch (intField[x, y])
               
                 {
                    case SINGLE_DECK_SHIP:
                        deadShips++;
                        Console.WriteLine("Торпедный катер потоплен");
                        break;
                    case DOUBLE_DECK_SHIP:
                        if (x > 0)
                            if (intField[x - 1, y] == DOUBLE_DECK_SHIP)
                            {
                                Console.WriteLine("Крейсер потоплен");

                                deadShips++;
                            }

                        if (x < 9)
                            if (intField[x + 1, y] == DOUBLE_DECK_SHIP)
                            {Console.WriteLine("Крейсер потоплен");

                                deadShips++;
                            }

                        if (y < 9)
                            if (intField[x, y + 1] == DOUBLE_DECK_SHIP)
                            {
                                Console.WriteLine("Крейсер потоплен");
                                deadShips++;
                            }

                        if (y > 0)
                            if (intField[x, y - 1] == DOUBLE_DECK_SHIP)
                            {Console.WriteLine("Крейсер потоплен");

                                deadShips++;
                            }

                        break;
                    case TRIPLE_DECK_SHIP:
                        if (y > 1)
                            if (intField[x, y - 1] == 3 && intField[x, y - 2] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        if (y < 9 && y > 0)
                            if (intField[x, y - 1] == 3 && intField[x, y + 1] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        if (y < 8)
                            if (intField[x, y + 1] == 3 && intField[x, y + 2] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        if (x > 1)
                            if (intField[x - 1, y] == 3 && intField[x - 2, y] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        if (x < 8)
                            if (intField[x + 1, y] == 3 && intField[x + 2, y] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        if (x < 9 && x > 0)
                            if (intField[x - 1, y] == 3 && intField[x + 1, y] == 3)
                            {
                                Console.WriteLine("Эсминец потоплен");
                                deadShips++;
                            }

                        break;
                    case FOUR_DECK_SHIP:
                        ++fourDeckCount;
                        if (fourDeckCount == 4)
                        {
                            Console.WriteLine("Линкор потоплен");
                            deadShips++;
                        }
                        break;
                }
            }
            DisplayField(field);
        }

        static void Main(string[] args)
        {
            string exit = "н";
            while (exit.Equals("н") || exit.Equals("n")
            ) //пока переменная exit равна н или n, программа не будет закрываться
            {
                Console.WriteLine("1 - Угадай ответ");
                Console.WriteLine("2 - Об авторе"); //пункты меню
                Console.WriteLine("3 - Сортировка массива");
                Console.WriteLine("4 - Морской бой");
                Console.WriteLine("5 - Выход");
                int choose; //переменная, отвечающая за выбор
                while (!int.TryParse(Console.ReadLine(), out choose))
                {
                    Console.WriteLine("Введите число");
                }

                switch (choose)
                {
                    case 1: //первый случай, при нажатии 1, срабытавает первый пункт меню
                        Console.WriteLine("Введите a:");
                        double a;
                        while (!double.TryParse(Console.ReadLine(), out a))
                        {
                            Console.WriteLine("Введите число");
                        }

                        Console.WriteLine("Введите b:");
                        double b;
                        while (!double.TryParse(Console.ReadLine(), out b))
                        {
                            Console.WriteLine("Введите число");
                        }

                        Console.WriteLine("Чему равно значение функции: f= -4*Sin^3(3*" + a + ")+sqrt(" + b + ")/ln(" +
                                          b + "+2)?");
                        try
                        {
                            double result = -4 * Math.Pow(Math.Sin(3 * a), 3) + Math.Sqrt(b) / Math.Log(b + 2);
                            result = Math.Round(result); //округление результата до целого числа для проверки
                            double user_Result;
                            if (Double.IsNaN(result)) Console.WriteLine("Ошибка вычисления");
                            else
                            {
                                int amountOfTries = 3;
                                do
                                {
                                    while (!double.TryParse(Console.ReadLine(), out user_Result))
                                    {
                                        Console.WriteLine("Введите число");
                                    }

                                    if (user_Result == result)
                                    {
                                        //проверка на правильность
                                        Console.WriteLine("Ответ верный");
                                        Console.WriteLine("--------------");
                                    }
                                    else
                                    {
                                        amountOfTries--;
                                        Console.WriteLine(amountOfTries != 0
                                            ? "Ответ неверный, попыток осталось: "
                                              + amountOfTries
                                            : "Ответ неверный, верный ответ: " + result);
                                        Console.WriteLine("---------------");
                                    }
                                } while (amountOfTries > 0 && user_Result != result);
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Ошибка вычисления");
                        }

                        break;
                    case 2:
                        Console.WriteLine("Зубрилин Влад. ИВТ: 6101. 2020 год");
                        Console.WriteLine("--------------");
                        break;
                    case 3:
                        Console.WriteLine("Введите количество элементов массива: ");
                        int n; //кол-во элементов массива
                        while (!int.TryParse(Console.ReadLine(), out n))
                        {
                            Console.WriteLine("Введите число");
                        }

                        int[] originalArray = new int [n];
                        originalArray = RandomArray(originalArray, n);
                        Console.WriteLine("Исходный массив: ");
                        OutputArray(originalArray);
                        int[] bubbleSortedArray = new int[n];
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        bubbleSortedArray = BubbleSort(originalArray);
                        stopwatch.Stop();
                        Console.WriteLine("Сортировка пузырьком: ");
                        OutputArray(bubbleSortedArray);
                        Console.WriteLine("Потрачено времени на сортировку пузырьком: " + stopwatch.Elapsed);
                        int[] selectionSortedArray = new int[n];
                        stopwatch.Restart();
                        selectionSortedArray = SelectionSort(originalArray);
                        stopwatch.Stop();
                        Console.WriteLine("Сортировка выбором: ");
                        OutputArray(selectionSortedArray);
                        Console.WriteLine("Потрачено времени на сортировку выбором: " + stopwatch.Elapsed);
                        break;
                    case 4:
                        int[,] field = CreateEmptyField();
                        field = ShipPlacementAlgorithm(field);
                        GameAlgorithm(field);
                        break;
                    case 5:
                        Console.WriteLine("Вы хотите выйти? д - да, н - нет");
                        exit = Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("Введите 1, 2, 3, 4, 5");
                        break;
                }
            }
        }
    }
}