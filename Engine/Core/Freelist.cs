using System;
using System.Runtime.InteropServices;

namespace Engine
{
    /// <summary>
    /// Implements a resizable freelist
    /// </summary>
    public class Freelist<T> where T : struct
    {
        Slot[] pool;
        int next_free;

        /// <summary>
        /// Initialize an instance of freelist with the given capacity
        /// </summary>
        public Freelist(int capacity)
        {
            capacity = Math.Max(4, capacity);
            pool = new Slot[capacity];
            var last_idx = pool.Length - 1;
            for (int i = 0; i < last_idx; i++)
                pool[i].Next = i + 1;

            pool[last_idx].SetAsLast();
            next_free = 0;
        }

        /// <summary>
        /// Gets or sets item at index
        /// </summary>
        public ref T this[int index] => ref pool[index].Value;

        /// <summary>
        /// The number of items this list contains.
        /// </summary>
        public int Count => pool.Length;

        /// <summary>
        /// Returns a free slot index
        /// </summary>
        public int New()
        {
            if (next_free < 0)
            {
                // resize the pool array
                int old_size = pool.Length;
                var new_size = old_size * 2;
                Slot[] new_pool = new Slot[new_size];
                Array.Copy(pool, 0, new_pool, 0, pool.Length);
                pool = new_pool;
                var last_idx = new_size - 1;
                for (int i = old_size; i < last_idx; i++)
                    pool[i].Next = i + 1;

                pool[last_idx].SetAsLast();
                next_free = old_size;
            }

            int r = next_free;
            next_free = pool[next_free].Next;
            pool[r].UseSlot();
            return r;
        }

        /// <summary>
        /// Free's slot at the given index
        /// </summary>
        public void Free(int index)
        {
            Assert.IsTrue((uint)index < pool.Length, "Index is out of range");
            if (pool[index].IsNotFree())
            {
                pool[index].Next = next_free;
                next_free = index;
            }
        }

        struct Slot
        {
            public int Next;
            public T Value;

            public void SetAsLast() => Next = -1;
            public void UseSlot() => Next = -2;
            public bool IsNotFree() => Next == -2;
        }

    }
}