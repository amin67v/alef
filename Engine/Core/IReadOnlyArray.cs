using System;
using System.Runtime.InteropServices;

namespace Engine
{
    /// <summary>
    /// ReadOnly dynamic array interface
    /// </summary>
    public interface IReadOnlyArray<T>
    {
        /// <summary>
        /// The number of items this array contains.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets item at index
        /// </summary>
        T GetItemAt(int index);

        /// <summary>
        /// Index of the first item that match the given value.
        /// </summary>
        int IndexOf(T item);

        /// <summary>
        /// Index of the last item that match the given value.
        /// </summary>
        int LastIndexOf(T item);

        // <summary>
        /// Finds index of an item that matches the given predicate.
        /// </summary>
        int FindIndex(Predicate<T> match);

        /// <summary>
        /// Returns true if array contains item.
        /// </summary>
        bool Contains(T item);

        /// <summary>
        /// Returns a copy of this array
        /// </summary>
        T[] ToArray();

        /// <summary>
        /// Returns pinned GCHandle for the internal array
        /// </summary>
        GCHandle GetPinnedHandle();
    }
}