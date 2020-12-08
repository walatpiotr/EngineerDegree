using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTimer : MonoBehaviour
{
    public int number;

    public float green;
    public float yellow;
    public float red;
    public float tBlock;

    public float delay;

    //manually
    public int numberOfPath;
    //

    public List<GameObject> carsInMyPath;
    public string tagOfCars;

    private bool greenFlag = false;
    private bool yellowFlag = true;
    private bool redFlag = false;

    private bool wasGreen = false;

    private float timer = 0f;

    private BoxCollider collider;
    private MeshRenderer renderer;
    private CarSpawner carSpawner;

    private bool tryingToEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        LightsTimeParametrsHolder parent = transform.parent.GetComponent<LightsTimeParametrsHolder>();

        green = parent.GreenLightTime;
        yellow = parent.YellowLightTime;
        tBlock = parent.TBlockTime;

        AssignNumber();

        red = yellow * 4 + green * 2 + tBlock * 2;

        if (number == 1)
        {
            delay = parent.YellowLightTime + parent.GreenLightTime + parent.YellowLightTime + parent.TBlockTime;
            tagOfCars = "car1";
        }
        else if (number == 2)    
        {
            delay = 2 * (parent.YellowLightTime + parent.GreenLightTime + parent.YellowLightTime + parent.TBlockTime);
            tagOfCars = "car2";
        }
        else if (number == 3)
        {
            delay = 0f;
            tagOfCars = "car3";
        }

        timer = delay;

        collider = this.GetComponent<BoxCollider>();
        renderer = this.GetComponent<MeshRenderer>();
        GameObject spawner = GameObject.FindGameObjectWithTag("spawner");
        carSpawner = spawner.GetComponent<CarSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        carsInMyPath = carSpawner.carsInPaths[numberOfPath];

        ChangeLights();

        if (tryingToEnable)
        {
            TryToEnableCollider(true);
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
                TryToEnableCollider(false);
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
                TryToEnableCollider(true);
                timer = yellow;
                return;
            }
        }
    }

    void TryToEnableCollider(bool value)
    {
        if (value == false)
        {
            //Debug Renderer
            renderer.enabled = false;

            collider.enabled = false;

            foreach(GameObject car in carsInMyPath)
            {
                var engine = car.GetComponent<CarEngine>();
                engine.LightWantToStartCar = true;
            }
            //Debug.Log("starting all cars");
        }
        else
        {
            var transform = this.GetComponent<Transform>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

            foreach (var collider in colliders)
            {
                if(collider.tag == tagOfCars)
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
}
