using System;
using System.Collections.Generic;

namespace MiiskoWiiyaas.Utils
{
    public static class EnumUtils
    {
        public static List<T> GetValuesFrom<T>(int start, int end) where T : struct, IConvertible
        {
            List<T> values = EnumUtils.GetValues<T>();
            List<T> results = new List<T>();

            for (int i = start; i <= end; i++)
            {
                results.Add(values[i]);
            }

            return results;
        }

        public static List<T> GetValues<T>() where T : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(T));
            return new List<T>((T[])values);
        }
    }
}