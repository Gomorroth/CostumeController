using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace gomoru.su.CostumeController
{
    internal static class Instanciator
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> cache = new();

        public static T Create<T>() where T : class => Create(typeof(T)) as T;

        public static object Create(Type type)
        {
            return cache.GetOrAdd(type, static type =>
            {
                var method = new DynamicMethod("", typeof(object), Type.EmptyTypes);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<object>)) as Func<object>;
            })();
        }
    }
}
