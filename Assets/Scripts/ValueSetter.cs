using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueSetter : MonoBehaviour
{
    public GameObject valueHolder;

    public Slider minutes;
    public Slider vehicles;
    public Slider green;
    public Slider yellow;
    public Slider tBlock;


    void Awake()
    {
        valueHolder = GameObject.FindGameObjectWithTag("valueHolder");

        var valuesFromMenu = valueHolder.GetComponent<ValueHolderScript>();

        minutes.value = valuesFromMenu.minutes;
        vehicles.value = valuesFromMenu.percentageOfBigCars;
        green.value = valuesFromMenu.greenLightSeconds;
        yellow.value = valuesFromMenu.yellowLightSeconds;
        tBlock.value = valuesFromMenu.tBlockTimeSeconds;

    }
}

