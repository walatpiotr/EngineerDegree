using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueHolderScript : MonoBehaviour
{
    public int minutes;
    public int percentageOfBigCars;
    public int greenLightSeconds;
    public int yellowLightSeconds;
    public int tBlockTimeSeconds;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
