using System;

namespace TDF.Core
{
    public static partial class Ext
    {
        public static bool IsAssignableGenericFrom(this Type type, Type c)
        {
            if (!type.IsGenericType)
            {
                return type.IsAssignableFrom(c);
            }
            if (type == c)
            {
                return true;
            }
            if (c.IsGenericType)
            {
                if (c.GetGenericTypeDefinition() == type)
                {
                    return true;
                }
            }
            var interfaceTypes = c.GetInterfaces();
            foreach (var interfaceType in interfaceTypes)
            {
                if (!interfaceType.IsGenericType)
                {
                    continue;
                }
                if (type == interfaceType.GetGenericTypeDefinition())
                {
                    return true;
                }

            }
            return false;
        }

        public static bool IsInherit(this Type type, Type c)
        {
            if (type == c) return true;
            if (type.BaseType == null) return false;
            if (type.BaseType == c) return true;
            if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == c) return true;
            return IsInherit(type.BaseType, c);
        }
    }
}
