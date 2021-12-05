using Data_Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boxes_Management.Inner_Data
{
    class Surface_X : IComparable<Surface_X>
    {
        public float X { get; private set; }
        public BST<Surface_Y> SurfaceY { get; private set; }
       
        public Surface_X(float surfaceX, BST<Surface_Y> surfaceY)
        {
            X = surfaceX;
            SurfaceY = surfaceY;
        }
        public Surface_X(float surfaceX)
        {
            X = surfaceX;
        }
        public int CompareTo(Surface_X other)
        {
            if (X.CompareTo(other.X) > 0)
            {
                return 1;
            }
            else if (X.CompareTo(other.X) < 0)
            {
                return - 1;
            }
            else
                return 0;
        }
        public override string ToString()
        {
            return $"{X}";
        }
    }
}
