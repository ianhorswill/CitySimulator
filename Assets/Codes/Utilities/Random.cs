using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Random number generator
/// </summary>
public static class Random
{
    private static int seed = Environment.TickCount;

    // Logger.Log("seed", seed.ToString());
    public static System.Random RandomNumberGenerator = new System.Random(seed);

    /// <summary>
    /// Returns a random integer in the rang [min, max).
    /// </summary>
    /// <param name="min">Smallest allowable value</param>
    /// <param name="max">Maximum allowable value plus one</param>
    /// <returns></returns>
    public static int Integer(int min, int max)
    {
        return RandomNumberGenerator.Next(min, max);
    }

    public static int Integer(int max)
    {
        return RandomNumberGenerator.Next(max);
    }

    /// <summary>
    /// Returns a random float in the range [min, max]
    /// </summary>
    /// <param name="min">Smallest allowable value</param>
    /// <param name="max">Largest allowable value</param>
    /// <returns>Random value</returns>
    public static float Float(float min, float max)
    {
        return (float)RandomNumberGenerator.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Returns a random element of list
    /// </summary>
    /// <typeparam name="T">Type of list element</typeparam>
    /// <param name="list">List to choose from</param>
    public static T RandomElement<T>(this IList<T> list)
    {
        return list[RandomNumberGenerator.Next(list.Count)];
    }
}
