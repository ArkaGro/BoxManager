using System;
using System.Collections.Generic;
using System.Text;
using Data_Structures;


namespace Boxes_Management.Inner_Data
{
    class Surface_Y: IComparable<Surface_Y>
    {
        public float Y { get; private set; }
        public int Amount { get; internal set; }
        public DateTime LastDate { get; private set; }
        public DoubleLinkedList<ExpireDate> ListOfDates { get; private set; }

        public Surface_Y(float surfaceY, int quantity, DoubleLinkedList<ExpireDate> dates, DateTime dateOfSupply)
        {
            Y = surfaceY;
            Amount = quantity;
            ListOfDates = dates;
            LastDate = dateOfSupply;
        }
        public Surface_Y(float surfaceY)
        {
            Y = surfaceY;
        }
        public DateTime ChangeDate(DateTime date) // ChangeDate() Function
        {
            LastDate = date.AddMinutes(10);
            return LastDate;
        }
        public int AddAmount(int a) // AddAmount() Function
        {
            Amount += a;
            return Amount;
        }
        public int CompareTo(Surface_Y other)
        {
            return Y.CompareTo(other.Y);
        }
        public override string ToString()
        {
            return $"{Y}, {LastDate}";
        }
    }
}
