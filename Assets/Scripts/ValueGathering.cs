using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueGathering : MonoBehaviour
{
    public TextMeshProUGUI minutes;
    public TextMeshProUGUI percentage;

    public TMP_InputField greenInput;
    public TMP_InputField yellowInput;
    public TMP_InputField tBlockInput;

    public GameObject valueHolder;

    void Update()
    {
        
        valueHolder = GameObject.FindGameObjectWithTag("valueHolder");
        var values = valueHolder.GetComponent<ValueHolderScript>();
        values.minutes = System.Convert.ToInt32(minutes.text);
        values.percentageOfBigCars = System.Convert.ToInt32(percentage.text);

        if(greenInput.text != "")
        {
            values.greenLightSeconds = System.Convert.ToInt32(greenInput.text);
        }
        if (yellowInput.text != "")
        {
            values.yellowLightSeconds = System.Convert.ToInt32(yellowInput.text);
        }
        if (tBlockInput.text != "")
        {
            values.tBlockTimeSeconds = System.Convert.ToInt32(tBlockInput.text);
        }
    }
}
