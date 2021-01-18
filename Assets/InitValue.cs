using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitValue : MonoBehaviour
{
    public Slider slider;
    public ValueHolderScript valueHolder;
    // Start is called before the first frame update
    void Awake()
    {
        if (valueHolder.generatorFrequency != 0)
        {
            slider.value = valueHolder.generatorFrequency;
        }
        else
        {
            slider.value = slider.maxValue;
        }
    }
}
