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
    public float maxBreakTorque = 150f;

    public bool isBraking = false;

    public CarSpawner spawner;
    public int pathNumber;
    public List<Transform> nodes;


    private int currentNode = 0;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSensorPosition = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        //find the vector pointing from our position to the target
        var _direction = (nodes[1].position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        var _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 300f);
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
        //Vector3 startingOfRay = transform.position;
        //startingOfRay.x = -1.0f;

        Debug.DrawRay(transform.position, destinationOfRay * 10f, Color.green);
        Ray redLightChecker = new Ray(transform.position, destinationOfRay);

        var raycast = Physics.Raycast(redLightChecker, out hit);
        if (raycast)
        {
            Debug.Log(hit.distance);
            if (hit.distance < 10f)
            {
                hit.collider.GetComponent<Renderer>().material.color = Color.green;
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
        if(!isBraking && currentSpeed < maxSpeed)
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
        if(Vector3.Distance(transform.position ,nodes[currentNode].position) < 1.5f)
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
