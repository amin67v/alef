using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Implements a resizable array, similar to List<T> but with direct
    /// access to internal array, no bounds checks, no versioning, uses
    /// assert in debug instead of exceptions.
    /// </summary>
    public class Array<T>
    {
        const int min_capacity = 4;

        T[] array;
        int count;

        /// <summary>
        /// Initialize a new instance of array with the given capacity.
        /// </summary>
        public Array(int capacity = min_capacity) => array = new T[Math.Max(capacity, min_capacity)];

        /// <summary>
        /// Initialize a new instance of the array and copy the content from the given data.
        /// </summary>
        public Array(T[] data)
        {
            if (data == null)
            {
                array = new T[min_capacity];
            }
            else
            {
                array = new T[Math.Max(min_capacity, data.Length)];
                Array.Copy(data, 0, array, 0, data.Length);
                count = data.Length;
            }
        }

        /// <summary>
        /// Gets or sets item at index
        /// </summary>
        public ref T this[int index] => ref array[index];

        /// <summary>
        /// Access to internal array for performance, please do not use Items.Length, instead use 'Count' property on this object.
        /// </summary>
        public T[] Items => array;

        /// <summary>
        /// The number of items this array contains.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets or sets first item inside the array
        /// </summary>
        public ref T First => ref array[0];

        /// <summary>
        /// Gets or sets last item inside the array
        /// </summary>
        public ref T Last => ref array[count - 1];

        /// <summary>
        /// Pushes an object as the last item to this array.
        /// </summary>
        public void Push(T item)
        {
            grow_if_needed();
            array[count++] = item;
        }

        /// <summary>
        /// Pushes all items from other array to this array.
        /// </summary>
        public void Push(Array<T> other) => Push(other.Items, 0, other.Count);

        /// <summary>
        /// Pushes all items from other array to this array.
        /// </summary>
        public void Push(T[] other) => Push(other, 0, other.Length);

        /// <summary>
        /// Pushes items from other array with the given index and length.
        /// </summary>
        public void Push(T[] other, int index, int length)
        {
            grow_if_needed(count);
            Array.Copy(other, index, array, count, length);
            count += length;
        }

        /// <summary>
        /// Pops the last item from this array.
        /// </summary>
        public T Pop()
        {
            Assert.IsTrue(count > 0, "Array is already empty.");

            var item = array[--count];
            array[count] = default(T);
            return item;
        }

        /// <summary>
        /// Swap two items at index a and b
        /// </summary>
        public void Swap(int a, int b)
        {
            Assert.IsTrue((uint)a < count);
            Assert.IsTrue((uint)b < count);

            var tmp = array[a];
            array[a] = array[b];
            array[b] = tmp;
        }

        /// <summary>
        /// Index of the first item that match the given value.
        /// </summary>
        public int IndexOf(T item)
        {
            var eq = EqualityComparer<T>.Default;
            for (int i = 0; i < count; ++i)
            {
                if (eq.Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Index of the last item that match the given value.
        /// </summary>
        public int LastIndexOf(T item)
        {
            var eq = EqualityComparer<T>.Default;
            for (int i = count - 1; i >= 0; --i)
            {
                if (eq.Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns true of array contains item.
        /// </summary>
        public bool Contains(T item) => IndexOf(item) != -1;

        /// <summary>
        /// Insert item at index.
        /// </summary>
        public void Insert(int index, T item)
        {
            Assert.IsTrue((uint)index > count, "Index is out of range");
            grow_if_needed();
            Array.Copy(array, index, array, index + 1, count - index);
            array[index] = item;
            ++count;
        }

        /// <summary>
        /// Removes an item at index.
        /// </summary>
        public void RemoveAt(int index)
        {
            Assert.IsTrue((uint)index > count, "Index is out of range");
            Array.Copy(array, index + 1, array, index, count - index);
            array[count--] = default(T);
        }

        /// <summary>
        /// Removes the first item that matches the given object, otherwise return false.
        /// </summary>
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return false;

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Shuffle items inside this array.
        /// </summary>
        public void Shuffle()
        {
            int n = count;
            while (n > 1)
            {
                n--;
                int k = Random.Int(0, n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Sorts items in this array with default comparer.
        /// </summary>
        public void Sort() => Sort(0, count, null);

        /// <summary>
        /// Sorts items in this array with the given comparison delegate.
        /// </summary>
        public void Sort(Comparison<T> compare) => Sort(0, count, compare);

        /// <summary>
        /// Sorts items in this array with the given comparison delegate.
        /// </summary>
        public void Sort(int index, int length, Comparison<T> compare)
        {
            Assert.IsTrue((uint)index <= count, "Index is out of range.");
            Assert.IsTrue(count - index >= length, "Length cant exceed array count.");

            if (compare == null)
                compare = Comparer<T>.Default.Compare;

            var i = index;
            var hi = index + length - 1;
            while (i < hi)
            {
                var x = array[i + 1];
                var j = i;
                while (j >= index && compare(x, array[j]) < 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = x;
                i++;
            }
        }

        /// <summary>
        /// Clears the array from all items.
        /// </summary>
        public void Clear(bool reset = true)
        {
            if (reset)
                Array.Clear(array, 0, count);
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void grow_if_needed(int new_items = 1)
        {
            var new_item_count = count + new_items;

            if (array.Length < new_item_count)
            {
                var new_size = new_item_count * 2;
                new_size = Math.Max(min_capacity, new_size);
                T[] new_arr = new T[new_size];
                Array.Copy(array, 0, new_arr, 0, count);
                array = new_arr;
            }
        }
    }
}
