namespace MiiskoWiiyaas.Utils
{
    public static class ArrayUtils
    {
        public static int Map2DTo1D(int x, int y, int length) => x + length * y;
        
        public static (int X, int Y) Map1DTo2D(int index, int length) => (index % length, index / length);

        public static int Map3DTo1D(int x, int y, int z, int length, int height) => x + length * y + height * z;

        public static (int X, int Y, int Z) Map1DTo3D(int index, int length, int height) => (index % length, (index / length) % height, index / (length * height));
    }

}