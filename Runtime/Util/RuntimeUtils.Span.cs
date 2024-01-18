using System;
using System.Collections.Generic;

namespace gomoru.su.CostumeController
{
    static partial class RuntimeUtils
    {
        public static T FirstOrDefault<T>(this Span<T> span, Func<T, bool> condition = null) => ((ReadOnlySpan<T>)span).FirstOrDefault(condition);
        public static T FirstOrDefault<T>(this ReadOnlySpan<T> span, Func<T, bool> condition = null)
        {
            condition ??= static _ => true;

            foreach(var item in span)
            {
                if(condition(item)) return item;
            }

            return default;
        }

        public static TResult[] Select<T, TResult>(this Span<T> span, Func<T, TResult> factory)
        {
            var result = new TResult[span.Length];
            for(int i = 0; i < span.Length; i++)
            {
                result[i] = factory(span[i]);
            }
            return result;
        }
    }
}
