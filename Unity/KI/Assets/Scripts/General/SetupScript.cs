using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SetupScript : MonoBehaviour {

    public int InstanceCount = 50;
    private List<System.Type> geneTypes;
    private List<System.Type> initializers;
    private List<System.Type> fitnessFunctions;
    private List<System.Type> selectors;
    private List<System.Type> mutators;
    private List<System.Type> recombiners;
    private List<System.Type> terminators;

    public GameObject car;
    public Transform carContainer;
    private List<GameObject> cars;

    public List<Type> GeneTypes
    {
        get
        {
            if (geneTypes == null)
            {
                geneTypes = GetAllOfType(typeof(IGene));
            }
            return geneTypes;
        }
    }

    public List<Type> Initializers
    {
        get
        {
            if(initializers==null)
            {
                initializers = GetAllOfType(typeof(IInitializer));
            }
            return initializers;
        }
    }

    public List<Type> FitnessFunctions
    {
        get
        {
            if (fitnessFunctions == null)
            {
                fitnessFunctions = GetAllOfType(typeof(IFitnessFunction));
            }
            return fitnessFunctions;
        }
    }

    public List<Type> Selectors
    {
        get
        {
            if (selectors == null)
            {
                selectors = GetAllOfType(typeof(ISelector));
            }
            return selectors;
        }
    }

    public List<Type> Mutators
    {
        get
        {
            if (mutators == null)
            {
                mutators = GetAllOfType(typeof(IMutator));
            }
            return mutators;
        }
    }

    public List<Type> Recombiners
    {
        get
        {
            if (recombiners == null)
            {
                recombiners = GetAllOfType(typeof(IRecombiner));
            }
            return recombiners;
        }
    }

    public List<Type> Terminators
    {
        get
        {
            if (terminators == null)
            {
                terminators = GetAllOfType(typeof(ITerminator));
            }
            return terminators;
        }
    }
    

    public List<GameObject> Setup () {
        Physics.IgnoreLayerCollision(4,4,true);
        if (cars != null)
        {
            for(int i = 0; i < cars.Count; i++)
            {
                Destroy(cars[i]);
            }
        }
        cars = new List<GameObject>();
		for(int i = 0; i < InstanceCount; i++)
        {
            GameObject newCar = GameObject.Instantiate(car);
            newCar.name = "Car_" + i;
            newCar.transform.parent = carContainer;
            cars.Add(newCar);
        }
        return cars;
	}

    private List<System.Type> GetAllOfType(System.Type type)
    {
        List<System.Type> typeList = new List<System.Type>(System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                        .Where(p => type.IsAssignableFrom(p) && p.IsClass).OrderBy(o => o.Name).ToList());
        if (typeList.Count < 1)
            Debug.LogWarning("Can't find classes that implement " + type.ToString());
        return typeList;
    }

}
