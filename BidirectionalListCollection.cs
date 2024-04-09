using System;
using TheCorporation;

namespace Lab_12
{
    /// <summary>
    /// Двунаправленный (двусвязный) список
    /// </summary>
    /// <typeparam name="T">Любой тип, реализующий интерефейс IInit и имеющий реализацию пустого конструктора</typeparam>
    public class BidirectionalListCollection<T> where T:IInit, ICloneable, new()
    {
        /// <summary>
        /// Количество элементов
        /// </summary>
        private int count;

        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count
        {
            get => count;
        }

        /// <summary>
        /// Информационное поле
        /// </summary>
        public T data;

        /// <summary>
        /// Ссылка на следующий элемент
        /// </summary>
        public BidirectionalListCollection<T> next;

        /// <summary>
        /// Ссылка на предыдущий элемент
        /// </summary>
        public BidirectionalListCollection<T> pred;

        /// <summary>
        /// Объект с пустыми полями
        /// </summary>
        public BidirectionalListCollection()
        {

        }

        /// <summary>
        /// Объект с заполненным полем data
        /// </summary>
        /// <param name="data">Информационное поле</param>
        public BidirectionalListCollection(T data)
        {
            count = 1;
            this.data = data;
        }

        /// <summary>
        /// Возвращает элемент, являющийся началом списка
        /// </summary>
        /// <returns>Объект, с полями pred = null, data = data, next = next</returns>
        public static BidirectionalListCollection<T> Begin(T data, BidirectionalListCollection<T> next)
        {
            BidirectionalListCollection<T> listBegin = new BidirectionalListCollection<T>();
            listBegin.pred = null;
            listBegin.data = data;
            listBegin.next = next;
            listBegin.count = 1;

            return listBegin;
        }

        /// <summary>
        /// Возвращает строковое представление информационного поля
        /// </summary>
        /// <returns>data.ToString()</returns>
        public override string ToString()
        {
            return data.ToString();
        }

        /// <summary>
        /// Формирует список из случайным образом сгенерированных объектов
        /// </summary>
        /// <param name="count">Длинна списка, влючая его начало</param>
        /// /// <param name="begin">Сюда помещается ссылка на начало списка</param>
        public static BidirectionalListCollection<T> Init(int count)
        {
            // Если размер <= 0, завершаем выполнение
            if (count < 0)
            {
                Print.Red("Размер списка не может быть меньше нуля");
                return null;
            }
            else if(count == 0)
            {
                return new BidirectionalListCollection<T>();
            }

            long memory = Memory.GetTotalMemory();

            // Генерируем начало списка
            BidirectionalListCollection<T> beginList;

            try
            {
                beginList = Begin((T)new T().Init(), null);
                beginList.count = count;
            }
            catch (InvalidCastException)
            {
                Print.Red("Не удалось сгенерировать объект случайным образом");
                return null;
            }

            // Если размер = 1, возвращаем начало списка
            if (count == 1)
            {
                return beginList;
            }

            // Генерируем остальные элементы списка
            BidirectionalListCollection<T> newList = new BidirectionalListCollection<T>();
            BidirectionalListCollection<T> predList = beginList;

            for (int i = 1; i < count; i++)
            {
                // Выделяем память
                newList = new BidirectionalListCollection<T>();

                // Генерируем информационное поле
                newList.data = (T)new T().Init();

                // Записываем ссылку на предыдущий элемент
                newList.pred = predList;

                // Записываем ссылку на следующий элемент
                predList.next = newList;

                // Записываем количество элементов
                newList.count = count - i;

                // Переопределяем предыдущий элемент
                predList = newList;
            }

            // Записываем в поле next последнего элемента null, завершаем список
            newList.next = null;

            Print.Yellow("Список сгенерирован");
            memory = Memory.GetTotalMemory() - memory;
            Console.WriteLine($"Выделено памяти {memory} (МБ)");

            // Возвращаем ссылку на начало списка
            return beginList;
        }

        /// <summary>
        /// Выводит список в консоль
        /// </summary>
        /// <param name="begin">Ссылка на начало списка</param>
        public static void Show(BidirectionalListCollection<T> begin)
        {
            if (begin.next == null && begin.data == null)
            {
                Print.Yellow("Список пуст");
                return;
            }   

            Print.Yellow("Двунаправленный список:");

            BidirectionalListCollection<T> nextItem = begin.next;
            if (begin.data == null)
                Console.WriteLine("Пусто");
            else
                Console.WriteLine(begin.data.ToString());

            if (begin.next == null)
                return;

            do
            {
                if (nextItem.data == null)
                    Console.WriteLine("Пусто");
                else
                    Console.WriteLine(nextItem.data.ToString());

                nextItem = nextItem.next;
            } while (nextItem != null);
        }

        /// <summary>
        /// Удаляет список из памяти
        /// </summary>
        /// <param name="begin">Ссылка на начало списка</param>
        public static void Remove(BidirectionalListCollection<T> begin)
        {
            if (begin.next == null && begin.data == null)
            {
                Print.Yellow("Список уже пуст");
                return;
            }

            begin.count = 0;
            begin.next = null;
            begin.data = default(T);

            long memory = Memory.GetTotalMemory();
            GC.Collect();

            memory -= Memory.GetTotalMemory();

            Print.Yellow("Список удалён из памяти");

            Console.WriteLine($"Освобождено памяти {memory} (МБ)");
        }

        /// <summary>
        /// Выполняет глубокое клонирование списка
        /// </summary>
        /// <param name="begin">Начало списка</param>
        /// <returns>Клон списка</returns>
        public static BidirectionalListCollection<T> Clone(BidirectionalListCollection<T> begin)
        {
            if (begin == null)
                return null;

            BidirectionalListCollection<T> newList = begin.Clone();

            if (begin.next == null)
                return newList;

            newList.pred = null;

            BidirectionalListCollection<T> newListBegin = newList;

            BidirectionalListCollection <T> nextItem = begin.next;

            // Идём по списку и последовательно клонируем все элементы
            do
            {
                newList.next = nextItem.Clone();
                newList.next.pred = newList;

                newList = newList.next;

                nextItem = nextItem.next;
            } while (nextItem != null);

            return newListBegin;
        }

        /// <summary>
        /// Выполняет поверхностное копирование списка
        /// </summary>
        /// <param name="begin">Начало списка</param>
        /// <returns>Копия списка</returns>
        public static BidirectionalListCollection<T> Copy(BidirectionalListCollection<T> begin)
        {
            if (begin == null)
                return null;

            BidirectionalListCollection<T> newList = begin;

            if (begin.next == null)
                return newList;

            newList.pred = null;

            BidirectionalListCollection<T> newListBegin = newList;

            BidirectionalListCollection<T> nextItem = begin.next;

            // Идём по списку и последовательно копируем все элементы
            do
            {
                newList.next = nextItem;
                newList.next.pred = newList;

                newList = newList.next;

                nextItem = nextItem.next;
            } while (nextItem != null);

            return newListBegin;
        }

        /// <summary>
        /// Клонирует поля data и count элемента
        /// </summary>
        /// <returns>new BidirectionalListCollection</returns>
        public BidirectionalListCollection<T> Clone()
        {
            BidirectionalListCollection<T> clone;

            if (this.data != null)
                clone = new BidirectionalListCollection<T>((T)this.data.Clone());
            else
            {
                clone = new BidirectionalListCollection<T>();
            }
            clone.count = this.count;
            return clone;
        }

        /// <summary>
        /// Добавляет элемент в конец списка
        /// </summary>
        /// <param name="begin">Начало списка</param>
        /// <param name="data">Элемент</param>
        public static void Add(BidirectionalListCollection<T> begin, T data)
        {
            // Если в списке есть только начало, сразу добавляем элемент
            if (begin.next == null)
            {
                begin.count = 2;
                begin.next = new BidirectionalListCollection<T>(data);
                begin.next.pred = begin;
                return;
            }

            BidirectionalListCollection<T> nextItem = begin.next;

            // Идём по списку и добавляем элемент в конец
            while (true)
            {
                nextItem.pred.count++;

                if (nextItem.next == null)
                {
                    nextItem.count++;
                    nextItem.next = new BidirectionalListCollection<T>(data);
                    nextItem.next.pred = nextItem;
                    nextItem.next.count = 1;
                    return;
                }

                nextItem = nextItem.next;
            }
        }

        /// <summary>
        /// Ищет элемент в списке
        /// </summary>
        /// <param name="begin">Начало списка</param>
        /// <param name="data">Элемент</param>
        /// <returns>Ссылку на найденный элемент или NULL</returns>
        public static BidirectionalListCollection<T> Find(BidirectionalListCollection<T> begin, T data)
        {
            if (data == null)
                return null;

            if (begin.data != null && begin.data.Equals(data))
                return begin;

            if (begin.next == null)
                return null;

            BidirectionalListCollection<T> nextItem = begin.next;

            // Идём по списку, ищем элемент
            do
            {
                if (nextItem.data != null && nextItem.data.Equals(data))
                    return nextItem;

                nextItem = nextItem.next;
            } while (nextItem != null);

            return null;
        }

        /// <summary>
        /// Удаляет элемент из списка
        /// </summary>
        /// <param name="begin">Начало списка</param>
        /// <param name="data">Элемент</param>
        /// <returns>Если удалось удалить - true, иначе - false</returns>
        public static bool Remove(ref BidirectionalListCollection<T> begin, T data)
        {
            if (data == null)
                return false;

            if (begin == null)
                return false;

            // Проверяем корень
            if (begin.data != null && begin.data.Equals(data))
            {
                begin.data = default;

                if (begin.next != null)
                {
                    begin.next.pred = null;
                    begin = begin.next;
                }
                else
                {
                    begin.count--;
                }

                return true;
            }

            if (begin.next == null)
                return false;

            BidirectionalListCollection<T> nextItem = begin.next;

            // Идём по спику, ищем и удаляем элемент
            do
            {
                nextItem.pred.count--;

                if (nextItem.data != null && nextItem.data.Equals(data))
                {
                    nextItem.pred.next = nextItem.next;

                    if (nextItem.next != null)
                        nextItem.next.pred = nextItem.pred;

                    nextItem.data = default;
                    return true;
                }

                nextItem = nextItem.next;
            } while (nextItem != null);

            nextItem = begin.next;

            // Идём по спику, восстанавливаем кол-во элементов
            do
            {
                nextItem.pred.count++;

                nextItem = nextItem.next;
            } while (nextItem != null);

            return false;
        }
    }
}
