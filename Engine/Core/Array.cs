using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Implements a resizable array, similar to List<T> but with ref
    /// access to items, no bounds checks, no versioning
    /// </summary>
    public class Array<T> : IReadOnlyArray<T>
    {
        const int MinCapacity = 4;

        T[] array;
        int count;

        /// <summary>
        /// Initialize a new instance of array with the given capacity.
        /// </summary>
        public Array(int capacity = MinCapacity) => array = new T[System.Math.Max(capacity, MinCapacity)];

        /// <summary>
        /// Initialize a new instance of the array and copy the content from the given data.
        /// </summary>
        public Array(T[] data)
        {
            if (data == null)
            {
                array = new T[MinCapacity];
            }
            else
            {
                array = new T[System.Math.Max(MinCapacity, data.Length)];
                Array.Copy(data, 0, array, 0, data.Length);
                count = data.Length;
            }
        }

        /// <summary>
        /// Gets or sets item at index
        /// </summary>
        public ref T this[int index] => ref array[index];

        //public T this[int index] => array[index];

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
        /// Gets item at index
        /// </summary>
        public T GetItemAt(int index) => this[index];

        /// <summary>
        /// Pushes an object as the last item to this array.
        /// </summary>
        public void Push(T item)
        {
            growIfNeeded();
            array[count++] = item;
        }

        /// <summary>
        /// Pushes all items from other array to this array.
        /// </summary>
        public void Push(Array<T> other) => Push(other.array, 0, other.Count);

        /// <summary>
        /// Pushes all items from other array to this array.
        /// </summary>
        public void Push(T[] other) => Push(other, 0, other.Length);

        /// <summary>
        /// Pushes items from other array with the given index and length.
        /// </summary>
        public void Push(T[] other, int index, int length)
        {
            growIfNeeded(length);
            Array.Copy(other, index, array, count, length);
            count += length;
        }

        /// <summary>
        /// Pops the last item from this array.
        /// </summary>
        public T Pop()
        {
            Assert.IsTrue(count > 0, "Array is empty.");

            var item = array[--count];
            array[count] = default(T);
            return item;
        }

        /// <summary>
        /// Swap two items at index a and b
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Swap(int a, int b)
        {
            Assert.IsTrue((uint)a < count);
            Assert.IsTrue((uint)b < count);

            var tmp = array[a];
            array[a] = array[b];
            array[b] = tmp;
        }

        /// <summary>
        /// Reverse item order inside this array
        /// </summary>
        public void Reverse()
        {
            var hc = count / 2;
            for (int i = 0; i < hc; i++)
            {
                T tmp = array[i];
                array[i] = array[count - i - 1];
                array[count - i - 1] = tmp;
            }
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
        /// Finds index of an item that matches the given predicate.
        /// </summary>
        public int FindIndex(Predicate<T> match)
        {
            for (int i = 0; i < Count; i++)
            {
                if (match(array[i]))
                    return i;
            }
            return -1; // not found
        }

        /// <summary>
        /// Returns true if array contains item.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => IndexOf(item) != -1;

        /// <summary>
        /// Insert item at index.
        /// </summary>
        public void Insert(int index, T item)
        {
            Assert.IsTrue((uint)index > count, "Index is out of range");
            growIfNeeded();
            Array.Copy(array, index, array, index + 1, count - index);
            array[index] = item;
            ++count;
        }

        /// <summary>
        /// Removes an item at index.
        /// </summary>
        public void RemoveAt(int index)
        {
            Assert.IsTrue((uint)index < count, "Index is out of range");
            count--;
            if (index < count)
                Array.Copy(array, index + 1, array, index, count - index);

            array[count] = default(T);
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
        /// Swaps item with the last item then pop it
        /// </summary>
        public bool SwapAndPop(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return false;

            Swap(index, Count - 1);
            Pop();
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
                int k = Random.Global.NextInt(0, n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Sorts items in this array with default comparer.
        /// </summary>
        public void Sort() => Array.Sort<T>(array, 0, count, null);

        /// <summary>
        /// Sorts items in this array with the given comparison delegate.
        /// </summary>
        public void Sort(IComparer<T> comparer) => Array.Sort<T>(array, 0, count, comparer);

        /// <summary>
        /// Sorts items in this array with the given comparer.
        /// </summary>
        public void Sort(int index, int length, IComparer<T> comparer)
        {
            Array.Sort<T>(array, index, length, comparer);
        }

        /// <summary>
        /// Clears the array from all items.
        /// </summary>
        public void Clear(bool reset = true)
        {
            if (reset && count > 0)
                Array.Clear(array, 0, count);
            count = 0;
        }

        /// <summary>
        /// Returns a copy of this array
        /// </summary>
        public T[] ToArray()
        {
            T[] newArray = new T[count];
            Array.Copy(array, newArray, count);
            return newArray;
        }

        /// <summary>
        /// Returns pinned GCHandle for the internal array
        /// </summary>
        public GCHandle GetPinnedHandle() => GCHandle.Alloc(array,  GCHandleType.Pinned);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void growIfNeeded(int numGrow = 1)
        {
            var newItemCount = count + numGrow;

            if (array.Length < newItemCount)
            {
                var newSize = newItemCount + newItemCount / 2;
                newSize = System.Math.Max(MinCapacity, newSize);
                T[] newArray = new T[newSize];
                Array.Copy(array, 0, newArray, 0, count);
                array = newArray;
            }
        }
    }
}
