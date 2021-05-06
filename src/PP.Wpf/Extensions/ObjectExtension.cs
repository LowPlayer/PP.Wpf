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
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">转换对象</param>
        /// <param name="t">目标对象</param>
        /// <returns></returns>
        public static Boolean TryChangeType<T>(this Object obj, out T t)
        {
            var type = typeof(T);

            try
            {
                t = (T)obj.ChangeType(type);
                return true;
            }
            catch
            {
                t = default;
                return false;
            }
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static Object ChangeType(this Object obj, Type targetType)
        {
            if (obj == null)
                return null;

            var sourtType = obj.GetType();

            if (targetType.IsAssignableFrom(sourtType))
                return obj;

            if (targetType.IsEnum)
                return Enum.Parse(targetType, obj.ToString());

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                targetType = targetType.GetGenericArguments()[0];

            return Convert.ChangeType(obj, targetType);
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">转换对象</param>
        /// <returns></returns>
        public static T ChangeType<T>(this Object obj)
        {
            var type = typeof(T);

            if (obj == null)
                return default;

            return (T)ChangeType(obj, type);
        }
    }
}
