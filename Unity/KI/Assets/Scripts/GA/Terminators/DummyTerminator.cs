using UnityEngine;

public class DummyTerminator : ITerminator
{
    private float chucksWrath = .2f;

    public bool JudgementDay(GenerationDB.Generation generation)
    {
        if (Random.value < chucksWrath)
        {
            Debug.Log("The end is now.");
            return true;
        }
        else
        {
            Debug.Log("The end is nigh.");
            return false;
        }
    }
}
