using System;
using System.Collections.Generic;
using TheCorporation;

namespace Lab_12
{
    /// <summary>
    /// Идеально сбалансированное бинарное дерево
    /// </summary>
    /// <typeparam name="T">Любой тип, реализующий интерефейс IInit и имеющий реализацию пустого конструктора</typeparam>
    public class BalancedBinaryTree<T> where T : IInit, IComparable, ICloneable, new()
    {
        /// <summary>
        /// Колличество элементов в дереве
        /// </summary>
        private int size;

        /// <summary>
        /// Колличество элементов в дереве
        /// </summary>
        public int Size
        {
            get => size;
        }

        /// <summary>
        /// Информационное поле
        /// </summary>
        public T data;

        /// <summary>
        /// Ссылка на левую ветку дерева
        /// </summary>
        public BalancedBinaryTree<T> left;

        /// <summary>
        /// Ссылка на правую ветку дерева
        /// </summary>
        public BalancedBinaryTree<T> right;
        
        /// <summary>
        /// Объект с пустыми полями
        /// </summary>
        public BalancedBinaryTree()
        {

        }

        /// <summary>
        /// Объект с заполненными информационным полем
        /// </summary>
        /// <param name="data">Информационное поле</param>
        public BalancedBinaryTree(T data)
        {
            size = 1;
            this.data = data;
        }

        /// <summary>
        /// Генерирует сбалансированное бинарное дерево с заданным количеством элементов
        /// </summary>
        /// <param name="size">Кол-во элементов</param>
        /// <param name="root">Корень дерева</param>
        /// <returns></returns>
        public static BalancedBinaryTree<T> Init(int size, BalancedBinaryTree<T> root)
        {
            // Если указан размер 0, сразу возвращаем корень
            if (size == 0)
                return root;

            // Генерируем данные
            root.data = (T)new T().Init();

            // Указываем оставшееся кол-во элементов
            root.size = size;

            // Рекурсивно уменьшаем размер, пока он не станет равен единице
            if (size > 2)
            {
                // Поскольку мы идём сразу в две стороны, делим размер на 2
                int leftSize = (size - 1) / 2;
                int rightSize = size - 1 - leftSize;

                // Идём в правую ветку
                root.right = new BalancedBinaryTree<T>();
                root.right = Init(rightSize, root.right);

                // Идём в левую ветку
                root.left = new BalancedBinaryTree<T>();
                root.left = Init(leftSize, root.left);
                
                size -= 2;
            }
            else if (size > 1)
            {
                // Заканчиваем дерево правой веткой
                root.right = new BalancedBinaryTree<T>();
                root.right = Init(size - 1, root.right);
                size--;
            }
            else
                return root;

            return root;
        }

        /// <summary>
        /// Добавляет элемент в сбалансированное дерево
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="data">Информационное поле элемента</param>
        /// <returns>Если data == null или root == null, возвращает false, иначе - true</returns>
        public static bool AddBalancedTree(BalancedBinaryTree<T> root, T data)
        {
            if (data == null)
                return false;

            if (root == null)
                return false;

            if (root.size % 2 == 0)
            {
                root.size++;

                if (root.left == null)
                {
                    root.left = new BalancedBinaryTree<T>(data);
                    return true;
                }

                return AddBalancedTree(root.left, data);
            }
            else
            {
                root.size++;

                if (root.right == null)
                {
                    root.right = new BalancedBinaryTree<T>(data);
                    return true;
                }

                return AddBalancedTree(root.right, data);
            }
        }

        /// <summary>
        /// Удаляет дерево из памяти
        /// </summary>
        /// <param name="root">Корень дерева</param>
        public static void Remove(BalancedBinaryTree<T> root)
        {
            if (root == null)
                return;

            root.right = null;
            root.left = null;
            root.data = default;
            root.size = 0;
        }

        /// <summary>
        /// Преобразует структуру в дерево поиска
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <returns>Если передано пустое дерево - false, иначе - true</returns>
        public static bool RemoveItem(BalancedBinaryTree<T> root, T data)
        {
            if (root.right == null && root.left == null)
                return false;

            List<T> newTreeData = new List<T>();

            GetTreeData(root, ref newTreeData);

            newTreeData.RemoveAt(0);

            if (newTreeData.Remove(data))
            {
                newTreeData.RemoveAt(0);

                root.right = null;
                root.left = null;

                foreach (T dataItem in newTreeData)
                    AddBalancedTree(root, dataItem);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Удаляет элемент (и ветку), с указанным информационным полем, из дерева (кроме корня)
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="data">Информационное поле</param>
        /// <param name="size">#Параметр для рекурсии#</param>
        /// <returns>Если элемент найден и удалён - true, иначе - false</returns>
        public static bool Remove(BalancedBinaryTree<T> root, T data, int size = 0)
        {
            if (root == null)
                return false;

            if (data == null)
                return false;

            // Идём в правую ветку и сравниваем там элементы
            if (root.right != null)
            {
                if (root.right.data != null)
                {
                    if (root.right.data.Equals(data))
                    {
                        root.size -= root.right.size;

                        if (root.right.right == null && root.right.left == null)
                            root.right = null;
                        else if (root.right.right == null)
                        {
                            if (root.left == null)
                            {
                                root.left = root.right.left;
                            }
                        }


                        return true;
                    }
                }

                size = root.right.size;

                if (Remove(root.right, data))
                {
                    root.size -= size - root.right.size;
                    return true;
                }
            }

            // Идём в левую ветку и сравниваем там элементы
            if (root.left != null)
            {
                if (root.left.data != null)
                {
                    if (root.left.data.Equals(data))
                    {
                        root.size -= root.left.size;
                        root.left = null;
                        return true;
                    }
                }

                size = root.left.size;

                if (Remove(root.left, data))
                {
                    root.size -= size - root.left.size;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Находит элемент, с указанным информационным полем в дереве (кроме корня)
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="data">Информационное поле</param>
        /// <returns>Если элемент найден - true, иначе - false</returns>
        public static bool Find(BalancedBinaryTree<T> root, T data)
        {
            if (data == null)
                return false;

            // Идём в правую ветку и сравниваем там элементы
            if (root.right != null)
            {
                if (root.right.data != null)
                {
                    if (root.right.data.Equals(data))
                        return true;
                }

                if (Find(root.right, data))
                    return true;
            }

            // Идём в левую ветку и сравниваем там элементы
            if (root.left != null)
            {
                if (root.left.data != null)
                {
                    if (root.left.data.Equals(data))
                        return true;
                }

                if (Find(root.left, data))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Находит элемент, с указанным информационным полем в дереве поиска (кроме корня)
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="data">Информационное поле</param>
        /// <returns>Если элемент найден - true, иначе - false</returns>
        public static bool SmartFind(BalancedBinaryTree<T> root, T data)
        {
            //if (data == null)
            //    return false;

            //if (root == null)
            //    return false;
            
            // Если root.data больше data, идём влево, если меньше - вправо
            if (root.data != null)
            {
                // Cравниваем элементы
                int compare = root.data.CompareTo(data);

                // Если элементы равны, уточняем равенство и завершаем поиск 
                if (compare == 0)
                {
                    return true;
                    //if (root.data.Equals(data))
                    //    return true;
                    //else
                    //    return false;
                }
                else if (compare > 0)
                {
                    if (root.left != null)
                        return SmartFind(root.left, data);
                }
                else
                {
                    if (root.right != null)
                        return SmartFind(root.right, data);
                }
            }

            return false;
        }

        /// <summary>
        /// Возвращает количество листьев дерева
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="count">#Параметр для рекурсии#</param>
        /// <returns></returns>
        public static int CountLeaves(BalancedBinaryTree<T> root, int count = 0)
        {
            if (root.right != null && root.left != null)
                return count + CountLeaves(root.right, count) + CountLeaves(root.left, count);

            if (root.right != null)
                return count + CountLeaves(root.right, count);

            if (root.left != null)
                return count + CountLeaves(root.left, count);

            return count + 1;
        }

        /// <summary>
        /// Печатает дерево в консоль
        /// </summary>
        /// <param name="root">Корень дерева</param>
        public static void Show(BalancedBinaryTree<T> root)
        {
            if (root.data != null)
                Console.WriteLine(root.data.ToString());
            else
                Console.WriteLine("Пусто");

            if (root.right != null)
                Show(root.right);

            if (root.left != null)
                Show(root.left);
        }

        /// <summary>
        /// Печатает дерево в консоль с отступами и раскраской (правые ветки - белым, левые - синим)
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="direction">#Параметр для рекурсии#</param>
        /// <param name="indent">#Параметр для рекурсии#</param>
        public static void NiceShow(BalancedBinaryTree<T> root, byte direction = 0, string indent = "")
        {
            if (direction == 0)
            {
                string message = "";
                if (root.data != null)
                    message = root.data.ToString();
                else
                    message = "Пусто";

                Print.Yellow(indent + message);

                if (root.left != null)
                    NiceShow(root.left, 2, indent + "   ");

                if (root.right != null)
                    NiceShow(root.right, 1, indent + "   ");
            }
            else if (direction == 1)
            {
                string message = "";
                if (root.data != null)
                    message = root.data.ToString();
                else
                    message = "Пусто";

                Console.WriteLine(indent + message);

                if (root.left != null)
                    NiceShow(root.left, 2, indent + "   ");

                if (root.right != null)
                    NiceShow(root.right, 1, indent + "   ");
            }
            else
            {
                string message = "";
                if (root.data != null)
                    message = root.data.ToString();
                else
                    message = "Пусто";

                Print.Blue(indent + message);

                if (root.right != null)
                    NiceShow(root.right, 1, indent + "   ");

                if (root.left != null)
                    NiceShow(root.left, 2, indent + "   ");
            }
        }

        /// <summary>
        /// Преобразует структуру в дерево поиска
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <returns>Если передано пустое дерево - false, иначе - true</returns>
        public static bool ConvertFindTree(BalancedBinaryTree<T> root)
        {
            if (root.right == null && root.left == null)
                return false;

            List<T> newTreeData = new List<T>();

            GetTreeData(root, ref newTreeData);

            newTreeData.RemoveAt(0);

            root.right = null;
            root.left = null;
            root.size = 1;

            GC.Collect();

            foreach (T data in newTreeData)
                AddFindTree(root, data);

            return true;
        }

        /// <summary>
        /// Возвращает все элементы дерева ввиде списка (включая корень)
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="tree">Ссылка на список</param>
        public static void GetTreeData(BalancedBinaryTree<T> root, ref List<T> tree)
        {
            if (root.data != null)
                tree.Add(root.data);

            if (root.right != null) 
                GetTreeData(root.right, ref tree);

            if (root.left != null)
                GetTreeData(root.left, ref tree);
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
        /// Добавляет элемент в дерево поиска
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="data">Информационное поле элемента</param>
        /// <returns>Если data == null или элемент уже присутствует в дереве, возвращает false, иначе - true</returns>
        public static bool AddFindTree(BalancedBinaryTree<T> root, T data)
        {
            if (root.data == null || data == null)
                return false;

            // Если элемент data меньше элемента root.data, идём в левую ветку
            if (root.data.CompareTo(data) > 0)
            {
                if (root.left != null)
                {
                    if (AddFindTree(root.left, data))
                    {
                        root.size++;
                        return true;
                    }

                    return false;
                }
                else
                {
                    root.size++;
                    root.left = new BalancedBinaryTree<T>(data);
                    return true;
                }
            }

            // Если элемент data больше элемента root.data, идём в правую ветку
            if (root.data.CompareTo(data) < 0)
            {
                if (root.right != null)
                {
                    if (AddFindTree(root.right, data))
                    {
                        root.size++;
                        return true;
                    }

                    return false;
                }
                else
                {
                    root.size++;
                    root.right = new BalancedBinaryTree<T>(data);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Выполняет глубокое клонирование дерева
        /// </summary>
        /// <param name="root">Корень</param>
        /// <param name="newRoot">Новый корень</param>
        /// <param name="isRoot">!!!Для рекурсии - клонирование корня</param>
        public static void Clone(BalancedBinaryTree<T> root, ref BalancedBinaryTree<T> newRoot, bool isRoot = true)
        {
            if (isRoot)
                newRoot = root.Clone();

            if (root.right != null)
            {
                newRoot.right = root.right.Clone();
                Clone(root.right, ref newRoot.right, false);
            }

            if (root.left != null)
            {
                newRoot.left = root.left.Clone();
                Clone(root.left, ref newRoot.left, false);
            }
        }

        /// <summary>
        /// Клонирует поля data и size элемента
        /// </summary>
        /// <returns>new BalancedBinaryTree</returns>
        public BalancedBinaryTree<T> Clone()
        {
            BalancedBinaryTree<T> newItem = new BalancedBinaryTree<T>();

            if (data != null)
                newItem.data = (T)this.data.Clone();
            else
                newItem.data = default;

            newItem.size = this.size;

            return newItem;
        }

    }
}
