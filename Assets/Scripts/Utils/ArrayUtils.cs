using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiiskoWiiyaas.Utils
{
    public static class ArrayUtils
    {
        public static int Map2DTo1D(int x, int y, int width) => x + width * y;
        
        public static (int X, int Y) Map1DTo2D(int index, int width) => (index % width, index / width);

        public static int Map3DTo1D(int x, int y, int z, int width, int height) => x + width * y + height * z;

        public static (int X, int Y, int Z) Map1DTo3D(int index, int width, int height) => (index % width, (index / width) % height, index / (width * height));
    }

}