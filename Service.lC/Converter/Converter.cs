using System;
using System.Collections.Generic;

namespace Service.lC
{
    internal static class Converter
    {
        private static Dictionary<Tuple<Type, Type>, object> dictionary = new Dictionary<Tuple<Type, Type>, object>();

        public static void Register<TSource, TResult>(Func<TSource, TResult> func)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TResult);

            dictionary.Add(new Tuple<Type, Type>(sourceType, destinationType), func);
        }

        public static Func<TSource, TResult> GetConverter<TSource, TResult>() where TSource : new() where TResult : new()
        {

            var sourceType = typeof(TSource);
            var destinationType = typeof(TResult);

            var key = new Tuple<Type, Type>(sourceType, destinationType);

            object value = null;

            try
            {
                value = dictionary[key];
            }
            catch (KeyNotFoundException ex)
            {
                //Init Type static constructor (Register converter)
                var source = new TSource();
                var destination = new TResult();

                value = dictionary[key];
            }           

            var func = value as Func<TSource, TResult>;

            return func;
        }

    }
}
