using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUTGeneration : MonoBehaviour
{
    private static float fastFastRatio = 71.82f;
    private static float fastSlowRatio = 5.45f;
    private static float slowFastRatio = 0.92f;
    private static float slowSlowRatio = 21.82f;

    private static List<float> situationCapValues = 
        new List<float> {
            fastFastRatio,
            fastFastRatio + fastSlowRatio,
            fastFastRatio + fastSlowRatio + slowFastRatio,
            fastFastRatio + fastSlowRatio + slowFastRatio + slowSlowRatio };

    private static float minFirst = 0.20f;
    private static float meanFirst = 1.57f;
    private static float medianFirst = 1.35f; 
    private static float maxFirst = 4.32f;

    private static float minSecond = 1.16f;
    private static float meanSecond = 2.64f;
    private static float medianSecond = 2.48f;
    private static float maxSecond = 4.78f;

    //Do not sure if needed
    private static float fastRatioOfFirstCar = 77.27f;
    private static float slowRatioOfFirstCar = 22.73f;

    private static float fastRatioOfSecondCar = 72.73f;
    private static float slowRatioOfSecondCar = 27.27f;
    //

    // Cluster centers
    private static float fastFirstClusterCenter = 1.07f;
    private static float slowFirstClusterCenter = 2.97f;

    private static float fastSecondClusterCenter = 2.18f;
    private static float slowSecondClusterCenter = 3.75f;

    private static List<string> situations = 
        new List<string>() { "ff", "fs", "sf", "ss" };

    public List<float> SetUpValuesForFirstTwoCars()
    {
        string reactionSituation = ChooseSituation();

        List<float> returnFloats = null;
        switch (reactionSituation)
        {
            case "ff":
                return SetUpProperSUTToSituation(true, true);
            case "fs":
                return SetUpProperSUTToSituation(true, false);
            case "sf":
                return SetUpProperSUTToSituation(false, true);
            case "ss":
                return SetUpProperSUTToSituation(false, false);
        }
        return returnFloats;
    }

    private string ChooseSituation()
    {
        var randomValue = Random.Range(0.0f, 100.0f);
        
        if (randomValue >= situationCapValues[2])
        {
            return situations[3];
        }
        if (randomValue >= situationCapValues[1])
        {
            return situations[2];
        }
        if (randomValue >= situationCapValues[0])
        {
            return situations[1];
        }
        if (randomValue >= 0.0f)
        {
            return situations[0];
        }

        return situations[Random.Range(0, 4)];
    }

    private List<float> SetUpProperSUTToSituation(bool firstFast, bool secondFast)
    {
        float firstCarValue;
        float secondCarValue;
        if (firstFast)
        {
            if (secondFast)
            {
                firstCarValue = Random.Range(minFirst, minFirst + 2 * (fastFirstClusterCenter - minFirst));
                secondCarValue = Random.Range(minSecond, minSecond + 2 * (fastSecondClusterCenter - minSecond));
            }
            else
            {
                firstCarValue = Random.Range(minFirst, minFirst + 2 * (fastFirstClusterCenter - minFirst));
                secondCarValue = Random.Range(maxSecond - 2 * (maxSecond-slowSecondClusterCenter), maxSecond);
            }
        }
        else
        {
            if (secondFast)
            {
                firstCarValue = Random.Range(maxFirst - 2 * (maxFirst-slowFirstClusterCenter), maxFirst);
                secondCarValue = Random.Range(minSecond, minSecond + 2 * (fastSecondClusterCenter - minSecond));
            }
            else
            {
                firstCarValue = Random.Range(maxFirst - 2 * (maxFirst - slowFirstClusterCenter), maxFirst);
                secondCarValue = Random.Range(maxSecond - 2 * (maxSecond - slowSecondClusterCenter), maxSecond);
            }
        }

        return new List<float>(){ firstCarValue, secondCarValue};
    }
}
