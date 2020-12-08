﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public float percentageOfBigCars;
    public GameObject smallCar;
    public GameObject bigCar;
    public List<GameObject> paths;
    public float spawnTime = 1.0f;
    public Dictionary<int, List<GameObject>> carsInPaths = new Dictionary<int, List<GameObject>> { };
    public Dictionary<int, List<GameObject>> nodesInPaths = new Dictionary<int, List<GameObject>> { };

    [Serializable]
    public struct NamedImage
    {
        public int number;
        public List<GameObject> cars;
    }

    public List<NamedImage> allCars;

    public List<string> wallTags;
    public List<string> carTags;

    private bool canUpdate = false;

    void Start()
    {
        wallTags = new List<string>() { "redlightwall3", "redlightwall2", "redlightwall1" };
        carTags = new List<string>() { "car1", "car2", "car3" };
        int i = 0;
        foreach (GameObject path in paths)
        {
            carsInPaths.Add(i, new List<GameObject> { });
            i++;
        }
        StartCoroutine(SpawnSetup());
        canUpdate = true;
    }

    void Update()
    {
        if (canUpdate)
        {
            allCars = new List<NamedImage>();
            foreach (var kvp in carsInPaths)
            {
                var x = new NamedImage() { number = kvp.Key, cars = kvp.Value};
                allCars.Add(x);
            }
        }
    }

    private IEnumerator SpawnSetup()
    {
        while (true)
        {
            int i = 0;
            foreach (KeyValuePair<int, List<GameObject>> pair in carsInPaths)
            {
                yield return new WaitForSeconds(spawnTime);
                var nodes = GetCertainPath(i);
                GameObject car = ChooseCarType();
                var component = car.GetComponent<CarEngine>();
                CarAddAndSetup(component, nodes, car, i);
                i++;
                if (i > paths.Count)
                {
                    break;
                }
            }
        }
    }

    private List<Transform> GetCertainPath(int num)
    {
        var path = paths[num];

        Transform[] transforms = path.GetComponentsInChildren<Transform>();
        var nodes = new List<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != path.transform)
            {
                nodes.Add(transforms[i]);
            }
        }

        return nodes;
    }

    private void CarAddAndSetup(CarEngine component, List<Transform> nodes, GameObject car, int numberOfPath)
    {
        if (CheckCarsPositions(nodes.First().position))
        {
            //Upping first node and a car a little bit when spawn
            var node = nodes.First().position;
            node.y = 0.2f;
            car.transform.position = node;

            //Adding spawner object to car and number of path and list of nodes in path - deleting object need those properties
            component.spawner = this;
            component.pathNumber = numberOfPath;
            component.nodes = nodes;
            

            if (numberOfPath < 4)
            {
                component.wallTagToAvoid = wallTags[0];
                component.carTagToAvoid = carTags[0];
            }
            if (numberOfPath < 8 && numberOfPath > 3 )
            {
                component.wallTagToAvoid = wallTags[1];
                component.carTagToAvoid = carTags[1];
            }
            if (7 < numberOfPath && numberOfPath < 10)
            {
                component.wallTagToAvoid = wallTags[2];
                component.carTagToAvoid = carTags[2];
            }

            //Adding car to path list of cars
            carsInPaths[numberOfPath].Add(car);
            Instantiate(car);
        }
        else
        {
            Debug.Log("Could not spawn on path" + nodes.First().position);
        }
    }

    private bool CheckCarsPositions(Vector3 position)
    {
        var cars = GameObject.FindGameObjectsWithTag("car");
        foreach(GameObject car in cars)
        {
            if(Vector3.Distance(position, car.transform.position) < 2f)
            {
                return false;
            }
        }
        return true;
    }

    private GameObject ChooseCarType()
    {
        GameObject car;
        var randomInt = UnityEngine.Random.Range(0, 100);
        if(randomInt < percentageOfBigCars)
        {
            car = bigCar as GameObject;
        }
        else{
            car = smallCar as GameObject;
        }
        return car;
    }
}
