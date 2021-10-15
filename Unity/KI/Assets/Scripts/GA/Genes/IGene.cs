
/// <summary>
/// Genes are assigned to a specific car and should execute small actions on it (such as applying motor torque, steering brakes) when called.
/// 
/// Author: Sascha Schewe
/// </summary>
public interface IGene
{

    /// <summary>
    /// Called every FixedUpdate that the given Gene is active, should make some call to its assigned CarController.
    /// Example: 
    /// public void Execute()
    /// {
    ///     controller.ApplyBrakes();
    /// }
    /// Would apply brakes on this car for one FixedUpate.
    /// </summary>
    void Execute();
    /// <summary>
    /// ID must be a unique char.
    /// </summary>
    char ID { get; }
    /// <summary>
    /// Set on initial setup, store to use it.
    /// </summary>
    CarController Controller { get; set; }
}
