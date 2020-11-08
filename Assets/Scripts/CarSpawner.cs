﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public float percentageOfBigCars;
    public GameObject smallCar;
    public GameObject bigCar;
    public List<GameObject> paths;
    public float spawnTime = 3.0f;

    public Dictionary<int, List<GameObject>> carsInPaths = new Dictionary<int, List<GameObject>> { };
    public Dictionary<int, List<GameObject>> nodesInPaths = new Dictionary<int, List<GameObject>> { };

    private int thisNumber = 0;

    void Start()
    {
        int i = 0;
        foreach (GameObject path in paths)
        {
            carsInPaths.Add(i, new List<GameObject> { });
            i++;
        }
        StartCoroutine(SpawnSetup());
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
                CarAddAndSetup(component, nodes, car);
                i++;
            }
        }
    }

    private void Spawn()
    {
        GameObject car = ChooseCarType();
        var component = car.GetComponent<CarEngine>();

        var nodes = GetRandomPath();

        if (!carsInPaths[thisNumber].Any())
        {
            CarAddAndSetup(component, nodes, car);
        }
        else
        {
            var flag = true;
            while (flag)
            {
                if (carsInPaths[thisNumber].Any())
                {
                    foreach (GameObject _car in carsInPaths[thisNumber])
                    {
                        var distance = Vector3.Distance(_car.transform.position, nodes.First().position);
                        Debug.Log(distance);
                        if ( distance > 100.0f)
                        {
                            flag = false;
                            CarAddAndSetup(component, nodes, car);
                            break;
                        }
                        else
                        {
                            nodes = GetRandomPath();
                        }
                    }
                }
                else
                {
                    CarAddAndSetup(component, nodes, car);
                }
            }
        }
    }

    private IEnumerator carSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            Spawn();
        }
    }

    private List<Transform> GetRandomPath()
    {
        thisNumber = UnityEngine.Random.Range(0, paths.Count);
        var path = paths[thisNumber];

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

    private void CarAddAndSetup(CarEngine component, List<Transform> nodes, GameObject car)
    {
        //Adding spawner object to car and number of path and list of nodes in path - deleting object need those properties
        component.spawner = this;
        component.pathNumber = thisNumber;
        component.nodes = nodes;

        //Upping first node and a car a little bit when spawn
        var node = nodes.First().position;
        node.y = 0.2f;
        car.transform.position = node;

        //Rotate toward second node in path
        Vector3 toward = nodes[1].position;

        Debug.DrawLine(car.transform.position, toward);

        car.transform.rotation = Quaternion.LookRotation(toward);

        //Adding car to path list of cars
        carsInPaths[thisNumber].Add(car);
    }

    private GameObject ChooseCarType()
    {
        GameObject car;
        var randomInt = Random.Range(0, 100);
        if(randomInt < percentageOfBigCars)
        {
            car = Instantiate(bigCar) as GameObject;
        }
        else{
            car = Instantiate(smallCar) as GameObject;
        }
        return car;
    }
}
