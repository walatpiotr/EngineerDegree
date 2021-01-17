using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCollector : MonoBehaviour
{
    public GameObject valueHolder;
    public GameObject lightsController;
    public GameObject carSpawner;
    public GameObject globalTimer;

    void Start()
    {
        valueHolder = GameObject.FindGameObjectWithTag("valueHolder");
        lightsController = GameObject.FindGameObjectWithTag("lightsController");
        carSpawner = GameObject.FindGameObjectWithTag("spawner");
        globalTimer = GameObject.FindGameObjectWithTag("globalTimer");

        var valuesFromMenu = valueHolder.GetComponent<ValueHolderScript>();
        var lightsParameters = lightsController.GetComponent<LightsTimeParametrsHolder>();
        var spawnerParameters = carSpawner.GetComponent<CarSpawner>();
        var globalParamters = globalTimer.GetComponent<GlobalTimerScript>();

        lightsParameters.GreenLightTime = valuesFromMenu.greenLightSeconds;
        lightsParameters.YellowLightTime = valuesFromMenu.yellowLightSeconds;
        lightsParameters.TBlockTime = valuesFromMenu.tBlockTimeSeconds;

        spawnerParameters.percentageOfBigCars = valuesFromMenu.percentageOfBigCars;

        globalParamters.time = valuesFromMenu.minutes * 60;

    }
}
