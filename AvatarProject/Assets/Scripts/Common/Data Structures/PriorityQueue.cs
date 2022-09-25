using System.Collections.Generic;

namespace AvatarBA.Common.DataStructures
{
    using System;

    public class PriorityQueue<TKey, T> where TKey : IComparable<TKey>
    {
        private List<T> _data;
        private List<TKey> _keys;

        public PriorityQueue()
        {
            _data = new List<T>();
            _keys = new List<TKey>();
        }

        public int Count => _data.Count;

        private void Swap(int a, int b)
        {
            T temp = _data[a];
            _data[a] = _data[b];
            _data[b] = temp;

            TKey tempKey = _keys[a];
            _keys[a] = _keys[b];
            _keys[b] = tempKey;
        }

        public void Enqueue(TKey key, T item)
        {
            _data.Add(item);
            _keys.Add(key);

            int childIndex = _data.Count - 1;
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;

                if (_keys[childIndex].CompareTo(_keys[parentIndex]) >= 0)
                    break;

                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            int lastIndex = _data.Count - 1;
            T front = _data[0];
            _data[0] = _data[lastIndex];
            _keys[0] = _keys[lastIndex];
            _data.RemoveAt(lastIndex);
            _keys.RemoveAt(lastIndex);

            --lastIndex;
            int parentIndex = 0;
            while (true)
            {
                int childIndex = parentIndex * 2 + 1;

                if (childIndex > lastIndex)
                    break;

                int rightChild = childIndex + 1;
                if (rightChild <= lastIndex && _keys[rightChild].CompareTo(_keys[childIndex]) < 0)
                    childIndex = rightChild;

                if (_keys[parentIndex].CompareTo(_keys[childIndex]) <= 0)
                    break;

                Swap(parentIndex, childIndex);
                parentIndex = childIndex;
            }

            return front;
        }

        public bool IsEmpty()
        {
            return Count <= 0;
        }

        public T Peek()
        {
            return _data[0];
        }
    }
}

