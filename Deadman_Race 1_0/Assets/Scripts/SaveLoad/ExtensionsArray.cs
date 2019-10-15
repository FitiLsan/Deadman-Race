using System;

namespace DeadMan_Race
{
	public static class ExtensionsArray
	{
        /// <summary>
        /// расширяем для массивов любого типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">массив для расширения</param>
        /// <param name="y">массив на сколько расширяем</param>
        /// <returns></returns>
        public static T[] Contact<T>(this T[] x, T[] y)
        {
            //берутся массивы
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            var oldLen = x.Length;
            //расширяем его размер
            Array.Resize<T>(ref x, x.Length + y.Length);
            //копируем второй массив в первый массив
            Array.Copy(y, 0, x, oldLen, y.Length);
            //возвращается третий - расширенный массив
            return x;
        }
    }
}