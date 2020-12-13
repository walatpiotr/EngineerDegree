using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public float currentSpeed;
    public float maxSpeed = 100f;
    public float maxStearingAngle = 45f;
    public float maxWheelTorque = 80f;
    public float maxBreakTorque = 100f;

    public bool isBraking;

    public CarSpawner spawner;
    public int pathNumber;
    public List<Transform> nodes;
    public List<string> carTagsToAvoid;
    public string wallTagToAvoid;
    public GlobalTimerScript globalTimer;

    private int currentNode = 0;

    public bool LightWantToStartCar = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSensorPosition = 0.5f;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        globalTimer = GameObject.FindGameObjectWithTag("globalTimer").GetComponent<GlobalTimerScript>();
        //Rotate toward second node in path
        this.transform.LookAt(nodes[3].position);

        this.tag = spawner.carTags[pathNumber];
        this.transform.Find("Body").tag = spawner.carTags[pathNumber];
        maxWheelTorque = 80f;
        maxBreakTorque = 200f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        globalTimer = GameObject.FindGameObjectWithTag("globalTimer").GetComponent<GlobalTimerScript>();
        Sensors();
        ApplyStear();
        Drive();
        CheckWaypointDistance();
        KeepRotation();
        CheckIfDestroy();
        Brake();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (LightWantToStartCar)
        {
            timer = 2f; 
            isBraking = false;
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

        var raycast = Physics.Raycast(redLightChecker, out hit);
        if (raycast)
        {
            var distanceToMeasure = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm / 20;

            if(pathNumber == 7 || pathNumber == 8)
            {
                if ((hit.distance < 1f) && ((hit.collider.tag == spawner.carTags[7]) || (hit.collider.tag == spawner.carTags[8]) || (hit.collider.tag == wallTagToAvoid)))
                {
                    isBraking = true;
                    Debug.Log("I stopped because i am to near: " + pathNumber);
                }
                else if (((hit.distance < distanceToMeasure) && ((hit.collider.tag == spawner.carTags[pathNumber]) || (hit.collider.tag == wallTagToAvoid))))
                {
                    isBraking = true;
                    //Debug.Log("I stopped because my speed is to much: " + pathNumber);
                }
                else
                {
                    isBraking = false;
                }
            }
            else
            {
                if ((hit.distance < 1f) && ((hit.collider.tag == spawner.carTags[pathNumber]) || (hit.collider.tag == wallTagToAvoid)))
                {
                    isBraking = true;
                    Debug.Log("I stopped because i am to near: " + pathNumber);
                }
                else if (((hit.distance < distanceToMeasure) && ((hit.collider.tag == spawner.carTags[pathNumber]) || (hit.collider.tag == wallTagToAvoid))))
                {
                    isBraking = true;
                    //Debug.Log("I stopped because my speed is to much: " + pathNumber);
                }
                else
                {
                    isBraking = false;
                }
            }
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
            Destroy(gameObject);
            globalTimer.amountOfCarsPassed += 1;
        }
    }
}
