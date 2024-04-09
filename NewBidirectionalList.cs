using System;
using System.Collections.Generic;
using Lab_12;
using TheCorporation;

namespace Lab_13
{
    /// <summary>
    /// Двунаправленный список
    /// </summary>
    /// <typeparam name="T">Данные, хранимые в списке</typeparam>
    public class NewBidirectionalList<T> : BidirectionalList<T>, IEnumerable<T> where T : IInit, ICloneable, new()
    {
        /// <summary>
        /// Передача информации об изменении в коллекции
        /// </summary>
        /// <param name="source">Текущая коллекция</param>
        /// <param name="args">Информация об имзенении</param>
        public delegate void ListHandler(NewBidirectionalList<T> source, ListHandlerEventArgs<T> args);
        
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Корень
        /// </summary>
        private BidirectionalList<T> dataItem;

        /// <summary>
        /// Количество элементов
        /// </summary>
        public new int Count
        {
            get => dataItem.Count;
        }

        /// <summary>
        /// Информационное поле
        /// </summary>
        public T Data
        {
            get => dataItem.data;
        }

        /// <summary>
        /// Происходит при добавлении или удалении элементаы
        /// </summary>
        public event ListHandler ListCountChanged;

        /// <summary>
        /// Происходит при изменении элемента коллекции
        /// </summary>
        public event ListHandler ListReferenceChanged;

        /// <summary>
        /// Обработчик события ListCountChanged
        /// </summary>
        /// <param name="source">Текущая коллекция</param>
        /// <param name="args">Информация об имзенении</param>
        private void OnListCountChanged(NewBidirectionalList<T> source, ListHandlerEventArgs<T> args)
        {
            if (ListCountChanged != null)
                ListCountChanged(source, args);
        }

        /// <summary>
        /// Обработчик события ListReferenceChanged
        /// </summary>
        /// <param name="source">Текущая коллекция</param>
        /// <param name="args">Информация об имзенении</param>
        private void OnListReferenceChanged(NewBidirectionalList<T> source, ListHandlerEventArgs<T> args)
        {
            if (ListReferenceChanged != null)
                ListReferenceChanged(source, args);
        }
        
        /// <summary>
        /// Объект с пустыми полями
        /// </summary>
        public NewBidirectionalList(string name)
        {
            Name = name;
            dataItem = new BidirectionalList<T>();
        }

        /// <summary>
        /// Объект с заполненным полем data
        /// </summary>
        /// <param name="data">Информационное поле</param>
        public NewBidirectionalList(string name, T data)
        {
            Name = name;
            dataItem = new BidirectionalList<T>(data);
        }

        /// <summary>
        /// Добавление элемента в коллекцию
        /// </summary>
        /// <param name="obj">Элемент</param>
        public void Add(T obj)
        {
            BidirectionalList<T>.Add(dataItem, obj);

            OnListCountChanged(this, new ListHandlerEventArgs<T>("Добавление элемента", obj));
        }

        /// <summary>
        /// Добавление случайно сгенерированного элемента в коллекцию
        /// </summary>
        public void AddDefault()
        {
            T item = (T)new T().Init();

            BidirectionalList<T>.Add(dataItem, item);

            OnListCountChanged(this, new ListHandlerEventArgs<T>("Добавление элемента", item));
        }

        /// <summary>
        /// Удаление элемента из коллекции
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <returns>true - если элемент удалён, иначе - false</returns>
        public bool Remove(int index)
        {
            T item = dataItem[index];

            if (BidirectionalList<T>.Remove(ref dataItem, index))
            {
                OnListCountChanged(this, new ListHandlerEventArgs<T>("Удаление элемента", item));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Печатает список в консоль
        /// </summary>
        public void Show()
        {
            BidirectionalList<T>.Show(dataItem);
        }

        /// <summary>
        /// Предоставляет доступ к элементу по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Если индекс выходит за пределы коллекции - default(T), иначе поле data</returns>
        public new T this[int index]
        {
            get => dataItem[index];
            set
            {
                try
                {
                    T saveItem = dataItem[index];
                    dataItem[index] = value;
                    OnListReferenceChanged(this, new ListHandlerEventArgs<T>("Изменение элемента", saveItem));
                }
                catch
                {
                    Print.Red("Не удалось установить значение элемента");
                }
            }
        }
    }
}