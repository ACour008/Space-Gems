using System;
using System.Collections.Generic;

namespace MiiskoWiiyaas.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Gets the values of the generic type T and stores them in a List based on a starting and ending point.
        /// </summary>
        /// <typeparam name="T">A Generic type that is a struct, enum or implements the System.IConvertible interface.</typeparam>
        /// <param name="start">The starting point for enumerating over generic type T</param>
        /// <param name="end">The ending point for the enumeration of generic type T</param>
        /// <returns>A list of the generic type T</returns>
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

        /// <summary>
        /// Gets all the values of the generic type T and stores them in a List.
        /// </summary>
        /// <typeparam name="T">A Generic type that is a struct, enum, or implements the System.IConvertible interface.</typeparam>
        /// <returns>A List of the generic type T</returns>
        public static List<T> GetValues<T>() where T : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(T));
            return new List<T>((T[])values);
        }
    }
}