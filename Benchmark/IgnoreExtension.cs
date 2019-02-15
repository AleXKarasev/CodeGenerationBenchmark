using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Benchmark
{
    public static class IgnoreExtension
    {
        public static bool IgnoreValueGeneric<T>(this Ignore ignore, T value) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                var floatValue = Converter<T, float>.Convert(value);
                return ignore.IgnoreValue(floatValue);
            }

            if (typeof(T) == typeof(double))
            {
                var doubleValue = Converter<T, double>.Convert(value);
                return ignore.IgnoreValue(doubleValue);
            }

            throw new ArgumentException($"Type {typeof(T)} does not supported!");
        }

        public static bool IgnoreValue(this Ignore ignore, float value)
        {
            switch (ignore)
            {
                case Ignore.Nothing:
                    return false;
                case Ignore.NaNs:
                    return float.IsNaN(value);
                case Ignore.NaNsAndInfs:
                    return float.IsNaN(value) || float.IsInfinity(value);
                default:
                    throw new NotImplementedException($"Function is not implemented for ignore case '{ignore}'");
            }
        }

        public static bool IgnoreValue(this Ignore ignore, double value)
        {
            switch (ignore)
            {
                case Ignore.Nothing:
                    return false;
                case Ignore.NaNs:
                    return double.IsNaN(value);
                case Ignore.NaNsAndInfs:
                    return double.IsNaN(value) || double.IsInfinity(value);
                default:
                    throw new NotImplementedException($"Function is not implemented for ignore case '{ignore}'");
            }
        }

        private static class Converter<TIn, TOut>
            where TIn : struct
            where TOut : struct
        {
            private static readonly Func<TIn, TOut> ConverterMethod;

            static Converter()
            {
                ConverterMethod = EmitConverter();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static TOut Convert(TIn value)
            {
                return ConverterMethod(value);
            }

            private static Func<TIn, TOut> EmitConverter()
            {
                var method = new DynamicMethod(string.Empty, typeof(TOut), new[] { typeof(TIn) });
                var il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);

                var newConverter = method.CreateDelegate(typeof(Func<TIn, TOut>));

                return (Func<TIn, TOut>)newConverter;
            }
        }
    }
}