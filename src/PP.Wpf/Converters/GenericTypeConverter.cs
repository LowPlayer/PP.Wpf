using PP.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// 通用类型转换器
    /// </summary>
    public sealed class GenericTypeConverter : IValueConverter
    {
        /// <summary>
        /// 正向键值对字典
        /// </summary>
        private Dictionary<String, String> Alias { get; set; }

        /// <summary>
        /// 反向键值对字典
        /// </summary>
        private Dictionary<String, String> BackAlias { get; set; }

        private String aliasStrTemp;

        /// <summary>
        /// 解析转换规则
        /// </summary>
        /// <param name="aliasStr">规则字符串</param>
        private void ParseAliasByStr(String aliasStr)
        {
            if (aliasStrTemp == aliasStr)
            {
                //避免再次解析
                return;
            }

            aliasStrTemp = aliasStr;

            Alias = new Dictionary<String, String>();
            BackAlias = new Dictionary<String, String>();

            String[] items = aliasStr.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String item in items)
            {
                var kv = item.Split(new Char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (kv.Length != 2)
                    continue;

                var key = kv[0];
                var value = kv[1];

                if (!String.Equals(key, "other", StringComparison.Ordinal))
                {
                    if (!BackAlias.ContainsKey(value))
                        BackAlias.Add(value, key);
                }

                Alias.Add(key, value);
            }
        }

        private Object ConvertCommon(Object value, Type targetType, Object parameter, Boolean isBack)
        {
            var arg = parameter as String;

            if (String.IsNullOrWhiteSpace(arg))
                return Binding.DoNothing;

            ParseAliasByStr(arg);

            Dictionary<String, String> alias;

            if (isBack)
                alias = BackAlias;
            else
                alias = Alias;

            //绑定的值
            String bindingValue = value?.ToString() ?? String.Empty;

            if (alias.ContainsKey(bindingValue))
                return alias[bindingValue].ChangeType(targetType);
            else if (alias.ContainsKey("other"))
                return alias["other"].ChangeType(targetType);

            return Binding.DoNothing;
        }

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertCommon(value, targetType, parameter, false);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertCommon(value, targetType, parameter, true);
        }
    }
}
