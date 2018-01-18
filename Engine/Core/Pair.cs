using System;

namespace Engine
{
    public struct Pair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public Pair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int prime = 486187739;
                int hash = prime;
                hash = (hash + Key.GetHashCode()) * prime;
                hash = (hash + Value.GetHashCode()) * prime;
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<TKey, TValue>)
                return Equals((Pair<TKey, TValue>)obj);
            else
                return false;
        }

        public bool Equals(Pair<TKey, TValue> other) => Key.Equals(other.Key) && Value.Equals(other.Value);
    }
}