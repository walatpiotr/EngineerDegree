using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public bool realisticSUT = false;
    public SUTGeneration generator;

    public Transform path;
    public string typeOfCar;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public float currentSpeed;
    public float maxSpeed;
    public float maxStearingAngle;
    public float maxWheelTorque;
    public float maxBreakTorque;

    public bool isBraking;

    public CarSpawner spawner;
    public int pathNumber;
    public List<Transform> nodes;
    public List<string> carTagsToAvoid;
    public string wallTagToAvoid;
    public GlobalTimerScript globalTimer;

    private int currentNode = 0;

    public bool CarShouldStay = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSensorPosition = 0.5f;

    private float timer = 0f;
    public float SUTTimer = 0f;

    public bool sutWasLesserThanZero = false;

    public float firstValue = 0f;
    public float secondValue = 0f;

    public float operationalTimer = 0f;
    public bool StartCounting = false;

    public bool cantBrake = false;

    public float distanceToWall;
    private float angle = 1f;
    // Start is called before the first frame update
    void Start()
    {
        globalTimer = GameObject.FindGameObjectWithTag("globalTimer").GetComponent<GlobalTimerScript>();
        //Rotate toward second node in path
        this.transform.LookAt(nodes[3].position);

        this.tag = spawner.carTags[pathNumber];
        this.transform.Find("Body").tag = spawner.carTags[pathNumber];

        //maxWheelTorque = 80f;
        // maxBreakTorque = 150f;
        if (typeOfCar == "big")
        {
            distanceToWall = 1.5f;
        }
        else
        {
            distanceToWall = 1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (operationalTimer < 0)
        {
            StartCounting = false;
            isBraking = false;
        }

        CheckOnceIfWallDisabled();
        Sensors();
        ApplyStear();
        CheckWaypointDistance();
        KeepRotation();
        CheckIfDestroy();

        if (StartCounting)
        {
            operationalTimer -= Time.deltaTime;
            isBraking = true;
        }

        Drive();
        Brake();
    }

    private void CheckOnceIfWallDisabled()
    {
        var walls = GameObject.FindGameObjectsWithTag(wallTagToAvoid);
        if (!walls[0].GetComponent<BoxCollider>().enabled && CarShouldStay)
        {
            if (realisticSUT)
            {
                //Debug.Log("CarShouldStay");

                SetOperationalTimer();
                StartCounting = true;
                CarShouldStay = false;
                //Debug.Log("CarShouldStay:" + CarShouldStay);
            }
            else
            {
                StartCounting = true;
                CarShouldStay = false;
            }
        }
       
    }
    private void Sensors()
    {
        RaycastHit hit;

        //properties of ray setup (to avoid self and ground detecting)
        Vector3 destinationOfRay = transform.forward;
        destinationOfRay.y = 0.1f;

        var positieVoor = transform.position + transform.forward;

        Debug.DrawRay(positieVoor, destinationOfRay, Color.green);
        Ray redLightChecker = new Ray(positieVoor, destinationOfRay);
        var raycast1 = Physics.Raycast(redLightChecker, out hit);


        if (raycast1)
        {
            var distanceToMeasure = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm / 16;


            if (hit.distance < distanceToMeasure && hit.distance > 2f)
            {
                foreach (string car in carTagsToAvoid)
                {
                    if (hit.collider.tag == car)
                    {
                        var carInFront = hit.transform.gameObject.GetComponent<CarEngine>();
                        if (carInFront.currentSpeed < this.currentSpeed)
                        {
                            isBraking = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("XXX");
                        isBraking = false;
                    }
                }
                if (hit.collider.tag == wallTagToAvoid)
                {
                    isBraking = true;
                }
            }
            else if ((hit.distance <= 2f))
            {
                foreach (string car in carTagsToAvoid)
                {
                    if (hit.collider.tag == car)
                    {
                        if (hit.transform.gameObject.GetComponent<CarEngine>().firstValue != 0f)
                        {
                            secondValue = hit.transform.gameObject.GetComponent<CarEngine>().secondValue;
                        }
                        CarShouldStay = true;
                        isBraking = true;
                    }
                }
                if (hit.collider.tag == wallTagToAvoid)
                {
                    isBraking = false;
                }
                if ((hit.distance <= distanceToWall) && (hit.collider.tag == wallTagToAvoid))
                {
                    firstValue = hit.transform.gameObject.GetComponent<LightTimer>().firstSUTValue;
                    secondValue = hit.transform.gameObject.GetComponent<LightTimer>().secondSUTValue;
                    CarShouldStay = true;
                    isBraking = true;
                }
            }
            else
            {
                isBraking = false;
            }
        }
        else
        {
            //Debug.Log("I am free!" + this);
            isBraking = false;
        }
    }

    private void ApplyStear()
    {

        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxStearingAngle;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxWheelTorque;
            wheelFR.motorTorque = maxWheelTorque;
        }
        else
        {
            wheelFL.motorTorque = 0f;
            wheelFR.motorTorque = 0f;
        }
        
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1.5f)
        {
            currentNode++;
        }
    }

    private void KeepRotation()
    {
        var rotation = transform.rotation;
        rotation.z = 0f;
        transform.rotation = rotation;
    }

    private void Brake()
    {
        if (isBraking)
        {
            wheelBL.brakeTorque = maxBreakTorque;
            wheelBR.brakeTorque = maxBreakTorque;
            wheelFL.motorTorque = 0f;
            wheelFR.motorTorque = 0f;
        }
        else
        {
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;
        }
    }

    private void CheckIfDestroy()
    {
        if (currentNode >= nodes.Count - 1)
        {
            spawner.carsInPaths[pathNumber].Remove(gameObject);
            var tuple = new Tuple<int, Guid, float, string>(globalTimer.amountOfCarsPassed+1, Guid.NewGuid(), globalTimer.timer, gameObject.ToString());
            globalTimer.listOfDestroyedCars.Add(tuple);
            Destroy(gameObject);
            globalTimer.amountOfCarsPassed += 1;
        }
    }

    public void SetOperationalTimer()
    {
        if (realisticSUT)
        {
            if (firstValue == 0f)
            {
                operationalTimer = secondValue;
            }
            if (firstValue != 0f && secondValue != 0f)
            {
                operationalTimer = firstValue;
            }
        }
        else
        {
            operationalTimer = 0f;
        }
        //Debug.Log("Set LWTSC to "+ CarShouldStay + " so it should set operationalTimer " + operationalTimer);
    }
}
