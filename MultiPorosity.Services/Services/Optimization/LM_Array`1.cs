using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiPorosity.Services.Optimization
{
    /// <summary>
    ///   Class that represents a "sub-array" within a larger array by implementing
    ///   appropriate indexing using an offset and sub-count. This was implemented in
    ///   the C# version in order to preserve the existing code semantics while also
    ///   allowing the code to be compiled w/o use of /unsafe compilation flag. This
    ///   permits execution of the code in low-trust environments, such as that required
    ///   by the CoreCLR runtime of Silverlight (Mac/PC) and Moonlight (Linux)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Note - modifications to this structure will modify the parent (source) array!</remarks>
    public sealed class LM_Array<T> : IList<T>
        where T : struct
    {
        private readonly T[] _array;

        private int _offset;

        public LM_Array(T[] array,
                        int offset,
                        int count)
        {
            _array  = array;
            _offset = offset;
            Count   = count;
        }

        public static implicit operator T[](LM_Array<T> source)
        {
            T[] destination = new T[source.Count];
            Array.Copy(source._array, source.GetOffset(), destination, 0, destination.Length);

            return destination;
        }

        public static implicit operator LM_Array<T>(T[] source)
        {
            return new LM_Array<T>(source, 0, source.Length);
        }

        public int GetOffset()
        {
            return _offset;
        }

        public void SetOffset(int offset)
        {
            if(offset + Count > _array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            _offset = offset;
        }

        public void SetOffsetAndCount(int offset,
                                      int count)
        {
            if(offset + count > _array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            _offset = offset;
            Count   = count;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for(var i = _offset; i < _offset + Count; i++)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IList<T> Members

        public int Count { get; private set; }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _array[_offset + index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _array[_offset + index] = value; }
        }

        public int IndexOf(T item)
        {
            var query = from i in Enumerable.Range(_offset, Count) where _array[i].Equals(item) select i;

            foreach(var i in query)
            {
                return i - _offset;
            }

            return -1;
        }

        public void Insert(int index,
                           T   item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<T> Members

        public bool Contains(T item)
        {
            return ((IEnumerable<T>)this).Contains(item);
        }

        public void CopyTo(T[] array,
                           int arrayIndex)
        {
            Array.Copy(_array, _offset, array, arrayIndex, Count);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}