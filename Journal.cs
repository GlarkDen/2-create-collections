using System;
using TheCorporation;
using System.Collections.Generic;

namespace Lab_13
{
    /// <summary>
    /// Журнал
    /// </summary>
    public class Journal<T> : IShow where T : IInit, ICloneable, new()
    {
        /// <summary>
        /// Кол-во элементов
        /// </summary>
        public int Count 
        {
            get => entries.Count;
        }

        /// <summary>
        /// Список изменений в журнале
        /// </summary>
        private List<JournalEntry> entries;

        /// <summary>
        /// Создание журнала
        /// </summary>
        /// <param name="size">Количество элементов</param>
        public Journal(int size = 10)
        {
            entries = new List<JournalEntry>(size);
        }

        /// <summary>
        /// Добавление записи в журнал
        /// </summary>
        /// <param name="source">Изменяемая коллекция</param>
        /// <param name="args">Информация об изменении</param>
        public void Add(NewBidirectionalList<T> source, ListHandlerEventArgs<T> args)
        {
            entries.Add(new JournalEntry(source.Name, args.Changing, args.ChangingObjectData.ToString()));
        }

        /// <summary>
        /// Печатает все записи журнала
        /// </summary>
        public void ShowInformation()
        {
            if (entries.Count == 0)
            {
                Print.Yellow("Журнал пуст");
                return;
            }

            Print.Yellow("Журнал:");

            for (int i = 0; i < entries.Count; i++)
                Console.WriteLine($"{i + 1}) {entries[i]}");
        }

        /// <summary>
        /// Удаление первых N элементов
        /// </summary>
        /// <param name="count">Количество удаляемых элементов</param>
        public void Clear(int count)
        {
            if (count <= 0)
                return;

            if (Count <= 0)
                return;
            
            if (Count < count)
            {
                Print.Blue($"Указанное количество больше числа записей. Журнал полностью очищен.");
                entries.RemoveRange(0, Count);
                return;
            }

            entries.RemoveRange(0, count);
        }

        /// <summary>
        /// Возвращает запись из журнала
        /// </summary>
        /// <param name="index">Позиция</param>
        public JournalEntry this[int index]
        {
            get => entries[index];
        }
    }

    /// <summary>
    /// Запись в журнале изменений
    /// </summary>
    public class JournalEntry
    {
        /// <summary>
        /// Название коллекции
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public string Changes { get; set; }

        /// <summary>
        /// Данные изменяемого объекта в коллекции
        /// </summary>
        public string ObjectData { get; set; }

        /// <summary>
        /// Создание записи об изменении в коллеции
        /// </summary>
        /// <param name="collectionName">Название коллекции</param>
        /// <param name="changes">Тип изменения</param>
        /// <param name="data">Данные изменяемого объекта в коллекции</param>
        public JournalEntry(string collectionName, string changes, string data)
        {
            CollectionName = collectionName;
            Changes = changes;
            ObjectData = data;
        }

        /// <summary>
        /// Запись об изменении в коллеции
        /// </summary>
        public override string ToString()
        {
            return $"Название: \"{CollectionName}\". Изменения: \"{Changes}\". Данные объекта: \"{ObjectData}\".";
        }
    }
}
