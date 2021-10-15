using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The terminator should determine whether the termination conditions of our GA are met.
/// Author: Sascha Schewe
/// </summary>
public interface ITerminator {

    /// <summary>
    /// *DaDaDamm-DaDamm... DaDaDamm-DaDamm*... Are our conditions for terminating the GA met (Generation count, current max fitness etc)?
    /// The GA will terminate if this returns true.
    /// (In case you're an uncultured, unwashed phillistine: <a href="https://www.youtube.com/watch?v=-W8CegO_Ixw">TRUE ART</link>)
    /// </summary>
    /// <param name="generation">the current generation... Maybe... THE LAST GENERATION!</param>
    /// <returns>whether the end is nigh</returns>
    bool JudgementDay(GenerationDB.Generation generation);
}
