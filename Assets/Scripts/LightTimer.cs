using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightTimer : MonoBehaviour
{
    public bool realisticSUT = false;
    public SUTGeneration generator;

    public int number;

    public float green;
    public float yellow;
    public float red;
    public float tBlock;

    public float delay;

    //manually
    public List<int> numberOfPath;
    //

    public List<GameObject> carsInMyPath;
    public List<string> tagsOfCars;

    private bool greenFlag = false;
    private bool yellowFlag = true;
    private bool redFlag = false;

    private bool wasGreen = false;

    public float timer = 0f;

    private BoxCollider collider;
    private MeshRenderer renderer;
    public CarSpawner carSpawner;

    public bool tryingToEnable = false;
    public bool startingToSetup = false;

    //For purpose of finding cars nearest when red light


    private void Awake()
    {
        AssignNumber();

        red = yellow * 4 + green * 2 + tBlock * 2;

        TimerSetup();

        collider = this.GetComponent<BoxCollider>();
        renderer = this.GetComponent<MeshRenderer>();
        try
        {
            GameObject spawner = GameObject.FindGameObjectWithTag("spawner");
            carSpawner = spawner.GetComponent<CarSpawner>();
            TagsOfCarsSetup();
        }
        catch (Exception e)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        carsInMyPath = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       
        timer -= Time.deltaTime;

        CheckCarsInPaths();

        ChangeLights();

        if (tryingToEnable)
        {
            TryToEnableCollider(true);
        }
    }

    void CheckCarsInPaths()
    {
        var carListTemp = new List<GameObject>();

        foreach (var paths in numberOfPath)
        {
            carListTemp.AddRange(carSpawner.carsInPaths[paths]);
        }
        carsInMyPath = carListTemp;
    }

    void TimerSetup()
    {
        if (number == 1)
        {
            delay = yellow + green + yellow + tBlock;
        }
        else if (number == 2)
        {
            delay = 2 * (yellow + green + yellow + tBlock);
        }
        else if (number == 3)
        {
            delay = 0f;
        }

        timer = delay;
    }

    void TagsOfCarsSetup()
    {
        foreach(int number in numberOfPath)
        {
            tagsOfCars.Add(carSpawner.carTags[number]);
        }
    }

    void ChangeLights()
    {
        if (timer <= 0)
        {
            if (greenFlag) // Green to yellow
            {
                greenFlag = false;
                yellowFlag = true;
                wasGreen = true;
                TryToEnableCollider(true);
                timer = yellow;
                return;
            }
            if (yellowFlag) // Yellow to something
            {
                if (wasGreen == true) // Yellow to Red
                {
                    redFlag = true;
                    yellowFlag = false;
                    wasGreen = false;
                    TryToEnableCollider(true);
                    timer = red;
                    return;
                }
                if (wasGreen == false) // Yellow to Green
                {
                    greenFlag = true;
                    yellowFlag = false;
                    wasGreen = true;
                    SetSUTsForTwoNearestCars();
                    TryToEnableCollider(false);
                    timer = green;
                    return;
                }
            }
            if (redFlag) // Red to yellow
            {
                redFlag = false;
                yellowFlag = true;
                wasGreen = false;
                TryToEnableCollider(false);
                timer = yellow;
                return;
            }
        }
    }

    void TryToEnableCollider(bool value)
    {
        if (value == false)
        {
            tryingToEnable = false;

            //Debug Renderer
            renderer.enabled = false;
            collider.enabled = false;
            
            foreach (GameObject car in carsInMyPath)
            {
                var engine = car.GetComponent<CarEngine>();
                engine.LightsTurnToGreen();
            }
            
            //Debug.Log("starting all cars");
        }
        else
        {
            tryingToEnable = false;

            var transform = this.GetComponent<Transform>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

            foreach (var collider in colliders)
            {
                if(tagsOfCars.Contains(collider.tag))
                {
                    tryingToEnable = true;

                    //Debug
                    var carPosition = collider.GetComponent<Transform>().position;
                    //Debug.Log("Can't spawn beacause of some car" + numberOfPath);
                }
            }
            if(!tryingToEnable)
            {
                //Debug Renderer
                renderer.enabled = true;

                collider.enabled = true;
            }
        }
    }

    void AssignNumber()
    {
        if(this.tag == "redlightwall1")
        {
            number = 1;
        }
        else if (this.tag == "redlightwall2")
        {
            number = 2;
        }
        else if (this.tag == "redlightwall3")
        {
            number = 3;
        }
    }

    void SetSUTsForTwoNearestCars()
    {
        if (realisticSUT && carsInMyPath.Count!=0)
        {
            SetUpCars();
        }
    }

    void SetUpCars()
    {
        carsInMyPath.Sort(CompareCarsByDistance);


        var firstCar = carsInMyPath[0];
        var secondCar = carsInMyPath[1];
        Debug.Log(firstCar + " : " + secondCar);
        generator.SetUpValuesForFirstTwoCars(firstCar, secondCar);
    }

    private int CompareCarsByDistance(GameObject x, GameObject y)
    {
        Vector3 currentPos = transform.position;
        float xDistance = Vector3.Distance(x.transform.position, currentPos);
        float yDistance = Vector3.Distance(y.transform.position, currentPos);

        if (xDistance > yDistance)
        {
            return 1;
        }
        if (yDistance > xDistance)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
