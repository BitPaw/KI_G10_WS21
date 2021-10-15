using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Recombiners take 2 parent sequences and combine them to create a new sequence, for example n-point crossover
/// </summary>
public interface IRecombiner {
    
    /// <summary>
    /// Shockingly, it should take 2 parents and generate a child sequence.
    /// </summary>
    /// <param name="parentA"></param>
    /// <param name="parentB"></param>
    /// <returns>An unholy abomination of genetics.</returns>
    string Combine(string parentA, string parentB);
}
