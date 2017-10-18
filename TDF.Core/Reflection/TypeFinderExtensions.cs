using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Core.Reflection
{
    public static class TypeFinderExtensions
    {
        /// <summary>
        /// 通过找到的类型集合来实例化集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="finder"></param>
        /// <returns></returns>
        public static IList<T> CreateInstanceOnFoundType<T>(this ITypeFinder finder) where T :class
        {
            var types = finder.FindClassesOfType<T>();
            var objects = new List<T>();
            foreach (var type in types)
            {
                try
                {
                    objects.Add((T) Activator.CreateInstance(type));
                }
                catch
                {
                    // ignored
                }
            }
            return objects;
        } 
    }
}
