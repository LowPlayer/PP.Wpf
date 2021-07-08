using System;

namespace PP.Wpf.Extensions
{
    /// <summary>
    /// Object类型的扩展方法
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 尝试转换类型，若失败，则返回默认值
        /// 警告:因包含try catch,且考虑类型多,效率极低
        /// </summary>
        public static Boolean TryChangeType<T>(this Object obj, out T t, T defaultValue = default)
        {
            var type = typeof(T);

            try
            {
                obj = obj.ChangeType(type);

                if (obj == null)
                {
                    t = defaultValue;
                    return false;
                }

                t = (T)obj;
                return true;
            }
            catch
            {
                t = defaultValue;
                return false;
            }
        }

        public static Object ChangeType(this Object obj, Type targetType)
        {
            if (obj == null)
                return null;

            var sourtType = obj.GetType();

            if (obj is String str && String.IsNullOrEmpty(str) && targetType != typeof(String))
                return null;

            if (targetType.IsAssignableFrom(sourtType))
                return obj;

            if (targetType.IsEnum)
                return Enum.Parse(targetType, obj.ToString());

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                targetType = targetType.GetGenericArguments()[0];

            return Convert.ChangeType(obj, targetType);
        }

        public static T ChangeType<T>(this Object obj)
        {
            var type = typeof(T);

            obj = ChangeType(obj, type);

            if (obj == null)
                return default;

            return (T)obj;
        }
    }
}
