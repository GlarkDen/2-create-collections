using System;
using TheCorporation;

namespace Lab_13
{
    class Program
    {
        static void Main(string[] args)
        {
            // Для выбора пункта меню
            ConsoleKeyInfo answerKey;

            // Определяем начало списков
            NewBidirectionalList<Engineer> newList_1 = new NewBidirectionalList<Engineer>("MyCollection_1", new Engineer().Init());
            NewBidirectionalList<Engineer> newList_2 = new NewBidirectionalList<Engineer>("MyCollection_2", new Engineer().Init());

            // Создание журнала и подписка метода Add на события ListCountChanged и ListReferenceChanged первой коллекции
            Journal<Engineer> journal_1 = new Journal<Engineer>();
            newList_1.ListCountChanged += new NewBidirectionalList<Engineer>.ListHandler(journal_1.Add);
            newList_1.ListReferenceChanged += new NewBidirectionalList<Engineer>.ListHandler(journal_1.Add);

            // Создание журнала и подписка метода Add на событие ListReferenceChanged обеих коллекций
            Journal<Engineer> journal_2 = new Journal<Engineer>();
            newList_1.ListReferenceChanged += new NewBidirectionalList<Engineer>.ListHandler(journal_2.Add);
            newList_2.ListReferenceChanged += new NewBidirectionalList<Engineer>.ListHandler(journal_2.Add);

            // Для перехода в следующее меню
            bool nextMenu = false;

            // Список операций
            PrintOperations();

            while (true)
            {
                // Вводим данные
                answerKey = Console.ReadKey(true);

                switch (answerKey.Key)
                {
                    #region Коллекция 1
                    case ConsoleKey.D1:
                        // Список операций
                        Console.Clear();
                        PrintOperations(1);
                        nextMenu = true;

                        while (nextMenu)
                        {
                            // Вводим данные
                            answerKey = Console.ReadKey(true);

                            switch (answerKey.Key)
                            {
                                // Добавление элемента
                                case ConsoleKey.D1:
                                    Print.Border("ДОБАВЛЕНИЕ ЭЛЕМЕНТА");
                                    newList_1.Add(new Engineer().Init());
                                    Print.Yellow("Элемент добавлен в список");
                                    Print.Border();
                                    break;

                                // Удаление элемента
                                case ConsoleKey.D2:
                                    Print.Border("УДАЛЕНИЕ ЭЛЕМЕНТА");
                                    // Проверяем, что список не пуст
                                    if (newList_1.Count == 0 && newList_1.Data == null)
                                    {
                                        Print.Yellow("Список пуст");
                                        Print.Border();
                                        break;
                                    }

                                    if (newList_1.Remove(Input.PositiveInteger("Введите позицию элемента: ") - 1))
                                        Print.Yellow("Элемент удалён");
                                    else
                                        Print.Red("Не удалось удалить элемент");
                                    Print.Border();
                                    break;

                                // Печать списка
                                case ConsoleKey.D3:
                                    Print.Border("ПЕЧАТЬ СПИСКА");
                                    newList_1.Show();
                                    Print.Border();
                                    break;

                                // Изменение элемента
                                case ConsoleKey.D4:
                                    Print.Border("ИЗМЕНЕНИЕ ЭЛЕМЕНТА");
                                    int position = Input.PositiveInteger("Введите позицию элемента: ") - 1;

                                    newList_1[position] = new Engineer("Новый работник");

                                    Print.Border();
                                    break;

                                // Смена названия коллекции
                                case ConsoleKey.D5:
                                    Print.Border("НАЗВАНИЕ КОЛЛЕКЦИИ");
                                    Console.Write("Введите новое название коллекции: ");
                                    newList_1.Name = Console.ReadLine();
                                    Print.Border();
                                    break;

                                // Очистка консоли
                                case ConsoleKey.Backspace:
                                    Console.Clear();
                                    PrintOperations(1);
                                    break;

                                // Выход в меню
                                case ConsoleKey.Escape:
                                    nextMenu = false;
                                    Console.Clear();
                                    PrintOperations();
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Коллекция 2
                    case ConsoleKey.D2:
                        // Список операций
                        Console.Clear();
                        PrintOperations(2);
                        nextMenu = true;

                        while (nextMenu)
                        {
                            // Вводим данные
                            answerKey = Console.ReadKey(true);

                            switch (answerKey.Key)
                            {
                                // Добавление элемента
                                case ConsoleKey.D1:
                                    Print.Border("ДОБАВЛЕНИЕ ЭЛЕМЕНТА");
                                    newList_2.Add(new Engineer().Init());
                                    Print.Yellow("Элемент добавлен в список");
                                    Print.Border();
                                    break;

                                // Удаление элемента
                                case ConsoleKey.D2:
                                    Print.Border("УДАЛЕНИЕ ЭЛЕМЕНТА");
                                    // Проверяем, что список не пуст
                                    if (newList_2.Count == 0 && newList_2.Data == null)
                                    {
                                        Print.Yellow("Список пуст");
                                        Print.Border();
                                        break;
                                    }

                                    if (newList_2.Remove(Input.PositiveInteger("Введите позицию элемента: ") - 1))
                                        Print.Yellow("Элемент удалён");
                                    else
                                        Print.Red("Не удалось удалить элемент");
                                    Print.Border();
                                    break;

                                // Печать списка
                                case ConsoleKey.D3:
                                    Print.Border("ПЕЧАТЬ СПИСКА");
                                    newList_2.Show();
                                    Print.Border();
                                    break;

                                // Изменение элемента
                                case ConsoleKey.D4:
                                    Print.Border("ИЗМЕНЕНИЕ ЭЛЕМЕНТА");
                                    int position = Input.PositiveInteger("Введите позицию элемента: ") - 1;

                                    newList_2[position] = new Engineer("Новый работник");

                                    Print.Border();
                                    break;

                                // Смена названия коллекции
                                case ConsoleKey.D5:
                                    Print.Border("НАЗВАНИЕ КОЛЛЕКЦИИ");
                                    Console.Write("Введите новое название коллекции: ");
                                    newList_2.Name = Console.ReadLine();
                                    Print.Border();
                                    break;

                                // Очистка консоли
                                case ConsoleKey.Backspace:
                                    Console.Clear();
                                    PrintOperations(2);
                                    break;

                                // Выход в меню
                                case ConsoleKey.Escape:
                                    nextMenu = false;
                                    Console.Clear();
                                    PrintOperations();
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Журналы
                    case ConsoleKey.D3:
                        // Список операций
                        Console.Clear();
                        PrintOperations(3);
                        nextMenu = true;

                        while (nextMenu)
                        {
                            // Вводим данные
                            answerKey = Console.ReadKey(true);

                            switch (answerKey.Key)
                            {
                                // Печать журнала 1
                                case ConsoleKey.D1:
                                    Print.Border("ЖУРНАЛ 1");
                                    journal_1.ShowInformation();
                                    Print.Border();
                                    break;

                                // Печать журнала 2
                                case ConsoleKey.D2:
                                    Print.Border("ЖУРНАЛ 2");
                                    journal_2.ShowInformation();
                                    Print.Border();
                                    break;

                                // Очистка журнала 1
                                case ConsoleKey.D3:
                                    Print.Border("ОЧИСТКА ЖУРНАЛА 1");

                                    int count = Input.PositiveInteger("Введите количество удаляемых элементов: ");

                                    if (count == 0)
                                        Print.Blue("Из журнала удалено 0 элементов.");
                                    else
                                        journal_1.Clear(count);

                                    Print.Border();
                                    break;

                                // Очистка журнала 2
                                case ConsoleKey.D4:
                                    Print.Border("ОЧИСТКА ЖУРНАЛА 2");

                                    count = Input.PositiveInteger("Введите количество удаляемых элементов: ");

                                    if (count == 0)
                                        Print.Blue("Из журнала удалено 0 элементов.");
                                    else
                                        journal_2.Clear(count);

                                    Print.Border();
                                    break;

                                // Очистка консоли
                                case ConsoleKey.Backspace:
                                    Console.Clear();
                                    PrintOperations(3);
                                    break;

                                // Выход в меню
                                case ConsoleKey.Escape:
                                    nextMenu = false;
                                    Console.Clear();
                                    PrintOperations();
                                    break;
                            }
                        }
                        break;
                    #endregion

                    // Выход из программы
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        /// <summary>
        /// Выводит список доступных операций
        /// </summary>
        public static void PrintOperations(byte number = 0)
        {
            switch (number)
            {
                case 0:
                    Print.Yellow("Список методов:");

                    Console.WriteLine("1) Коллекция 1");
                    Console.WriteLine("2) Коллекция 2");
                    Console.WriteLine("3) Журналы");

                    Console.WriteLine("\nEsc) Выйти из программы");
                    break;
                
                case 1:
                    Print.Yellow("Список методов:");

                    Console.WriteLine("1) Добавление элемента");
                    Console.WriteLine("2) Удаление элемента");
                    Console.WriteLine("3) Печать списка");
                    Console.WriteLine("4) Изменение элемента");
                    Console.WriteLine("5) Смена названия коллекции");

                    Console.WriteLine("\nBackspace) Очистить консоль");
                    Console.WriteLine("Esc) Выйти в меню");
                    break;

                case 2:
                    Print.Yellow("Список методов:");

                    Console.WriteLine("1) Добавление элемента");
                    Console.WriteLine("2) Удаление элемента");
                    Console.WriteLine("3) Печать списка");
                    Console.WriteLine("4) Изменение элемента");
                    Console.WriteLine("5) Смена названия коллекции");

                    Console.WriteLine("\nBackspace) Очистить консоль");
                    Console.WriteLine("Esc) Выйти в меню");
                    break;

                case 3:
                    Print.Yellow("Список методов:");

                    Console.WriteLine("1) Печать журнала 1");
                    Console.WriteLine("2) Печать журнала 2");
                    Console.WriteLine("3) Очистка журнала 1");
                    Console.WriteLine("4) Очистка журнала 1");

                    Console.WriteLine("\nBackspace) Очистить консоль");
                    Console.WriteLine("Esc) Выйти в меню");
                    break;
            }
        }
    }
}
