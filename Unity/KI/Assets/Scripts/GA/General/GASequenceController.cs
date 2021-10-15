using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class GASequenceController : MonoBehaviour {

    private SetupScript setup;

    private string selectedInitializer, selectedMutator, selectedFitnessFunction, selectedRecombiner, selectedSelector, selectedTerminator;
    private List<string> selectedGenes;
    private int generationSize = 200;
    private int individualLength = 50;
    private int geneExecutionDuration = 20;

    private IInitializer init;
    private GenerationDB generations;
    private UIManager ui;
    private bool lockedInGenerationStep = false;

    private List<GameObject> cars;
    private Dictionary<GameObject, CarState> carStates;
    private Dictionary<GameObject, GeneExecutor> carExecutors;

    private IFitnessFunction fitness;
    private ITerminator terminator;
    private IMutator mutator;
    private ISelector selector;
    private IRecombiner recombiner;

    private GameObject individualDisplay;

    private Coroutine simulation, simulationStep;

    public int GenerationSize
    {
        get
        {
            return generationSize;
        }

        set
        {
            generationSize = value;
        }
    }

    public int IndividualLength
    {
        get
        {
            return individualLength;
        }

        set
        {
            individualLength = value;
        }
    }

    public int GeneExecutionDuration
    {
        get
        {
            return geneExecutionDuration;
        }

        set
        {
            geneExecutionDuration = value;
        }
    }

    public string SelectedInitializer
    {
        get
        {
            return selectedInitializer;
        }

        set
        {
            selectedInitializer = value;
        }
    }

    public string SelectedMutator
    {
        get
        {
            return selectedMutator;
        }

        set
        {
            selectedMutator = value;
        }
    }

    public string SelectedFitnessFunction
    {
        get
        {
            return selectedFitnessFunction;
        }

        set
        {
            selectedFitnessFunction = value;
        }
    }

    public string SelectedRecombiner
    {
        get
        {
            return selectedRecombiner;
        }

        set
        {
            selectedRecombiner = value;
        }
    }

    public string SelectedSelector
    {
        get
        {
            return selectedSelector;
        }

        set
        {
            selectedSelector = value;
        }
    }

    public string SelectedTerminator
    {
        get
        {
            return selectedTerminator;
        }

        set
        {
            selectedTerminator = value;
        }
    }

    private void Start()
    {
        individualDisplay = GameObject.Find("TopDisplay");
        individualDisplay.SetActive(false);
    }

    public void Launch () {
        if (generations != null)
            generations.Reset();
        if (setup == null)
            setup = GetComponent<SetupScript>();
        ui = GetComponent<UIManager>();
        cars= setup.Setup();
        carStates = new Dictionary<GameObject, CarState>();
        carExecutors = new Dictionary<GameObject, GeneExecutor>();
        generations = GetComponent<GenerationDB>();
        fitness = (IFitnessFunction)System.Activator.CreateInstance(setup.FitnessFunctions.Where(type => type.Name.Equals(selectedFitnessFunction)).First());
        terminator = (ITerminator)System.Activator.CreateInstance(setup.Terminators.Where(type => type.Name.Equals(selectedTerminator)).First());
        mutator = (IMutator)System.Activator.CreateInstance(setup.Mutators.Where(type => type.Name.Equals(selectedMutator)).First());
        foreach (string genetype in selectedGenes)
        {
            mutator.AssignGene(((IGene)System.Activator.CreateInstance(setup.GeneTypes.Where(type => type.Name.Equals(genetype)).First())).ID);
        }
        selector = (ISelector)System.Activator.CreateInstance(setup.Selectors.Where(type => type.Name.Equals(selectedSelector)).First());
        recombiner = (IRecombiner)System.Activator.CreateInstance(setup.Recombiners.Where(type => type.Name.Equals(selectedRecombiner)).First());
        init = (IInitializer)System.Activator.CreateInstance(setup.Initializers.Where(type => type.Name.Equals(selectedInitializer)).First());
        foreach (string genetype in selectedGenes)
        {
            init.AssignGene(((IGene)System.Activator.CreateInstance(setup.GeneTypes.Where(type => type.Name.Equals(genetype)).First())).ID);
        }
        
        SetupCars();
        simulation = StartCoroutine(FullSimulation());
	}

    private void CheckFitness()
    {
        foreach(GameObject car in cars)
        {
            if(carExecutors[car].CurrentIndividual !=null)
                carExecutors[car].CurrentIndividual.Fitness = fitness.DetermineFitness(carStates[car]);
        }
    }
    private bool CheckTerminator()
    {
        return terminator.JudgementDay(generations.CurrentGeneration);
    }

    private void SetupCars()
    {
        foreach (GameObject car in cars)
        {
            carStates.Add(car, car.GetComponent<CarState>());
            GeneExecutor exec = car.GetComponent<GeneExecutor>();
            carExecutors.Add(car, exec);
            foreach (string genetype in selectedGenes)
            {
                exec.AssignGene((IGene)System.Activator.CreateInstance(setup.GeneTypes.Where(type => type.Name.Equals(genetype)).First()));
                exec.SetExecDuration(geneExecutionDuration);
            }
        }
    }

    private void ResetCars()
    {
        foreach(CarState state in carStates.Values)
        {
            state.Reset();
        }
        foreach(GeneExecutor exec in carExecutors.Values)
        {
            exec.CurrentIndividual = null;
        }
    }

    private bool AssignIndividuals(ref int ptr)
    {
        Debug.Log("Assigning individuals from " + ptr);
        for(int i = 0; i< cars.Count && ptr<generations.CurrentGeneration.Individuals.Count; i++)
        {
            carExecutors[cars[i]].CurrentIndividual = generations.CurrentGeneration.Individuals[ptr++];
        }
        return ptr < generations.CurrentGeneration.Individuals.Count;
    }

    private List<string> MutateIndividuals(List<string> ind)
    {
        List<string> newInd = new List<string>();
        foreach(string individual in ind)
        {
            newInd.Add(mutator.Mutate(individual));
        }
        return newInd;
    }

    private IEnumerator GenerationStep()
    {
        lockedInGenerationStep = true;
        int ptr = 0;
        while(AssignIndividuals(ref ptr))
        {
            yield return new WaitUntil(() => carExecutors.Values.All(p => p.IsFinished));
            CheckFitness();
            ResetCars();
        }
        yield return new WaitUntil(() => carExecutors.Values.All(p => p.IsFinished));
        CheckFitness();
        ResetCars();
        lockedInGenerationStep = false;
    }

    private IEnumerator FullSimulation()
    {
        Debug.Log("Starting simulation");
        Debug.Log("Seeding initial generation");
        List<Individual> originalPop = init.CreateInitialGeneration(generationSize, individualLength);
        generations.CurrentGeneration.Add(originalPop);
        while (true)
        {
            Debug.Log("Simulating generation " + generations.GenerationCount);
            simulationStep = StartCoroutine(GenerationStep());
            while (lockedInGenerationStep)
            {
                yield return new WaitForSeconds(.1f);
            }
            generations.CurrentGeneration.Sort();
            if (CheckTerminator())
                break;
            Debug.Log("Selecting using " + selector.GetType().Name);
            List<string> parents = selector.SelectFromGeneration(generations.CurrentGeneration);
            List<string> children = new List<string>();
            Debug.Log("Recombining using " + recombiner.GetType().Name);
            for(int i = 0; i < parents.Count;)
            {
                children.Add(recombiner.Combine(parents[i++], parents[i++]));
            }
            Debug.Log("Mutating using " + mutator.GetType().Name);
            for(int i = 0; i < children.Count; i++)
            {
                children[i] = mutator.Mutate(children[i]);
            }
            generations.NewGeneration.Add(children);

        }
        Debug.Log("The end of days has arrived, kneel before your master and accept your DOOM!");

        generations.UpdateHighScoreList(generations.CurrentGeneration);
        Time.timeScale = 1;

        individualDisplay.SetActive(true);
        carExecutors[cars[0]].CurrentIndividual = generations.GetFittestIndividualFromHistory();
        for (int i = cars.Count-1; i > 0; i--)
        {
            GameObject car = cars[i];
            cars.Remove(car);
            carStates.Remove(car);
            carExecutors.Remove(car);
            Destroy(car);
        }
        while (true)
        {
            yield return new WaitUntil(() => carExecutors[cars[0]].IsFinished);
            yield return new WaitForSeconds(4f);
            carStates[cars[0]].Reset();
            carExecutors[cars[0]].CurrentIndividual = generations.GetFittestIndividualFromHistory();
        }
    }

    public void StopSimulation()
    {
        if (simulationStep != null)
            StopCoroutine(simulationStep);
        if (simulation != null)
            StopCoroutine(simulation);
        ResetCars();
        generations.Reset();
        ui.SetVisibility(true);
        individualDisplay.SetActive(false);
    }

    public void AddGeneType(string gene)
    {
        if (selectedGenes == null)
            selectedGenes = new List<string>();
        if (!selectedGenes.Contains(gene))
            selectedGenes.Add(gene);
    }

    public void ClearGeneTypes()
    {
        selectedGenes = null;
    }
}
