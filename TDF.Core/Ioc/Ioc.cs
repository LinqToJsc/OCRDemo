using System;

namespace TDF.Core.Ioc
{
    public class Ioc
    {
        public static ContainerManager ContainerManager;

        public static T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public static T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }
    }
}
