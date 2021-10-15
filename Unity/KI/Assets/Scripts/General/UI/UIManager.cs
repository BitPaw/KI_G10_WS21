using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject Toggle;
    public GameObject RadioButton;
    public Transform MenuMover;
    private Vector3 TargetLocation;
    private Vector3 OriginalLocation;
    public Transform GeneContent, InitializerContent, FitnessContent, SelectorContent, RecombinerContent, MutatorContent, TerminatorContent;
    private ToggleGroup InitializerGroup, FitnessGroup, SelectorGroup, RecombinerGroup, MutatorGroup, TerminatorGroup;
    private List<Toggle> GeneToggles, InitializerToggles, FitnessToggles, SelectorToggles, RecombinerToggles, MutatorToggles, TerminatorToggles;
    public Text CarInstances, GenerationSize, SequenceLength, GeneDuration, CarInstancesBG, GenerationSizeBG, SequenceLengthBG, GeneDurationBG;
    private GASequenceController controller;
    private SettingSaver settings;
    private SettingSaver.SettingContainer container;

    private bool currentVisibility = true;

	// Use this for initialization
	void Start () {
        settings = GetComponent<SettingSaver>();
        container = settings.GetSettings();
        controller = GetComponent<GASequenceController>();
        GeneToggles = GenerateToggles(container.Genes, null, GeneContent,container.SelectedGenes);
        InitializerGroup = InitializerContent.gameObject.AddComponent<ToggleGroup>();
        InitializerToggles = GenerateToggles(container.Initializers, InitializerGroup, InitializerContent, container.SelectedInitializer);
        FitnessGroup = FitnessContent.gameObject.AddComponent<ToggleGroup>();
        FitnessToggles = GenerateToggles(container.FitnessFunctions, FitnessGroup, FitnessContent,container.SelectedFitnessFunction);
        SelectorGroup = SelectorContent.gameObject.AddComponent<ToggleGroup>();
        SelectorToggles = GenerateToggles(container.Selectors, SelectorGroup, SelectorContent,container.SelectedSelector);
        RecombinerGroup = RecombinerContent.gameObject.AddComponent<ToggleGroup>();
        RecombinerToggles = GenerateToggles(container.Recombiners, RecombinerGroup, RecombinerContent, container.SelectedRecombiner);
        MutatorGroup = MutatorContent.gameObject.AddComponent<ToggleGroup>();
        MutatorToggles = GenerateToggles(container.Mutators, MutatorGroup, MutatorContent, container.SelectedMutator);
        TerminatorGroup = TerminatorContent.gameObject.AddComponent<ToggleGroup>();
        TerminatorToggles = GenerateToggles(container.Terminators, TerminatorGroup, TerminatorContent,container.SelectedTerminator);
        CarInstancesBG.text = container.CarInstances.ToString();
        GenerationSizeBG.text = container.GenerationSize.ToString();
        SequenceLengthBG.text = container.SequenceLength.ToString();
        GeneDurationBG.text = container.GeneDuration.ToString();

        TargetLocation = MenuMover.position;
        OriginalLocation = MenuMover.position;
    }

    void Update()
    {
        if ((MenuMover.position - TargetLocation).sqrMagnitude > 1)
        {
            MenuMover.position = Vector3.Lerp(MenuMover.position, TargetLocation, Time.deltaTime * 5);
        }
    }

    public void Launch()
    {
        foreach (Toggle toggle in InitializerToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedInitializer = toggle.name;
                controller.SelectedInitializer = toggle.name;
                break;
            }
        }
        foreach (Toggle toggle in FitnessToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedFitnessFunction = toggle.name;
                controller.SelectedFitnessFunction = toggle.name;
                break;
            }
        }
        foreach (Toggle toggle in SelectorToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedSelector = toggle.name;
                controller.SelectedSelector = toggle.name;
                break;
            }
        }
        foreach (Toggle toggle in RecombinerToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedRecombiner = toggle.name;
                controller.SelectedRecombiner = toggle.name;
                break;
            }
        }
        foreach (Toggle toggle in MutatorToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedMutator = toggle.name;
                controller.SelectedMutator = toggle.name;
                break;
            }
        }
        foreach (Toggle toggle in TerminatorToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedTerminator = toggle.name;
                controller.SelectedTerminator = toggle.name;
                break;
            }
        }
        controller.ClearGeneTypes();
        container.SelectedGenes.Clear();
        foreach (Toggle toggle in GeneToggles)
        {
            if (toggle.isOn)
            {
                container.SelectedGenes.Add(toggle.name);
                controller.AddGeneType(toggle.name);
            }
        }
        if(CarInstances.text != "")
            container.CarInstances = int.Parse(CarInstances.text);
        if (GenerationSize.text != "")
            container.GenerationSize = int.Parse(GenerationSize.text);
        if (SequenceLength.text != "")
            container.SequenceLength = int.Parse(SequenceLength.text);
        if (GeneDuration.text != "")
            container.GeneDuration = int.Parse(GeneDuration.text);
        controller.GenerationSize = container.GenerationSize;
        controller.IndividualLength = container.SequenceLength;
        controller.GeneExecutionDuration = container.GeneDuration;
        GetComponent<SetupScript>().InstanceCount = container.CarInstances;
        container.Save();
        controller.Launch();
        SetVisibility(false);
    }

    private List<Toggle> GenerateToggles(List<string> items, ToggleGroup group, Transform parent, List<string> SelectedItems)
    {
        List<Toggle> toggles = new List<Toggle>();
        foreach (string item in items)
        {
            GameObject t;
            if (group==null)
                t = Instantiate(Toggle);
            else
                t = Instantiate(RadioButton);
            t.transform.SetParent(parent);
            Toggle newToggle = t.GetComponent<Toggle>();
            newToggle.group = group;
            newToggle.GetComponentInChildren<Text>().text = item;
            newToggle.name = item;
            newToggle.isOn = SelectedItems.Contains(item);
            toggles.Add(newToggle);
        }
        parent.parent.parent.GetComponent<ScrollRect>();
        return toggles;
    }

    private List<Toggle> GenerateToggles(List<string> items, ToggleGroup group, Transform parent, string SelectedItem)
    {
        List<Toggle> toggles = new List<Toggle>();
        foreach (string item in items)
        {
            GameObject t;
            if (group == null)
                t = Instantiate(Toggle);
            else
                t = Instantiate(RadioButton);
            t.transform.SetParent(parent);
            Toggle newToggle = t.GetComponent<Toggle>();
            newToggle.group = group;
            newToggle.GetComponentInChildren<Text>().text = item;
            newToggle.name = item;
            newToggle.isOn = false;
            newToggle.isOn = SelectedItem.Equals(item);
            toggles.Add(newToggle);
        }

        return toggles;
    }
    public void SetVisibility(bool visibility)
    {
        if (currentVisibility == visibility)
            return;
        currentVisibility = visibility;
        if (!visibility)
        {
            TargetLocation = OriginalLocation + (Vector3.up * Screen.height);
        }
        else
        {
            TargetLocation = OriginalLocation;
        }
    }
}
