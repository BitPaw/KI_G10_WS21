using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for selectors, selectors select the individuals from an extant generation to procreate/recombine/mutate
/// </summary>
public interface ISelector {

    /// <summary>
    /// Generate a list of parents to recombine, [0] will be combined with [1], [2] with [3] etc.
    /// This list should be TWICE the size of the desired future generation!
    /// </summary>
    /// <param name="parentGeneration">the previous generation to be selected from</param>
    /// <returns>The gene sequences to be recombined</returns>
    List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration); 
}
