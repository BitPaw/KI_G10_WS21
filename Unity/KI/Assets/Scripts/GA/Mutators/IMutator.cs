using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mutators are used to make small, usually random changes to a given gene sequence.
/// Think "radioactive spider".
/// 
/// Author: Sascha Schewe
/// </summary>
public interface IMutator {

    /// <summary>
    /// Assigns the geneIDs that can be used for mutations.
    /// </summary>
    /// <param name="ID"></param>
    void AssignGene(char ID);

    /// <summary>
    /// Mutates one sequence.
    /// </summary>
    /// <param name="original">The sequence to be mutated</param>
    /// <returns>Swamp Thing. Possibly. More likely something that's going to die from cancer after a short and unpleasant existence.
    /// which is not to imply that Swamp Thing's existence is particularly pleasant, but hey, no cancer!</returns>
    string Mutate(string original);
}
