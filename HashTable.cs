using System;
using System.Collections.Generic;
using TheCorporation;
using System.Diagnostics;

namespace Lab_12
{
    public class HashTable<T> where T : IInit, ICloneable, new()
    {
        /// <summary>
        /// Объекты хеш-таблицы
        /// </summary>
        private HashTableItem<T>[] items;

        /// <summary>
        /// Колличество объектов
        /// </summary>
        private int countItems = 0;

        /// <summary>
        /// Колличество объектов
        /// </summary>
        public int CountItems
        {
            get => countItems;
        }

        /// <summary>
        /// Размер хеш-таблицы
        /// </summary>
        public int Size
        {
            get
            {
                if (items != null)
                    return items.Length;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Создание пустой хеш-таблицы
        /// </summary>
        public HashTable()
        {

        }
        
        /// <summary>
        /// Создание хеш-таблицы с заданным размером
        /// </summary>
        /// <param name="size">Кол-во элементов</param>
        public HashTable(int size)
        {
            items = new HashTableItem<T>[size];
        }

        /// <summary>
        /// Рандомная генерация элементов хеш-таблицы
        /// </summary>
        /// <returns>Таблица с сгенерированными элементами</returns>
        public HashTable<T> Init()
        {
            if (Size == 0)
                return this;

            for (int i = 0; i < Size; i++)
                Add((T)new T().Init());

            return this;
        }

        /// <summary>
        /// Добавление элемента в хеш-таблицу
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns>Если удалось добавить - true, иначе - false</returns>
        public bool Add(T item)
        {
            // Если таблица полностью заполнена
            if (Size - CountItems == 0)
            {
                if (Size != 0)
                {
                    // Создаём новую таблицу с увеличенным рамзером и переписываем элементы
                    HashTableItem<T>[] saveItems = items;

                    countItems = 0;

                    items = new HashTableItem<T>[Size + 5];

                    foreach (HashTableItem<T> saveItem in saveItems)
                    {
                        if (saveItem != null)
                            Add(saveItem.value);
                    }

                    // Снова пробуем добавить элемент
                    return Add(item);
                }
                else
                {
                    // Если таблица пустая, увеличиваем размер и снова добавляем элемент
                    items = new HashTableItem<T>[Size + 5];
                    return Add(item);
                }
            }
            
            int hashCode = item.GetHashCode();
            int hashCodeIndex = hashCode % items.Length;

            // Если место свободно, добавляем элемент на него
            if (items[hashCodeIndex] == null)
            {
                items[hashCodeIndex] = new HashTableItem<T>(item, hashCode, hashCodeIndex);
                countItems++;
                return true;
            }
            else
            {
                // Дубликаты запрещены
                if (items[hashCodeIndex].value.Equals(item))
                    return false;

                // Если место не свободно, ищем свободное место в таблице
                for (int i = hashCodeIndex; i < items.Length; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = new HashTableItem<T>(item, hashCode, hashCodeIndex);
                        countItems++;
                        return true;
                    }
                    else
                    {
                        // Дубликаты запрещены
                        if (items[i].value.Equals(item))
                            return false;
                    }
                }

                for (int i = 0; i < hashCodeIndex; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = new HashTableItem<T>(item, hashCode, hashCodeIndex);
                        countItems++;
                        return true;
                    }
                    else
                    {
                        // Дубликаты запрещены
                        if (items[i].value.Equals(item))
                            return false;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Удаление элемента из хеш-таблицы
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns>Если удалось найти и удалить - true, иначе - false</returns>
        public bool Remove(T item)
        {
            // Ищем элемент на позиции, вычисленной по хеш-коду
            int hashCode = item.GetHashCode();
            int hashCodeIndex = hashCode % Size;
            HashTableItem<T> removeItem = items[hashCodeIndex];

            if (removeItem != null && removeItem.value.Equals(item))
            {
                items[hashCodeIndex] = null;
                countItems--;
                return true;
            }
            else
            {
                // Если элемент не найден на ожидаемой позиции, обходим всю хеш-таблицу
                for (int i = hashCodeIndex + 1; i < Size; i++)
                {
                    if (items[i] != null)
                    {
                        if (items[i].value.Equals(item))
                        {
                            items[i] = null;
                            countItems--;
                            return true;
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    if (items[i] != null)
                    {
                        if (items[i].value.Equals(item))
                        {
                            items[i] = null;
                            countItems--;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Поиск элемента в хеш-таблице
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns>Если элемент найден, то время поиска в ElapsedTicks, если нет, то -1</returns>
        public long Find(T item)
        {
            if (countItems == 0)
                return -1;

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            // Ищем элемент на позиции, вычисленной по хеш-коду
            int hashCode = item.GetHashCode();
            int hashCodeIndex = hashCode % Size;
            HashTableItem<T> findItem = items[hashCodeIndex];

            if (findItem != null && findItem.value != null)
            {
                if (item.Equals(findItem.value))
                {
                    stopWatch.Stop();
                    return stopWatch.ElapsedTicks;
                }
            }

            // Если элемент не найден на ожидаемой позиции, обходим всю хеш-таблицу
            for (int i = hashCodeIndex + 1; i < Size; i++)
            {
                if (items[i] != null && items[i].value.Equals(item))
                {
                    stopWatch.Stop();
                    return stopWatch.ElapsedTicks;
                }
            }

            for (int i = 0; i < Size; i++)
            {
                if (items[i] != null && items[i].value.Equals(item))
                {
                    stopWatch.Stop();
                    return stopWatch.ElapsedTicks;
                }
            }

            stopWatch.Stop();

            return -1;

        }

        /// <summary>
        /// Удаляет хеш-таблицу из памяти
        /// </summary>
        /// <returns>Кол-во освобождённой памяти</returns>
        public long Remove()
        {
            if (Size == 0)
            {
                Print.Yellow("Хеш-таблица пустая");
                return 0;
            }

            long memory = Memory.GetTotalMemory();

            items = null;
            countItems = 0;

            GC.Collect();

            memory -= Memory.GetTotalMemory();

            Print.Yellow("Хеш-таблица удалёна из памяти");

            Console.WriteLine($"Освобождено памяти {memory} (МБ)");

            return memory;
        }

        /// <summary>
        /// Выводит в консоль хеш-таблицу
        /// </summary>
        public void Show()
        {
            if (CountItems == 0)
            {
                Print.Yellow("Хеш-таблица пустая");
                return;
            }

            Print.Yellow("Хеш-таблица:");

            for (int i = 0; i < Size; i++)
            {
                if (items[i] != null)
                    Console.WriteLine(i + ") " + items[i].ToString());
            }
        }

        /// <summary>
        /// Выводит в консоль ключи хеш-таблицы
        /// </summary>
        public void ShowKeys()
        {
            if (CountItems == 0)
            {
                Print.Yellow("Хеш-таблица пустая");
                return;
            }

            Print.Yellow("Ключи хеш-таблицы:");

            for (int i = 0; i < Size; i++)
            {
                if (items[i] != null)
                    Console.WriteLine(i + ") " + items[i].value);
            }
        }

        /// <summary>
        /// Подсчитывает кол-во коллизий в хеш-таблице
        /// </summary>
        /// <returns></returns>
        public int CollisionCount()
        {
            if (CountItems < 2)
                return 0;

            // Добавляем в список найденные позиции, подсчитываем совпадения
            List<int> collisions = new List<int>();
            int collisionCount = 0;

            foreach (var item in items)
            {
                if (item != null)
                {
                    if (collisions.Contains(item.position))
                        collisionCount++;
                    else
                        collisions.Add(item.position);
                }
            }

            return collisionCount;
        }

        /// <summary>
        /// Возвращает элемент хеш-таблицы по индексу
        /// </summary>
        /// <param name="index">Позиция элемента</param>
        /// <returns>Если элемент не найден, вернёт NULL</returns>
        public HashTableItem<T> this[int index]
        {
            get
            {
                if (index >= 0 && index < Size)
                    return items[index];
                else
                    return null;
            }
        }

        /// <summary>
        /// Делает глубокую копию хеш-таблицы
        /// </summary>
        /// <returns>Новая хеш-таблица</returns>
        public HashTable<T> Clone()
        {
            if (countItems == 0)
                return new HashTable<T>();

            // Переписыаем элементы и их количество в новую таблицу
            HashTable<T> newHashTable = new HashTable<T>(Size);

            for (int i = 0; i < Size; i++)
                newHashTable.items[i] = items[i].Clone();

            newHashTable.countItems = CountItems;

            return newHashTable;
        }

    }

    /// <summary>
    /// Элемент хеш-таблицы: хранит ключ (хеш-код), значение (объект) и позицию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashTableItem<T> where T : IInit, ICloneable, new()
    {
        /// <summary>
        /// Ключ (хеш-код)
        /// </summary>
        public int key;

        /// <summary>
        /// Значение (объект)
        /// </summary>
        public T value;

        /// <summary>
        /// Позиция элемента (key % size)
        /// </summary>
        public int position;

        /// <summary>
        /// Создаёт элемент хеш-таблицы
        /// </summary>
        /// <param name="item">Данные</param>
        /// <param name="key">Хеш-код</param>
        /// <param name="position">Позиция</param>
        public HashTableItem(T item, int key, int position)
        {
            value = item;
            this.key = key;
            this.position = position;
        }

        /// <summary>
        /// Возвращает строковое представление всех полей элемента
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Ключ: " + key + " Позиция: " + position + " Значение: " + value.ToString();
        }

        /// <summary>
        /// Возвращает глубокую копию элемента
        /// </summary>
        /// <returns>Новый элемент с склонированными полями</returns>
        public HashTableItem<T> Clone()
        {
            return new HashTableItem<T>((T)value.Clone(), key, position);
        }
    }
}
