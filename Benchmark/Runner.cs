using System;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    public class Runner
    {
        const int ElementAmount = 10000;

        private readonly double[] _doubleCollection;
        private readonly float[] _floatCollection;
        private readonly Ignore[] _ignoreCollection;
        private readonly bool[] _results;

        public Runner()
        {
            var random = new Random();

            _doubleCollection = new double[ElementAmount];
            _floatCollection = new float[ElementAmount];
            _ignoreCollection = new Ignore[ElementAmount];
            _results = new bool[ElementAmount];

            for (int i = 0; i < ElementAmount; i++)
            {
                var randomValue = random.NextDouble();
                if (randomValue < 0.3)
                {
                    randomValue = double.NaN;
                }

                if (randomValue > 0.7)
                {
                    randomValue = double.PositiveInfinity;
                }

                _doubleCollection[i] = randomValue;
                _floatCollection[i] = Convert(randomValue);

                _ignoreCollection[i] = (Ignore)random.Next(2);
            }
        }

        private float Convert(double value)
        {
            if (double.IsNaN(value))
            {
                return float.NaN;
            }

            if (double.IsInfinity(value))
            {
                return float.PositiveInfinity;
            }

            return System.Convert.ToSingle(value);
        }

        [Benchmark]
        public void Generic()
        {
            for (int i = 0; i < ElementAmount; i++)
            {
                _results[i] = _ignoreCollection[i].IgnoreValueGeneric(_doubleCollection[i]);
            }

            for (int i = 0; i < ElementAmount; i++)
            {
                _results[i] = _ignoreCollection[i].IgnoreValueGeneric(_floatCollection[i]);
            }
        }

        [Benchmark]
        public void Native()
        {
            for (int i = 0; i < ElementAmount; i++)
            {
                _results[i] = _ignoreCollection[i].IgnoreValue(_doubleCollection[i]);
            }

            for (int i = 0; i < ElementAmount; i++)
            {
                _results[i] = _ignoreCollection[i].IgnoreValue(_floatCollection[i]);
            }
        }
    }
}