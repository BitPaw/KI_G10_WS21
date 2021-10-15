/// <summary>
/// Evaluate a given car to determine a given Individual's specific fitness value.
/// 
/// Author: Sascha Schewe
/// </summary>
public interface IFitnessFunction {

    /// <summary>
    /// Determine the fitness of a given Car
    /// <seealso cref="CarState"/>
    /// </summary>
    /// <param name="state">The current state of a given car</param>
    /// <returns>The attached Individual's fitness value</returns>
    float DetermineFitness(CarState state);
}
