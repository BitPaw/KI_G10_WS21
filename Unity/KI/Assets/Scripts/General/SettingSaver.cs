using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;

public class SettingSaver : MonoBehaviour {

    private string saveFile = "settings.xml";
    private SettingContainer currentSettings;

    public SettingContainer GetSettings()
    {
        if (currentSettings == null)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingContainer));
                FileStream stream = new FileStream(Path.Combine(Application.dataPath, saveFile), FileMode.Open);
                SettingContainer container = serializer.Deserialize(stream) as SettingContainer;
                stream.Close();
                currentSettings = container;
                if (!currentSettings.Verify(GetComponent<SetupScript>()))
                {
                    Debug.Log("Settings incompatible, generating new Settings");
                    currentSettings = new SettingContainer(GetComponent<SetupScript>());
                }
            }
            catch (System.Exception)
            {
                currentSettings = new SettingContainer(GetComponent<SetupScript>());
            }
        }
        currentSettings.FileLocation = saveFile;
        return currentSettings;
    }

    public void SaveSettings()
    {
        if (currentSettings == null)
        {
            GetSettings();
        }
        currentSettings.FileLocation = saveFile;
        currentSettings.Save();
    }

    [XmlRoot("Settings")]
    public class SettingContainer
    {
        public string FileLocation;
//        [XmlArray("Genes")]
//        [XmlArrayItem("Gene")]
        public List<string> Genes;
//        [XmlArrayItem("Setting")]
        public List<string> SelectedGenes;
//        [XmlArray("Initializers")]
//        [XmlArrayItem("Initializer")]
        public List<string> Initializers;
//        [XmlArrayItem("SelectedInitializer")]
        public string SelectedInitializer;
//        [XmlArray("FitnessFunctions")]
//        [XmlArrayItem("FitnessFunctions")]
        public List<string> FitnessFunctions;
//        [XmlArrayItem("SelectedFitnessFunction")]
        public string SelectedFitnessFunction;
//        [XmlArray("Selectors")]
//        [XmlArrayItem("Selector")]
        public List<string> Selectors;
//        [XmlArrayItem("SelectedSelector")]
        public string SelectedSelector;
//        [XmlArray("Recombiners")]
//        [XmlArrayItem("Recombiner")]
        public List<string> Recombiners;
//        [XmlArrayItem("SelectedRecombiner")]
        public string SelectedRecombiner;
//        [XmlArray("Mutators")]
//        [XmlArrayItem("Mutator")]
        public List<string> Mutators;
//        [XmlArrayItem("SelectedMutator")]
        public string SelectedMutator;
//        [XmlArray("Terminators")]
//        [XmlArrayItem("Terminator")]
        public List<string> Terminators;
//        [XmlArrayItem("SelectedTerminator")]
        public string SelectedTerminator;

        [XmlAttribute("CarInstances")]
        public int CarInstances;
        [XmlAttribute("GenerationSize")]
        public int GenerationSize;
        [XmlAttribute("SequenceLength")]
        public int SequenceLength;
        [XmlAttribute("GeneDuration")]
        public int GeneDuration;

        private SettingContainer()
        {

        }

        public SettingContainer(SetupScript setup)
        {
            Genes = setup.GeneTypes.Select(x => x.Name).ToList();
            SelectedGenes = setup.GeneTypes.Select(x => x.Name).ToList();
            Initializers = setup.Initializers.Select(x => x.Name).ToList();
            FitnessFunctions = setup.FitnessFunctions.Select(x => x.Name).ToList();
            Selectors = setup.Selectors.Select(x => x.Name).ToList();
            Recombiners = setup.Recombiners.Select(x => x.Name).ToList();
            Mutators = setup.Mutators.Select(x => x.Name).ToList();
            Terminators = setup.Terminators.Select(x => x.Name).ToList();
            SelectedInitializer = Initializers.First();
            SelectedFitnessFunction = FitnessFunctions.First();
            SelectedSelector = Selectors.First();
            SelectedRecombiner = Recombiners.First();
            SelectedMutator = Mutators.First();
            SelectedTerminator = Terminators.First();
            CarInstances = 50;
            GenerationSize = 200;
            SequenceLength = 10;
            GeneDuration = 10;
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingContainer));
            FileStream stream = new FileStream(Path.Combine(Application.dataPath, FileLocation), FileMode.Create);
            serializer.Serialize(stream, this);
            stream.Close();
        }

        public bool Verify(SetupScript setup)
        {
            if (!VerifyListEquality(setup.GeneTypes.Select(x => x.Name).ToList(), Genes))
                return false;
            if (!VerifyListEquality(setup.Initializers.Select(x => x.Name).ToList(), Initializers))
                return false;
            if (!VerifyListEquality(setup.FitnessFunctions.Select(x => x.Name).ToList(), FitnessFunctions))
                return false;
            if (!VerifyListEquality(setup.Selectors.Select(x => x.Name).ToList(), Selectors))
                return false;
            if (!VerifyListEquality(setup.Recombiners.Select(x => x.Name).ToList(), Recombiners))
                return false;
            if (!VerifyListEquality(setup.Mutators.Select(x => x.Name).ToList(), Mutators))
                return false;
            if (!VerifyListEquality(setup.Terminators.Select(x => x.Name).ToList(), Terminators))
                return false;

            Debug.Log("Settings verified");
            return true;
        }

        private bool VerifyListEquality(List<string> a, List<string> b)
        {
            foreach (string element in a)
            {
                if (!b.Contains(element))
                    return false;
            }
            foreach (string element in b)
            {
                if (!a.Contains(element))
                    return false;
            }
            return true;
        }
    }

}
