using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Initializers are used to generate the first generation of Individuals to be operated on.
/// 
/// Author: Sascha Schewe
/// </summary>
public interface IInitializer {

    /// <summary>
    /// Assigns the gene IDs that are to be used to build gene sequences their IDs should make up Individuals' gene sequences.
    /// </summary>
    /// <param name="ID"></param>
    void AssignGene(char ID);
    /// <summary>
    /// Creates an INITIAL generation of the given size, with individuals of the given sequence length.
    /// <seealso cref="Individual"/>
    /// </summary>
    /// <param name="generationSize">Number of Individuals in a generation</param>
    /// <param name="individualSize">Length of gene sequence every individual should have</param>
    /// <returns>A list to be turned into the initial Generation</returns>
    List<Individual> CreateInitialGeneration(int generationSize, int individualSize);
}
