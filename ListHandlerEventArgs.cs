using System;
using TheCorporation;


namespace Lab_13
{
    /// <summary>
    /// Данные об измениния в коллекции
    /// </summary>
    public class ListHandlerEventArgs<T> : EventArgs where T : IInit, ICloneable, new()
    {
        /// <summary>
        /// Тип изменения
        /// </summary>
        public string Changing { get; set; }

        /// <summary>
        /// Изменяемый объект
        /// </summary>
        public T ChangingObjectData { get; set; }

        /// <summary>
        /// Создаёт запись с данными об измении в коллекции
        /// </summary>
        /// <param name="changing">Тип изменения</param>
        /// <param name="changingObjectData">Изменяемый объект</param>
        public ListHandlerEventArgs(string changing, T changingObjectData)
        {
            Changing = changing;
            ChangingObjectData = changingObjectData;
        }
    }
}
