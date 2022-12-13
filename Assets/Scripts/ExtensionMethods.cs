using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default;

            return list.ElementAt(Random.Range(0, list.Count()));
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int amount)
        {
            if (list.Count() == 0)
                return default;

            List<T> retList = new();
            for (int i = amount; i > 0; i--)
            {
                retList.Add(GetRandomElement(list));
            }

            return retList;
        }

        public static T GetRandomElementExcluding<T>(this IEnumerable<T> list, IEnumerable<T> excludes)
        {
            IEnumerable<T> FilteredList = list.Where(x => !excludes.Contains(x));

            if (FilteredList.Count() == 0)
                return default;

            return FilteredList.ElementAt(Random.Range(0, FilteredList.Count()));
        }

        public static List<T> GetRandomElementsExcluding<T>(this IEnumerable<T> list, IEnumerable<T> excludes, int amount)
        {
            if (list.Count() == 0)
                return default;

            List<T> retList = new();
            List<T> excList = excludes.ToList();
            for (int i = amount; i > 0; i--)
            {
                var randElem = GetRandomElementExcluding(list, excList);
                if (randElem == null) return retList;
                retList.Add(randElem);
                excList.Add(randElem);
            }

            return retList;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            if (list == null) return null;

            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

    public static class EnumExtensions
    {
        public static T GetRandomEnum<T>(this T en) where T : struct, System.IConvertible
        {
            var values = System.Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static T GetRandomEnumExcluding<T>(this T en, List<string> excludes) where T : struct, System.IConvertible
        {
            List<T> except = new();

            var values = System.Enum.GetValues(typeof(T));

            foreach (string item in excludes)
            {
                System.Enum.TryParse<T>(item, out T exclude);
                except.Add(exclude);
            }

            var data = values
                .Cast<T>()
                .Except(except);

            return data.GetRandomElement();
        }
    }
}
