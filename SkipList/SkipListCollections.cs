using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SkipList
{
    /// <summary>
    /// Represents a collection of SkipListNodes.  This class differs from the base class - NodeList -
    /// in that it contains an internal method to increment or decrement the height of the SkipListNodeList. 
    /// Incrementing the height adds a new neighbor to the list, decrementing the height removes the
    /// top-most neighbor.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the SkipListNode instances that are contained
    /// within this SkipListNodeList.</typeparam>
    public class SkipListNodeList<T> : NodeList<T>
    {
        #region Constructors
        public SkipListNodeList(int height)
            : base(height)
        {

        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Increases the size of the SkipListNodeList by one, adding a default SkipListNode.
        /// </summary>
        internal void IncrementHeight()
        {
            // add a dummy entry
            base.Items.Add(default(Node<T>));
        }

        /// <summary>
        /// Decreases the size of the SkipListNodeList by one, removing the "top-most" SkipListNode.
        /// </summary>
        internal void DecrementHeight()
        {
            // delete the last entry
            base.Items.RemoveAt(base.Items.Count - 1);
        }
        #endregion
    }

    /// <summary>
    /// Represents a collection of Node&lt;T&gt; instances.
    /// </summary>
    /// <typeparam name="T">The type of data held in the Node instances referenced by this class.</typeparam>
    public class NodeList<T> : Collection<Node<T>>
    {
        #region Constructors
        public NodeList()
            : base()
        {

        }
        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Searches the NodeList for a Node containing a particular value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The Node in the NodeList, if it exists; null otherwise.</returns>
        public Node<T> Find(T value)
        {
            foreach (Node<T> node in base.Items)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }

        #endregion
    }

    /// <summary>
    /// The Node&lt;T&gt; class represents the base concept of a Node for a tree or graph. 
    /// It contains a data item of type T, and a list of neighbors.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the Node.</typeparam>
    public class Node<T>
    {
        #region Fields
        private T data;
        private NodeList<T> neighbors;
        #endregion

        #region Properties
        public T Value
        {
            get { return data; }
            protected internal set { data = value; }
        }
        protected NodeList<T> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }
        #endregion

        #region Constructors
        protected internal Node()
        {

        }
        public Node(T data)
            : this(data, null)
        {

        }
        public Node(T data, NodeList<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }
        #endregion
    }

    /// <summary>
    /// Represents a node in a SkipList.  A SkipListNode has a Height and a set of neighboring
    /// SkipListNodes (precisely as many neighbor references as its Height, although some neighbor 
    /// references may be null).  Also, a SkipListNode contains some piece of data associated with it.
    /// </summary>
    /// <typeparam name="T">The type of the data stored in the SkipListNode.</typeparam>
    public class SkipListNode<T> : Node<T>
    {
        #region Properties
        /// <summary>
        /// Returns the height of the SkipListNode
        /// </summary>
        public int Height
        {
            get { return base.Neighbors.Count; }
        }

        /// <summary>
        /// Provides ordinally-indexed access to the neighbors of the SkipListNode.
        /// </summary>
        public SkipListNode<T> this[int index]
        {
            get { return (SkipListNode<T>)base.Neighbors[index]; }
            set { base.Neighbors[index] = value; }
        }
        #endregion

        #region Constructors
        public SkipListNode(int height)
        {
            base.Neighbors = new SkipListNodeList<T>(height);
        }
        public SkipListNode(T value, int height)
            : base(value)
        {
            base.Neighbors = new SkipListNodeList<T>(height);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Increases the height of the SkipListNode by 1.
        /// </summary>
        internal void IncrementHeight()
        {
            // Increase height by 1
            ((SkipListNodeList<T>)base.Neighbors).IncrementHeight();
        }

        /// <summary>
        /// Decreases the height of the SkipListNode by 1.
        /// </summary>
        internal void DecrementHeight()
        {
            // Decrease height by 1
            ((SkipListNodeList<T>)base.Neighbors).DecrementHeight();
        }
        #endregion
    }

    /// <summary>
    /// Represnts a SkipList.  A SkipList is a combination of a BST and a sorted link list, providing
    /// sub-linear access, insert, and deletion running times.  It is a randomized data structure, randomly
    /// choosing the heights of the nodes in the SkipList.
    /// </summary>
    /// <typeparam name="T">Type type of elements contained within the SkipList.</typeparam>
    public class SkipList<T> : ICollection<T>
    {
        #region Fields
        protected readonly double _prob = 0.5;  // the probability used in determining the heights of the SkipListNodes
        private SkipListNode<T> _head;      // a reference to the head of the SkipList
        private long _comparisons;      // an internal counter used for performance testing of the SkipList
        private int _count;
        private Random _rndNum;
        private IComparer<T> comparer;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the height of the tallest SkipListNode in the SkipList.
        /// </summary>
        public int Height
        {
            get { return _head.Height; }
        }

        /// <summary>
        /// Returns the number of total comparisons made - used for perf. testing.
        /// </summary>
        /// <value></value>
        public long Comparisons
        {
            get { return _comparisons; }
        }

        #region ICollection Properties
        /// <summary>
        /// Returns the number of elements in the SkipList
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        #endregion
        #endregion

        #region Constructors
        public SkipList()
            : this(-1, null)
        {

        }
        public SkipList(int randomSeed)
            : this(randomSeed, null)
        {

        }
        public SkipList(IComparer<T> comparer)
            : this(-1, comparer)
        {

        }
        public SkipList(int randomSeed, IComparer<T> comparer)
        {
            _head = new SkipListNode<T>(1);
            _comparisons = 0;
            _count = 0;
            _count++;
            if (randomSeed < 0)
            {
                _rndNum = new Random();
            }
            else
            {
                _rndNum = new Random(randomSeed);
            }
            this.comparer = comparer ?? Comparer<T>.Default;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Selects a height for a new SkipListNode using the "loaded dice" technique.
        /// The value selected is between 1 and maxLevel.
        /// </summary>
        /// <param name="maxLevel">The maximum value ChooseRandomHeight can return.</param>
        /// <returns>A randomly chosen integer value between 1 and maxLevel.</returns>
        protected virtual int ChooseRandomHeight(int maxLevel)
        {
            int level = 1;
            while (_rndNum.NextDouble() < _prob && level < maxLevel)
            {
                level++;
            }
            return level;
        }

        #region ICollection Methods and Related Helpers
        #region Add
        /// <summary>
        /// Adds a new element to the SkipList.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <remarks>This SkipList implementation does not allow for duplicates.  Attempting to add a
        /// duplicate value will not raise an exception, it will simply exit the method without
        /// changing the SkipList.</remarks>
        public void Add(T value)
        {
            SkipListNode<T>[] updates = BuildUpdateTable(value);
            SkipListNode<T> current = updates[0];
            // see if a duplicate is being inserted
            if (current[0] != null && comparer.Compare(current[0].Value, value) == 0)
            {
                // cannot enter a duplicate, handle this case by either just returning or by throwing an exception
                return;
            }
            // create a new node
            SkipListNode<T> n = new SkipListNode<T>(value, ChooseRandomHeight(_head.Height + 1));
            _count++;
            // if the node's level is greater than the head's level, increase the head's level
            if (n.Height > _head.Height)
            {
                _head.IncrementHeight();
                _head[_head.Height - 1] = n;
            }
            // splice the new node into the list
            for (int i = 0; i < n.Height; i++)
            {
                if (i < updates.Length)
                {
                    n[i] = updates[i][i];
                    updates[i][i] = n;
                }
            }
        }
        #endregion

        #region Clear
        /// <summary>
        /// Clears out the contents of the SkipList and creates a new head, with height 1.
        /// </summary>
        public void Clear()
        {
            _head = null;
            _head = new SkipListNode<T>(1);
            _count = 0;
        }
        #endregion

        #region Contains
        /// <summary>
        /// Determines if a particular element is contained within the SkipList.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if value is found in the SkipList; false otherwise.</returns>
        public bool Contains(T value)
        {
            SkipListNode<T> current = _head;
            // first, determine the nodes that need to be updated at each level
            for (int i = _head.Height - 1; i >= 0; i--)
            {
                while (current[i] != null)
                {
                    _comparisons++;
                    int results = comparer.Compare(current[i].Value, value);
                    if (results == 0)
                    {
                        return true;        // we found the item
                    }
                    else if (results < 0)
                    {
                        current = current[i];   // move on to the next neighbor
                    }
                    else
                    {
                        break;    // exit while loop, we need to move down the height of the current node
                    }
                }
            }
            // if we reach here, we searched to the end of the list without finding the element
            return false;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Attempts to remove a value from the SkipList.
        /// </summary>
        /// <param name="value">The value to remove from the SkipList.</param>
        /// <returns>True if the value is found and removed; false if the value is not found
        /// in the SkipList.</returns>
        public bool Remove(T value)
        {
            SkipListNode<T>[] updates = BuildUpdateTable(value);
            SkipListNode<T> current = updates[0][0];
            if (current != null && comparer.Compare(current.Value, value) == 0)
            {
                _count--;
                // We found the data to delete
                for (int i = 0; i < _head.Height; i++)
                {
                    if (updates[i][i] != current)
                    {
                        break;
                    }
                    else
                    {
                        updates[i][i] = current[i];
                    }
                }
                // finally, see if we need to trim the height of the list
                if (_head[_head.Height - 1] == null)
                {
                    // we removed the single, tallest item... reduce the list height
                    _head.DecrementHeight();
                }
                current = null;
                return true;        // the item was successfully removed
            }
            else
            {
                // the data to delete wasn't found.
                return false;
            }
        }
        #endregion

        #region BuildUpdateTable
        /// <summary>
        /// Creates a table of the SkipListNode instances that will need to be updated when an item is
        /// added or removed from the SkipList.
        /// </summary>
        /// <param name="value">The value to be added or removed.</param>
        /// <returns>An array of SkipListNode instances, as many as the height of the head node.
        /// A SkipListNode instance in array index k represents the SkipListNode at height k that must
        /// be updated following the addition/deletion.</returns>
        protected SkipListNode<T>[] BuildUpdateTable(T value)
        {
            SkipListNode<T>[] updates = new SkipListNode<T>[_head.Height];
            SkipListNode<T> current = _head;
            // determine the nodes that need to be updated at each level
            for (int i = _head.Height - 1; i >= 0; i--)
            {
                if (!(current[i] != null && comparer.Compare(current[i].Value, value) < 0))
                {
                    _comparisons++;
                }
                while (current[i] != null && comparer.Compare(current[i].Value, value) < 0)
                {
                    current = current[i];
                    _comparisons++;
                }
                updates[i] = current;
            }
            return updates;
        }
        #endregion

        #region CopyTo
        /// <summary>
        /// Copies the contents of the SkipList to the passed-in array.
        /// </summary>
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the contents of the SkipList to the passed-in array.
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            // copy the values from the skip list to array
            if (array == null)
            {
                throw new ArgumentNullException("array is null");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index is less than 0");
            }
            if (index >= array.Length)
            {
                throw new ArithmeticException("index is greater than the length of array");
            }
            if (array.Length - index <= _count)
            {
                throw new ArgumentException("insufficient space in array to store skip list starting at specified index");
            }
            SkipListNode<T> current = _head[0];
            int i = 0;
            while (current != null)
            {
                array[i + index] = current.Value;
                i++;
            }
        }
        #endregion
        #endregion

        #region GetEnumerator
        /// <summary>
        /// Returns an enumerator to access the contents of the SkipList.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            // enumerate through the skip list one element at a time
            SkipListNode<T> current = _head[0];
            while (current != null)
            {
                yield return current.Value;
                current = current[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region ToString
        /// <summary>
        /// This overridden form of ToString() is simply for displaying detailed information
        /// about the contents of the SkipList, used by SkipListTester - feel free to remove it.
        /// </summary>
        public override string ToString()
        {
            SkipListNode<T> current = _head[0];
            StringBuilder sb = new StringBuilder();
            while (current != null)
            {
                sb.Append(current.Value);
                sb.Append(" [ H=").Append(current.Height);
                for (int i = current.Height - 1; i >= 0; i--)
                {
                    sb.Append(" | ").Append(current[i] == null ? "NULL" : current[i].Value.ToString());
                }
                sb.AppendLine(" ] ; ");
                current = current[0];
            }
            return sb.ToString();
        }
        #endregion

        #region ResetComparisons
        /// <summary>
        /// Resets the internal comparison counter back to zero.  Used for performance testing (can be removed).
        /// </summary>
        public void ResetComparisons()
        {
            _comparisons = 0;
        }
        #endregion
        #endregion
    }
}
