﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueGathering : MonoBehaviour
{
    public TextMeshProUGUI minutes;
    public TextMeshProUGUI percentage;
    public TextMeshProUGUI green;
    public TextMeshProUGUI yellow;
    public TextMeshProUGUI tBlock;

    public GameObject valueHolder;

    void Update()
    {
        valueHolder = GameObject.FindGameObjectWithTag("valueHolder");
        var values = valueHolder.GetComponent<ValueHolderScript>();
        values.minutes = System.Convert.ToInt32(minutes.text);
        values.percentageOfBigCars = System.Convert.ToInt32(percentage.text);
        values.greenLightSeconds = System.Convert.ToInt32(green.text);
        values.yellowLightSeconds = System.Convert.ToInt32(yellow.text);
        values.tBlockTimeSeconds = System.Convert.ToInt32(tBlock.text);
    }
}
