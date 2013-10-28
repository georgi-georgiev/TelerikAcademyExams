using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deque
{
    /// <summary>
    /// Represent a collection of elements which can be accessed
    /// from the begin and from the end
    /// </summary>
    /// <typeparam name="T">type parameter is a placeholder
    /// for a specific type that a client specifies
    /// when they instantiate a variable of the generic type.</typeparam>
    public interface IDeque<T>
    {
        /// <summary>
        /// Get count of elements in deque.
        /// </summary>
        /// <returns>Count of elements in deque.</returns>
        int Count { get;}

        /// <summary>
        /// Insert element at the begining of the deque.
        /// </summary>
        /// <param name="element">Element of specific type
        /// from instance of interface which you want to insert</param>
        void PushFirst(T element);
        
        /// <summary>
        /// Insert element at the end of the deque.
        /// </summary>
        /// <param name="element">Element which you want to insert</param>
        void PushLast(T element);

        /// <summary>
        /// Get and remove first element from the deque.
        /// </summary>
        /// <returns>Element at first position from the deque.</returns>
        T PopFirst();

        /// <summary>
        /// Get and remove last element from the deque.
        /// </summary>
        /// <returns>Element at last position from the deque.</returns>
        T PopLast();

        /// <summary>
        /// Get first element from the deque.
        /// </summary>
        /// <returns>Element at first position from the deque.</returns>
        T PeekFirst();

        /// <summary>
        /// Get last element from the deque.
        /// </summary>
        /// <returns>Element at last position from the deque.</returns>
        T PeekLast();

        /// <summary>
        /// Remove all elements
        /// </summary>
        void Clear();

        /// <summary>
        /// Check if deque contains element
        /// </summary>
        /// <param name="element">Element which you want to check.</param>
        /// <returns>True if the deque contains the element,
        /// False if the deque did not contains the element</returns>
        bool Contains(T element);
    }
    
    /// <summary>
    /// Represent a collection of elements which can be accessed
    /// from the begin and from the end
    /// </summary>
    /// <typeparam name="T">type parameter is a placeholder
    /// for a specific type that a client specifies
    /// when they instantiate a variable of the generic type.</typeparam>
    public class Deque<T> : IDeque<T>
    {
        private LinkedList<T> elements;

        /// <summary>
        /// Initilize a new instace of type Deque
        /// </summary>
        public Deque()
        {
            elements = new LinkedList<T>();
        }

        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        public void PushFirst(T element)
        {
            this.elements.AddFirst(element);
        }

        public void PushLast(T element)
        {
            this.elements.AddLast(element);
        }

        public T PopFirst()
        {
            T firstElement = this.PeekFirst();
            this.elements.RemoveFirst();
            return firstElement;
        }

        public T PopLast()
        {
            T lastElement = this.PeekLast();
            this.elements.RemoveLast();
            return lastElement;
        }

        public T PeekFirst()
        {
            return this.elements.First();
        }

        public T PeekLast()
        {
            return this.elements.Last();
        }

        public void Clear()
        {
            this.elements.Clear();
        }

        public bool Contains(T element)
        {
            if (this.elements.Contains(element))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
