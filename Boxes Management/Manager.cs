using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using Boxes_Management.Inner_Data;
using Data_Structures;

namespace Boxes_Management
{
    public class Manager
    {
        private Timer _timer;

        private BST<Surface_X> _surfaceX = new BST<Surface_X>();
        private BST<Surface_Y> _surfaceY = new BST<Surface_Y>();
        private DoubleLinkedList<ExpireDate> _timeToRemove = new DoubleLinkedList<ExpireDate>();

        Surface_X _foundXValue;
        Surface_Y _foundYValue;

        ExpireDate _dateOfAdding;
        ExpireDate _dateOfLastBought;

        private int _maxAmount = 50;
        private int _minAmount = 5;
        public Manager()
        {
            _timer = new Timer(
                DoThings,   // Method to Run on every tick of timer! (To check expire date)
                null,       // Method Parameter
                new TimeSpan(0, 0, 5, 0),  // Due Time - ticks for the first time!  (5 minutes)
                new TimeSpan(0, 5, 0));    // Interval time (5 minutes)

        }
        private void DoThings(object parameter) // Function to check expire dates!
        {
            ExpireDate tmp = new ExpireDate(DateTime.Now);

            if (_timeToRemove.Count == 0) // Condition Checks if "Double Linked List" is Empty
            {
                return;
            }
            _timeToRemove.GetAt(0, out _dateOfAdding); // GetAt() Function brings First "Node" (Box) from the "Double Linked List"

            if (tmp.Today.CompareTo(_dateOfAdding.Today) >= 0) // Condition Checks If Today is Equals OR BIgger than "_dateOfAdding" of the First "Node" (Box) in the "Double Linked LIst"
            {

                _surfaceY.Search(_dateOfAdding.YKey, out _foundYValue); // Calls To Search Function() in "_surfaceY BST" 
                _surfaceX.Search(_dateOfAdding.Xkey, out _foundXValue); // Calls To Search Function() in "_surfaceX BST" 

                _timeToRemove.RemoveFirst(out _dateOfAdding); // RemoveLast() Function in the "Double Linked List"

                _foundXValue.SurfaceY.Remove(_foundYValue); // Remove() Function in the "BST Tree" of "_surfaceY"
                _surfaceX.Remove(_foundXValue); // Remove() Function in the "BST Tree" of "_surfaceX"
                MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}, Were Expired Date! They Were Removed!", "Message For Manager!");
            }
        }
        public string Supply(float x, float y, int quantity) // Supply() Function
        {
            DateTime addsTimeToExpireDate = DateTime.Now.AddMinutes(0); // Adds 10 Extra minutes to Expire Date - Could be Changed by requirement to hours/days/weeks etc.

            Surface_X surface_X = new Surface_X(x, _surfaceY); // Creates a new "surface_X"
            Surface_Y surface_Y = new Surface_Y(y, quantity, _timeToRemove, addsTimeToExpireDate); // Creates a new "surface_Y"
            ExpireDate expireDate = new ExpireDate(surface_X, surface_Y, surface_Y.LastDate);  // Creates a new "expireDate"

            if (_surfaceX.Search(surface_X, out _foundXValue) == false) // If SurfaceX does not exist in "BST"
            {
                _surfaceX.Add(surface_X);  // Adds SurfaceX to "BST"
                surface_X.SurfaceY.Add(surface_Y);  // Adds SurfaceY to "BST"
                surface_Y.ListOfDates.AddLast(expireDate);// Adds expireDate to Double Linked List 
            }
            else  // If SurfaceX Exist in BST
            {
                if (surface_X.SurfaceY.Search(surface_Y, out _foundYValue) == false) // If SurfaceY does not exist in "BST"
                {
                    surface_X.SurfaceY.Add(surface_Y);  // Adds SurfaceY to "BST"
                    surface_Y.ListOfDates.AddLast(expireDate); // Adds expireDate to "Double Linked List"
                }
                else
                {
                    _foundYValue.AddAmount(quantity); // Calls To AddAmount() Function
                    if (_foundYValue.Amount > _maxAmount) // Condition to check, if Amount of boxes bigger then Maximal Amount
                    {
                        int bringIt = _foundYValue.Amount - _maxAmount; // Creates Value to Return 
                        _foundYValue.Amount = _maxAmount; // Amount of the "_foundYValue" becames Maximum
                        return $"{bringIt}, Bringed Boxes To Supplier!\nBoxes Of Size: {surface_X.X} x {surface_Y.Y}\nAmount is: {_foundYValue.Amount}"; // Displays Amount of boxes that were returned to supplier
                    }
                }
            }
            return $"Boxes Were Added To WareHouse!\nAmount: {quantity}"; // Displays Amount Of Boxes That Were Added To WareHouse
        }
        public string ShowBoxInfo(float x, float y) // ShowBoxInfo() Function
        {
            Surface_X bottom = new Surface_X(x);
            Surface_Y height = new Surface_Y(y);

            if (_surfaceX.Search(bottom, out _foundXValue) == true && _surfaceY.Search(height, out _foundYValue) == true) // Condtition calls to Search() functions in each "BST" and checks if values are exists in "Trees"
            {
                return $"Bottom Size: {_foundXValue.X}\nHeight Size: {_foundYValue.Y}\nAmount: {_foundYValue.Amount}\n{_foundYValue.LastDate}";
            }
            else
                return $"There is no boxes with size: {x} x {y}";
        }
        private bool BuyBestMatch(Surface_Y min) // BuyBestMatch() Function
        {
            bool search = true;
            while (search == true)
            {
                _surfaceY.SearchNext(min, out _foundYValue); // Calls To SearchNext() Function From "BST" - Function Returns Next Leaf than bigger then searched value
                if (min.Y < _foundYValue.Y && _foundYValue.Y < min.Y * 1.2)  // Condition that checks if founded value not bigger than 20 % from original value
                {
                    return true;
                }
            }
            return false;
        }
        private bool BuyBestMatch(Surface_X min) // BuyBestMatch() Function
        {
            bool search = true;
            while (search == true)
            {
                _surfaceX.SearchNext(min, out _foundXValue); // Calls To SearchNext() Function From "BST" - Function Returns Next Leaf than bigger then searched value
                if (min.X < _foundXValue.X && _foundXValue.X < min.X * 1.2)  // Condition that checks if founded value not bigger than 20 % from original value
                {
                    return true;
                }
            }
            return false;
        }
        public string BuyBox(float x, float y) // BuyBox() Function
        {
            Surface_X buyX = new Surface_X(x);
            Surface_Y buyY = new Surface_Y(y);
            ExpireDate searchDate;

            if (_surfaceX.GetDepth() == 0) // Condition Checks if "BST Tree" "_surfaceX" is Empty
            {
                return "There Is Nothing To Buy!";
            }

            if (_surfaceX.Search(buyX, out _foundXValue) == true) // Condition Checks if Inserted Size exist in The "BST Tree"
            {
                if (_surfaceY.Search(buyY, out _foundYValue) == true) // Condition Checks if Inserted Size exist in The "BST Tree"
                {
                    searchDate = new ExpireDate(_foundXValue, _foundYValue, _foundYValue.LastDate);
                    _foundYValue.ListOfDates.Search(searchDate, out _dateOfLastBought); // Function Search() searching for "searchDate" in the "ListOfDates" (Double Linked List)
                    _foundYValue.ListOfDates.PlaceChanger(_dateOfLastBought);  // Function PlaceChanger() moves founded "_dateOfLastBought" to the Last Place in the "ListOfDates" (Double Linked List)

                    _dateOfLastBought.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_dateOfLastBought" in the "ListOfDates" (Double Linked List)
                    _foundYValue.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_foundedYValue"

                    _foundYValue.Amount -= 1; // Subtract from "_foundedYValue" Amount
                    if (1 <= _foundYValue.Amount && _foundYValue.Amount <= _minAmount)
                    {
                        MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\n{_foundYValue.Amount} Pieces Left!\nWaiting for supply!", "Message For Manager!");
                    }
                    if (_foundYValue.Amount == 0) // Condition Checks if "_foundedYValue" Amount equals to "0"
                    {
                        _surfaceY.Remove(_foundYValue); // Remove() Function in the "BST Tree" of "_surfaceY"
                        _surfaceX.Remove(_foundXValue); // Remove() Function in the "BST Tree" of "_surfaceX"
                        _foundYValue.ListOfDates.RemoveLast(out _dateOfLastBought); // RemoveLast() Function in the "Double Linked List"
                        MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\nThe Last Box Was Saled!\nWaiting for supply!", "Message For Manager!");
                    }
                    return $"You Bought A Box, Size: {x} x {y}".ToString();
                }
                else if (_surfaceY.Search(buyY, out _foundYValue) == false) // Condition Checks if Inserted Size exist in The "BST Tree"
                {
                    if (BuyBestMatch(buyY) == true)  // Calls to Function BuyBestMuch() if it brings "TRUE" => found a suitable box
                    {
                        var result = MessageBox.Show($"But, We Found {x} x {_foundYValue.Y}\nIf You Agree Press <Yes>", $"There Is No Boxes With This Size: {x} x {y}", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes) // Condition Checks Answer of User
                        {
                            searchDate = new ExpireDate(_foundXValue, _foundYValue, _foundYValue.LastDate);
                            _foundYValue.ListOfDates.Search(searchDate, out _dateOfLastBought); // Function Search() searching for "searchDate" in the "ListOfDates" (Double Linked List)
                            _foundYValue.ListOfDates.PlaceChanger(_dateOfLastBought);  // Function PlaceChanger() moves founded "_dateOfLastBought" to the Last Place in the "ListOfDates" (Double Linked List)

                            _dateOfLastBought.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_dateOfLastBought" in the "ListOfDates" (Double Linked List)
                            _foundYValue.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_foundedYValue"

                            _foundYValue.Amount -= 1; // Subtract from "_foundedYValue" Amount
                            if (1 <= _foundYValue.Amount && _foundYValue.Amount <= _minAmount)
                            {
                                MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\n{_foundYValue.Amount} Pieces Left!\nWaiting for supply!", "Message For Manager!");
                            }
                            if (_foundYValue.Amount == 0) // Condition Checks if "_foundedYValue" Amount equals to "0"
                            {
                                _surfaceY.Remove(_foundYValue); // Remove() Function in the "BST Tree" of "_surfaceY"
                                _surfaceX.Remove(_foundXValue); // Remove() Function in the "BST Tree" of "_surfaceX"
                                _foundYValue.ListOfDates.RemoveLast(out _dateOfLastBought); // RemoveLast() Function in the "Double Linked List"
                                MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\nThe Last Box Was Saled!\nWaiting for supply!", "Message For Manager!");
                            }
                            return $"You Bought A Box, Size: {x} x {_foundYValue.Y}".ToString();
                        }
                    }
                }
                return $"There Is No Boxes With This Size: {x} x {y}".ToString();
            }
            else if (_surfaceX.Search(buyX, out _foundXValue) == false) // Condition Checks if Inserted Size exist in The "BST Tree"
            {
                if (BuyBestMatch(buyX) == true) // Calls to Function BuyBestMuch() if it brings "TRUE" => found a suitable box
                {
                    var result = MessageBox.Show($"But, We Found Bottom Size: {_foundXValue.X}\nIf You Agree Press <Yes>", $"There Is No Boxes With Bottom Size: {x}", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes) // Condition Checks Answer of User
                    {
                        if (_surfaceY.Search(buyY, out _foundYValue) == true) // Condition Checks if Inserted Size exist in The "BST Tree"
                        {
                            searchDate = new ExpireDate(_foundXValue, _foundYValue, _foundYValue.LastDate);
                            _foundYValue.ListOfDates.Search(searchDate, out _dateOfLastBought); // Function Search() searching for "searchDate" in the "ListOfDates" (Double Linked List)
                            _foundYValue.ListOfDates.PlaceChanger(_dateOfLastBought); // Function PlaceChanger() moves founded "_dateOfLastBought" to the Last Place in the "ListOfDates" (Double Linked List)

                            _dateOfLastBought.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_dateOfLastBought" in the "ListOfDates" (Double Linked List)
                            _foundYValue.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_foundedYValue"

                            _foundYValue.Amount -= 1; // Subtract from "_foundedYValue" Amount
                            if (1 <= _foundYValue.Amount && _foundYValue.Amount <= _minAmount)
                            {
                                MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\n{_foundYValue.Amount} Pieces Left!\nWaiting for supply!", "Message For Manager!");
                            }
                            if (_foundYValue.Amount == 0) // Condition Checks if "_foundedYValue" Amount equals to "0"
                            {
                                _surfaceY.Remove(_foundYValue); // Remove() Function in the "BST Tree" of "_surfaceY"
                                _surfaceX.Remove(_foundXValue); // Remove() Function in the "BST Tree" of "_surfaceX"
                                _foundYValue.ListOfDates.RemoveLast(out _dateOfLastBought); // RemoveLast() Function in the "Double Linked List"
                                MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\nThe Last Box Was Saled!\nWaiting for supply!", "Message For Manager!");
                            }
                            return $"You Bought A Box, Size: {_foundXValue.X} x {_foundYValue.Y}".ToString();
                        }
                        else if (_surfaceY.Search(buyY, out _foundYValue) == false) // Condition Checks if Inserted Size exist in The "BST Tree"
                        {
                            if (BuyBestMatch(buyY) == true)  // Calls to Function BuyBestMuch() if it brings "TRUE" => found a suitable box
                            {
                                var result2 = MessageBox.Show($"But, We Found Height Size: {_foundYValue.Y}\nIf You Agree Press <Yes>", $"There Is No Boxes With Height Size: {y}", MessageBoxButton.YesNo);

                                if (result2 == MessageBoxResult.Yes) // Condition Checks Answer of User
                                {
                                    searchDate = new ExpireDate(_foundXValue, _foundYValue, _foundYValue.LastDate);
                                    _foundYValue.ListOfDates.Search(searchDate, out _dateOfLastBought); // Function Search() searching for "searchDate" in the "ListOfDates" (Double Linked List)
                                    _foundYValue.ListOfDates.PlaceChanger(_dateOfLastBought);  // Function PlaceChanger() moves founded "_dateOfLastBought" to the Last Place in the "ListOfDates" (Double Linked List)

                                    _dateOfLastBought.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_dateOfLastBought" in the "ListOfDates" (Double Linked List)
                                    _foundYValue.ChangeDate(_foundYValue.LastDate); // Function ChangeDate() change date to "_foundedYValue"

                                    _foundYValue.Amount -= 1; // Subtract from "_foundedYValue" Amount
                                    if (1 <= _foundYValue.Amount && _foundYValue.Amount <= _minAmount)
                                    {
                                        MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\n{_foundYValue.Amount} Pieces Left!\nWaiting for supply!", "Message For Manager!");
                                    }
                                    if (_foundYValue.Amount == 0) // Condition Checks if "_foundedYValue" Amount equals to "0"
                                    {
                                        _surfaceY.Remove(_foundYValue); // Remove() Function in the "BST Tree" of "_surfaceY"
                                        _surfaceX.Remove(_foundXValue); // Remove() Function in the "BST Tree" of "_surfaceX"
                                        _foundYValue.ListOfDates.RemoveLast(out _dateOfLastBought); // RemoveLast() Function in the "Double Linked List"
                                        MessageBox.Show($"Boxes With Size: {_foundXValue.X} x {_foundYValue.Y}\nThe Last Box Was Saled!\nWaiting for supply!", "Message For Manager!");
                                    }
                                    return $"You Bought A Box, Size: {_foundXValue.X} x {_foundYValue.Y}".ToString();
                                }
                            }
                        }
                    }
                }
            }
            return $"There Is No Boxes With This Size: {x} x {y}".ToString();
        }
    }
}
 