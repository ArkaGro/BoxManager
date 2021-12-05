using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Data_Structures
{
   public class DoubleLinkedList<T> : IEnumerable<T> // Generic Class
    {
        Node _start;
        Node _end;
        int _count;

        public int Count { get { return _count; } protected set { _count = value; } }

        class Node
        {
            public T _data;
            public Node _next;    // Reference to "NEXT" 
            public Node _prev;

            public Node(T data)
            {
                this._data = data;
                // _next = null;  // Value of the next Reference Type "null"
            }
        }
        public bool PlaceChanger(T changePlace)// PlaceChanger() Function
        {
            Node tmp = _start;
            if (_start == null) // Checks if "_start" equals to "null"
            {
                return false;
            }

            while (_start != null) // Condition will run "switch" if "_start" is NOT equal to "null"
            {
                if (changePlace.Equals(tmp._data)) // Checks if "changePlace" equals to "tmp._data"
                {
                    if (tmp._prev != null) // Checks if "tmp._prev" is NOT Equal to "null"
                    {
                        if (tmp._next != null)  // Checks if "tmp._prev" is NOT Equal to "null"
                        {
                            tmp._prev._next = tmp._next;
                            tmp._next._prev = tmp._prev; 
                            _end._next = tmp;
                            tmp._prev = _end; 
                            _end = tmp;
                            tmp._next = null;

                            return true;
                        }
                        else
                            _end._next = tmp;
                        tmp._prev = _end;
                        _end._prev = tmp._prev._prev; 
                        _end = tmp;
                        tmp._next = null;

                        return true;
                    }
                    else if(tmp._prev == null && tmp._next == null) // Checks if "tmp._prev" is equal to "null" and "tmp._next" is equal to "null"
                    {
                        _start = tmp;
                        return true;
                    }
                    else
                        _start = tmp._next;
                    _start._prev = null;

                    _end._next = tmp;
                    tmp._prev = _end;
                    _end._prev = tmp._prev._prev;
                    _end = tmp;
                    tmp._next = null;

                    return true;
                }
                tmp = tmp._next;
            }
            return false;
        }
        public bool Search(T searchValue, out T foundValue) // Search() Function
        {
            Node tmp = _start;
            if (_start == null)
            {
                foundValue = default(T);
                return false;
            }

            while (tmp != null)
            {
                if (searchValue.Equals(tmp._data))
                {
                    foundValue = tmp._data;
                    return true;
                }
                tmp = tmp._next;
            }

            foundValue = default(T); // If "searchValue" NOT found will return "Default" Value
            return false;
        }
        public void AddFirst(T value)  // O(1)  or O(c)  - const  תמיד 3 פעולות, לא משנה כמה אברים ברשימה
        {
            Node newNode = new Node(value); // Creates new "Node"
            newNode._next = _start; // New Node becames "_START" ,  first in the LinkedList

            if (_start == null)
                _end = newNode; 
            _start = newNode; // _START equals "NEW NODE"

            _count++;
            Count = _count; 
        }
        public bool RemoveFirst(out T removedValue) // O(1)
        {
            if (_start == null) // Empty List
            {
                removedValue = default(T);
                return false;
            }
           
            removedValue = _start._data;
            _start = _start._next;
            if (_start == null) _end = null;
            _count--;
            return true;
        }
        public void AddLast(T value) // O()
        {
            Node newNode = new Node(value);

            if (_start == null)
            {
                AddFirst(value);
                return;  // With "return" don't need to write "ELSE"
            }

            _end._next = newNode;
            newNode._prev = _end; // newNode._prev equals to Last "_end" //                             
            _end = newNode;

            _count++;
            Count = _count; 
        }
        public bool RemoveLast(out T removedValue)  
        {
            if (_end == null) // Condition "IF" checks if "_end" equals to "null" (if true LinkedList<T> empty)
            {
                removedValue = default(T); // if first Node is empty, then removedValue equals to default Value
                return false; // Returns "False"
            }

            removedValue = _end._data; // removedValue equals to Last Node(_data) in the LInkedList<T>

            if (_end._prev != null) // Condition "IF" checks if "_end._prev" NOT equals to "null" 
            {
                _end._prev._next = null;
            }
            _end = _end._prev; //  _end = _end._prev; // Assigning of "Last (_end)" in the LinkedList<T> goes to the "Previous" newNode in the List

            _count--; // Subtracts from count
            return true; // Returns "True"
        }
        public bool GetAt(int position, out T foundValue) // By default "ZERO" based
        {
            if (position < 0 || position >= _count) throw new ArgumentException("Position Out Of Range!!!");  

            Node tmp = _start;
            for (int i = 0; i < position; i++)
            {
                tmp = tmp._next;
            }
            foundValue = tmp._data;
            return true;
        }
        public bool AddAt(int position, T value) // "AddAt" Function, Adds value to specific position // ADDED 05.03.2021
        {
            Node current = new Node(value); // creates new Node
            value = default(T); // default value 

            if (_count == 0) // Cheks if count equals to "0"
            {
                _start = _end = current; // If yes,  NewNode(current) will be "_start" and "_end" at the same time
            }
            else
            {
                if (position == 0) // Cheks if Inserted "position" equals to "0"
                {
                    AddFirst(value);  // Calls to "AddFirst" Function
                }
                else if (position == _count) // Cheks if Inserted "position" equals to "counter"
                {
                    AddLast(value);  // Calls to "AddLast" Function
                }
                else
                {
                    Node temp = this._start;  // "temp" equals to First Node in the LinkedList<T> "_start"
                    for (int i = 0; i < position; i++)
                    {
                        temp = temp._next; // iteration over values
                    }
                    current._prev = temp;  // "current._prev" equals to "temp" (_start) First Node in the LinkedList<T>
                    current._next = temp._next; // "current._next" equals to "temp._next"
                    current._next._prev = current; // "current._next._prev" equals to "current"
                    temp._next = current; // "temp._next" equals to "current"
                }
            }
            _count++; // Increasing counter
            return true;
        }
        public T this[int position]
        {
            get {
                if (position < 0 || position >= _count) throw new ArgumentException("Position Out Of Range!!!");

                Node tmp = _start;
                for (int i = 0; i < position; i++)
                {
                    tmp = tmp._next;
                }
                return tmp._data;
            }
        }
        public override string ToString() // O(n)
        {
            StringBuilder sb = new StringBuilder();
            Node tmp = _start;

            while (tmp != null)
            {
                sb.Append($"{tmp._data} ");
             //   sb.AppendLine(tmp._data);

                tmp = tmp._next;
            }
            return sb.ToString();
        }
        public IEnumerator<T> GetEnumerator()
        {
            //ListEnumerator enumeratorObj = new ListEnumerator(_start);  // OLD VERSION
            //return enumeratorObj;

            Node tmp = _start;
            while (tmp != null)          // NEW VERSION
            {
                yield return tmp._data;
                tmp = tmp._next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        class ListEnumerator : IEnumerator<T>
        {
            Node _start;
            Node _tmp;
            bool _isFirst;
            public ListEnumerator(Node start)
            {
                this._start = start;
                Reset();
            }
            public T Current => _tmp._data;
            object IEnumerator.Current => _tmp._data;
            public void Reset()
            {
                this._tmp = _start;
                _isFirst = true;
            }
            public bool MoveNext()
            {
                if(_isFirst)
                {
                    _isFirst = false;
                    return _start != null;
                }

                _tmp = _tmp._next;  // 
                return _tmp != null; ;
            }
            public void Dispose()
            {

            }
        }
    }
}
