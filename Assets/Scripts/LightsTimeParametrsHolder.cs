using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTimeParametrsHolder : MonoBehaviour
{
    public GameObject redLightWalltoInstantiate;
    public List<float> paramters1;
    public List<float> paramters2;
    public List<float> paramters3;

    public float GreenLightTime = 10.0f;
    public float YellowLightTime = 1.0f;
    public float TBlockTime = 2.0f;

    public float delayOne;
    public float delayTwo;

    // Start is called before the first frame update
    void Start()
    {
        delayOne = YellowLightTime+GreenLightTime+YellowLightTime+TBlockTime;
        delayTwo = delayOne * 2;
    }
}
