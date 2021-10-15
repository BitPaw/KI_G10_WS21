using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneExecutor : MonoBehaviour {

    private int executionDuration = 20;
    private int executionDurationCounter = 0;

    private Individual individual;
    private Dictionary<char, IGene> genes;
    private int sequencePosition = 0;

    public Individual CurrentIndividual
    {
        get
        {
            return individual;
        }

        set
        {
            Reset();
            individual = value;
        }
    }

    public bool IsFinished
    {
        get
        {
            return (individual == null || sequencePosition >= individual.GeneSequence.Length);
        }
    }

    public void SetExecDuration(int duration)
    {
        executionDuration = duration;
    }

    public void AssignGene(IGene gene)
    {
        if (genes == null)
            genes = new Dictionary<char, IGene>();
        if (genes.ContainsKey(gene.ID))
            throw new System.Exception("Genes must have unique IDs/cannot be assigned twice");
        genes.Add(gene.ID, gene);
        gene.Controller = GetComponent<CarController>();
    }

    private void Reset()
    {
        sequencePosition = 0;
    }

    private void Awake()
    {
        genes = new Dictionary<char, IGene>();
    }

    void FixedUpdate()
    {        
        if (IsFinished)
            return;
        if (executionDurationCounter >= executionDuration)
            executionDurationCounter = 0;
        if (executionDurationCounter++ != 0)
            return;
        IGene gene;
        if(genes.TryGetValue(individual.GeneSequence[sequencePosition], out gene))
        {
            gene.Execute();
            sequencePosition++;
        }
        else
        {
            throw new System.Exception("Missing gene ID " + individual.GeneSequence[sequencePosition] + ", are all genes assigned?");
        }
    }

}
