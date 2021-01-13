using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueGathering : MonoBehaviour
{
    public TextMeshProUGUI minutes;
    public TextMeshProUGUI percentage;

    public TextMeshProUGUI greenInput;
    public TextMeshProUGUI yellowInput;
    public TextMeshProUGUI tBlockInput;

    public GameObject valueHolder;

    void Update()
    {
        
        valueHolder = GameObject.FindGameObjectWithTag("valueHolder");
        var values = valueHolder.GetComponent<ValueHolderScript>();
        values.minutes = System.Convert.ToInt32(minutes.text);
        values.percentageOfBigCars = System.Convert.ToInt32(percentage.text);

        values.greenLightSeconds = System.Convert.ToInt32(greenInput.text);
        
        values.yellowLightSeconds = System.Convert.ToInt32(yellowInput.text);
        
        values.tBlockTimeSeconds = System.Convert.ToInt32(tBlockInput.text);
        
    }
}
