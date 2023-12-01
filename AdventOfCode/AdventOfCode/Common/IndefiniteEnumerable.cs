using System.Collections;

namespace AdventOfCode.Common
{
    public class IndefiniteEnumerator<T> : IEnumerator<T>
    {
        private readonly Func<int, T> _iteration;
        private int pos = -1;

        public IndefiniteEnumerator(Func<int, T> iteration)
        {
            _iteration = iteration;
        }


        public bool MoveNext()
        {
            pos += 1;
            this.Current = this._iteration(pos);
            return true;
        }

        public void Reset()
        {
            this.Current = default;
            pos = 0;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }
    }

    public class IndefiniteEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<int, T> _iteration;

        public IndefiniteEnumerable(Func<int, T> iteration)
        {
            _iteration = iteration;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new IndefiniteEnumerator<T>(_iteration);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
