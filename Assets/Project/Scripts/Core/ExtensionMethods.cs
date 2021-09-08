using System;

namespace Project.Scripts.Core
{
    public static class ArrayExtensions
    {
        public static T Random<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array is empty or null.");

            return array[UnityEngine.Random.Range(0, array.Length)];
        }
    }
}