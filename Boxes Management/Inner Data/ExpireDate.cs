using System;
using System.Collections.Generic;
using System.Text;
using Data_Structures;


namespace Boxes_Management.Inner_Data
{
    class ExpireDate                                         
    {
        public DateTime Today { get; set; }
        public Surface_X Xkey { get; set; }
        public Surface_Y YKey { get; set; }
        public ExpireDate(Surface_X surfaceX, Surface_Y surfaceY, DateTime expireDate)
        {
            Today = expireDate; 
            Xkey = surfaceX;
            YKey = surfaceY;
        }
        public DateTime ChangeDate(DateTime date) // ChangeDate() Function
        {
            Today = date.AddMinutes(10);
            return Today;
        }
        public ExpireDate(DateTime now)
        {
            Today = now;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType()) return false;

            ExpireDate temp = (ExpireDate)obj;
          
            return temp.Today.Equals(Today);
        }
        public override string ToString() 
        {
            return $"Expire Date: {Today}";
        }
    }
}
