using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fenit.Toolbox.Core.Extension
{
    public static class SystemExtension
    {
        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }

        public static bool IntLenth(this int val, int lenth)
        {
            var valtring = val.ToString();
            return valtring.Length == lenth;
        }

        public static decimal DecimalTryParse(this string val)
        {
            decimal result = 0;
            if (val.Contains(","))
            {
                var numinf = new NumberFormatInfo {NumberDecimalSeparator = ","};
                result = decimal.Parse(val, numinf);
            }
            else
            {
                if (!string.IsNullOrEmpty(val) && !string.IsNullOrWhiteSpace(val))
                {
                    var numinf = new NumberFormatInfo {NumberDecimalSeparator = "."};
                    result = decimal.Parse(val, numinf);
                }
            }

            return result;
        }

        public static int? ConvertIntNullable(this string val)
        {
            if (int.TryParse(val, out var @int)) return @int;
            return null;
        }

        public static int ConvertInt(this string val)
        {
            int.TryParse(val, out var @int);
            return @int;
        }

        public static short? ConvertShortNullable(this string val)
        {
            if (short.TryParse(val, out var @short)) return @short;
            return null;
        }

        public static short ConvertShort(this string val)
        {
            short.TryParse(val, out var @short);
            return @short;
        }

        public static DateTime? AsNullDateTime(this string val)
        {
            return val.AsNullDateTime("yyyy-MM-dd");
        }

        public static TimeSpan AsTimeSpan(this string val)
        {
            var table = val.Split(':');
            int h;
            if (int.TryParse(table[0], out h))
            {
                int m;
                if (int.TryParse(table[1], out m)) return new TimeSpan(h, m, 0);
            }

            return new TimeSpan();
        }

        public static DateTime? AsNullDateTime(this string val, string format)
        {
            DateTime intervalVal;
            if (DateTime.TryParseExact(val,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out intervalVal))
                return intervalVal;
            return null;
        }

        public static DateTime AsDateTime(this string val, string format)
        {
            var dt = val.AsNullDateTime(format);
            if (dt != null) return (DateTime) dt;
            return DateTime.MinValue;
        }

        public static DateTime AsDateTime(this string val)
        {
            var dt = val.AsNullDateTime();
            if (dt != null) return (DateTime) dt;
            return DateTime.MinValue;
        }
    }
}