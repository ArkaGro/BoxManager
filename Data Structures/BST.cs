using System;

namespace Data_Structures
{
    public class BST<T> where T : IComparable<T>
    {
        Node _root;
        Node _data;
        public bool Search(T dataSearch, out T dataFound)
        {
            Node tmp = _root;

            while(tmp != null)
            {
                if (dataSearch.CompareTo(tmp._data) < 0)
                {
                    tmp = tmp._left;
                }
                else if (dataSearch.CompareTo(tmp._data) > 0)
                {
                    tmp = tmp._right;
                }
                else
                {
                    dataFound = tmp._data;
                    return true;
                }
            }
            dataFound = default(T);
            return false;
        }
        public T SearchNext(T dataSearch, out T dataFound) 
        {
            Node tmp = _root;

            while (tmp != null)
            {
                if (dataSearch.CompareTo(tmp._data) < 0)
                {
                    if (tmp._left == null)
                    {
                        return dataFound = tmp._data;
                    }
                    else
                            tmp = tmp._left; // LAST ADDED  07.03.2021
                }
                else if (dataSearch.CompareTo(tmp._data) > 0)
                {
                    if (tmp._right == null)
                    {
                        return dataFound = tmp._data;
                    }
                    else
                        tmp = tmp._right; // LAST ADDED  07.03.2021
                }
            }
            dataFound = default(T);
            return dataFound;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Add(T value)  // O(logn) - O(n)
        {
            if (_root == null)  // If Binary Tree is Empty
            {
                _root = new Node(value);
                return;
            }
            Node tmp = _root;
            bool toContinue = true;

            while (toContinue)
            {
                if (value.CompareTo(tmp._data) < 0)
                {
                    if (tmp._left == null)
                    {
                        tmp._left = new Node(value);
                        toContinue = false;
                    }
                    else tmp = tmp._left;
                }
                else // value >= tmp._data
                {
                    if (tmp._right == null)
                    {
                        tmp._right = new Node(value);
                        toContinue = false;
                    }
                    else tmp = tmp._right;
                }
            }
        }
        public int GetDepth()
        {
            return GetDepth(_root);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Remove(T valueToRemove)//////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            if (_root == null) // CHeck if Root Is Empty
                return;

            Node current = _root, parent = null; // Two NODES to Use:  first _root and second empty(null)

            int result;
            do
            {
                result = (valueToRemove.CompareTo(current._data)); // result equals to compairing datas between "valueToRemove" and "current._data"(_root)
                if (result < 0) // Check if "result" less than "ZERO"
                {
                    parent = current;        // Empty(null) Node "parent" becames "current" Node
                    current = current._left; // "current" Node goes left
                }
                else if (result > 0)  // Check if "result" bigger than "ZERO"
                {
                    parent = current;        // Empty(null) Node "parent" becames "current" Node
                    current = current._right; // "current" Node goes right
                }
                if (current == null) // Check if "current" equals to "null"
                    return;
            }
            while (result != 0); // Condition "While" will works till "result" not equals to "ZERO"

            if (current._right == null) //  Check If "current._right" equals to "ZERO"
            {
                if (current == _root)  // Check If ""current" equals to "_root"
                    _root = current._left; // "_root" becames "current._left"
                else
                {
                    result = (valueToRemove.CompareTo(parent._data)); // result equals to compairing datas between "valueToRemove" and "parent._data"
                    if (result < 0) // Check if "result" less than "ZERO"
                        parent._left = current._left;  // "parent._left" becames "current._left"
                    else
                        parent._right = current._left;  // "parent._left" becames "current._left"
                }
            }
            else if (current._right._left == null)  // Checks If "current._rught._left" equals to "ZERO"
            {
                current._right._left = current._left; // "current._right._left" becames "current._left"
                if (current == _root) // Check If ""current" equals to "_root"
                    _root = current._right; // "_root" becames "current._right"
                else
                {
                    result = (valueToRemove.CompareTo(parent._data)); // result equals to compairing datas between "valueToRemove" and "parent._data"
                    if (result < 0) // Check if "result" less than "ZERO"
                        parent._left = current._right; // "parent._left" becames "current.right"
                    else
                        parent._right = current._right; // "parent._right" becames "current._right"
                }
            }
            else
            {
                Node min = current._right._left, prev = current._right; // Makes Node "min" that equals to "current._right._left" and Node "prev" that equals to "current._right"
                while (min._left != null) // Condition "While" will works till "min._left" not equals to "ZERO"
                {
                    prev = min; // "prev" becames "min"
                    min = min._left;  // "min" becames "min._left"
                }
                prev._left = min._right; // "prev._left" becames "min._right"
                min._left = current._left; // "min._left" becames "current._left"
                min._right = current._right; // "min._right" becames "current._right"

                if (current == _root) // Check If ""current" equals to "_root"
                    _root = min;  // "_root" equals to "min"
                else
                {
                    result = (valueToRemove.CompareTo(parent._data)); // result equals to compairing datas between "valueToRemove" and "parent._data"
                    if (result < 0) // Check if "result" less than "ZERO"
                        parent._left = min; // "parent._left" becames "min"
                    else
                        parent._right = min; // "parent._right" becames "min"
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private int GetDepth(Node tmp) => (tmp == null) ? 0 : Math.Max(GetDepth(tmp._left), GetDepth(tmp._right)) + 1;

        public void ScanInOrder(Action<T> action)
        {
            ScanInOrder(_root, action);
        }
        private void ScanInOrder(Node tmp, Action<T> action)  // Best to Binary Search Tree  // O(n)
        {
            if (tmp == null) return;  // Cheks if "root" equals to "NULL"

            ScanInOrder(tmp._left, action); // Recursion "Goes Left"
            action(tmp._data);
            ScanInOrder(tmp._right, action);  // Recursion "Goes Right"
        }
        public class Node
        {
            public Node _left;
            public Node _right;
            public T _data;

            public Node(T data)
            {
                this._data = data;
            }
        }
    }
}
