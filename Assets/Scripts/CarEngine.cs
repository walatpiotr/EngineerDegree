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
    public string carTagToAvoid;
    public string wallTagToAvoid;

    private int currentNode = 0;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSensorPosition = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Rotate toward second node in path
        this.transform.LookAt(nodes[3].position);

        this.tag = carTagToAvoid;
        this.transform.Find("Body").tag = carTagToAvoid;
        maxWheelTorque = 80f;
        maxBreakTorque = 200f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sensors();
        ApplyStear();
        Drive();
        CheckWaypointDistance();
        KeepRotation();
        CheckIfDestroy();
        Brake();
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
            var distanceToMeasure = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm / 15;
            //Debug.Log("Real distance : " + hit.distance + "   Distance by speed :  " + distanceToMeasure);
            if(((hit.distance < 2f) && ((hit.collider.tag == carTagToAvoid) || (hit.collider.tag == wallTagToAvoid))) 
                || ((hit.distance < distanceToMeasure) && ((hit.collider.tag == carTagToAvoid) || (hit.collider.tag == wallTagToAvoid))))
            {
                isBraking = true;
            }
            else
            {
                isBraking = false;
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
        }
    }
}
