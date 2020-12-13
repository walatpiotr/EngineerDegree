using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTimeParametrsHolder : MonoBehaviour
{
    public float GreenLightTime = 10.0f;
    public float YellowLightTime = 1.0f;
    public float TBlockTime = 2.0f;

    public float delayOne;
    public float delayTwo;

    public List<GameObject> walls;

    // Start is called before the first frame update
    void Start()
    {
        delayOne = YellowLightTime+GreenLightTime+YellowLightTime+TBlockTime;
        delayTwo = delayOne * 2;

        SpawnAllRedLights();
    }

    void SpawnAllRedLights()
    {
        foreach (GameObject wall in walls)
        {
            var wallParameters = wall.GetComponent<LightTimer>();
            wallParameters.green = GreenLightTime;
            wallParameters.yellow = YellowLightTime;
            wallParameters.tBlock = TBlockTime;

            Instantiate(wall);
        }
    }
}
