using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTimeParametrsHolder : MonoBehaviour
{
    public bool realisticSUT = false;
    public SUTGeneration generator = null;

    public float GreenLightTime = 10.0f;
    public float YellowLightTime = 1.0f;
    public float TBlockTime = 2.0f;

    public float delayOne;
    public float delayTwo;

    public List<GameObject> walls;

    public float timer1 = 0f;
    public float timer2 = 0f;
    public float timer3 = 0f;

    public GameObject lightRed3_1;
    public GameObject lightRed3_2;
    public GameObject lightRed2_1;
    public GameObject lightRed2_2;
    public GameObject lightRed1_1;
    public GameObject lightRed1_2;

    public GameObject lightYellow3_1;
    public GameObject lightYellow3_2;
    public GameObject lightYellow2_1;
    public GameObject lightYellow2_2;
    public GameObject lightYellow1_1;
    public GameObject lightYellow1_2;

    public GameObject lightGreen3_1;
    public GameObject lightGreen3_2;
    public GameObject lightGreen2_1;
    public GameObject lightGreen2_2;
    public GameObject lightGreen1_1;
    public GameObject lightGreen1_2;

    public GameObject lightRedYellow3_1;
    public GameObject lightRedYellow3_2;
    public GameObject lightRedYellow2_1;
    public GameObject lightRedYellow2_2;
    public GameObject lightRedYellow1_1;
    public GameObject lightRedYellow1_2;

    public GameObject current3_1;
    public GameObject current3_2;
    public GameObject current2_1;
    public GameObject current2_2;
    public GameObject current1_1;
    public GameObject current1_2;

    private bool greenFlag1 = false;
    private bool yellowFlag1 = true;
    private bool redFlag1 = false;
    private bool wasGreen1 = false;

    private bool greenFlag2 = false;
    private bool yellowFlag2 = true;
    private bool redFlag2 = false;
    private bool wasGreen2 = false;

    private bool greenFlag3 = false;
    private bool yellowFlag3 = true;
    private bool redFlag3 = false;
    private bool wasGreen3 = false;

    private float RedLightTime;

    private GameObject currentForDestroy3_1;
    private GameObject currentForDestroy3_2;
    private GameObject currentForDestroy2_1;
    private GameObject currentForDestroy2_2;
    private GameObject currentForDestroy1_1;
    private GameObject currentForDestroy1_2;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllRedLights();

        delayOne = YellowLightTime + GreenLightTime + YellowLightTime + TBlockTime;
        delayTwo = delayOne * 2;

        timer3 = 0f;
        timer1 = delayTwo;
        timer2 = delayOne;
        RedLightTime = YellowLightTime * 4 + GreenLightTime * 2 + TBlockTime * 2;
        StartingSetup();
    }

    void StartingSetup()
    {
        current3_1 = lightRed3_1;
        current3_2 = lightRed3_2;
        current2_1 = lightRed2_1;
        current2_2 = lightRed2_2;
        current1_1 = lightRed1_1;
        current1_2 = lightRed1_2;
        currentForDestroy1_1 = Instantiate(current1_1);
        currentForDestroy1_2 = Instantiate(current1_2);
        currentForDestroy2_1 = Instantiate(current2_1);
        currentForDestroy2_2 = Instantiate(current2_2);
        currentForDestroy3_1 = Instantiate(current3_1);
        currentForDestroy3_2 = Instantiate(current3_2);
    }

    void SpawnAllRedLights()
    {
        foreach (GameObject wall in walls)
        {
            var wallParameters = wall.GetComponent<LightTimer>();
            wallParameters.green = GreenLightTime;
            wallParameters.yellow = YellowLightTime;
            wallParameters.tBlock = TBlockTime;
            wallParameters.realisticSUT = realisticSUT;
            wallParameters.generator = generator;
            Instantiate(wall);
        }
    }

    private void Update()
    {
        RunTime();
        CheckLightStatus1();
        CheckLightStatus2();
        CheckLightStatus3();
    }

    void CheckLightStatus1()
    {
        if (timer1 <= 0)
        {
            if (greenFlag1) // Green to yellow
            {
                greenFlag1 = false;
                yellowFlag1 = true;
                wasGreen1 = true;
                AssignProperLightTotimer(timer1, "yellow");
                DestroyPrevious(timer1);
                InstantiateProperLights(timer1);
                timer1 = YellowLightTime;
                return;
            }
            if (yellowFlag1) // Yellow to something
            {
                if (wasGreen1 == true) // Yellow to Red
                {
                    redFlag1 = true;
                    yellowFlag1 = false;
                    wasGreen1 = false;
                    AssignProperLightTotimer(timer1, "red");
                    DestroyPrevious(timer1);
                    InstantiateProperLights(timer1);
                    timer1 = RedLightTime;
                    return;
                }
                if (wasGreen1 == false) // Yellow to Green
                {
                    greenFlag1 = true;
                    yellowFlag1 = false;
                    wasGreen1 = true;
                    AssignProperLightTotimer(timer1, "green");
                    DestroyPrevious(timer1);
                    InstantiateProperLights(timer1);
                    timer1 = GreenLightTime;
                    return;
                }
            }
            if (redFlag1) // Red to yellow
            {
                Debug.Log("1 - redToYellow - upper");

                redFlag1 = false;
                yellowFlag1 = true;
                wasGreen1 = false;
                AssignProperLightTotimer(timer1, "redYellow");
                DestroyPrevious(timer1);
                InstantiateProperLights(timer1);
                timer1 = YellowLightTime;
                return;
            }
        }
    }

    void CheckLightStatus2()
    {
        if (timer2 <= 0)
        {
            if (greenFlag2) // Green to yellow
            {
                greenFlag2 = false;
                yellowFlag2 = true;
                wasGreen2 = true;
                AssignProperLightTotimer(timer2, "yellow");
                DestroyPrevious(timer2);
                InstantiateProperLights(timer2);
                timer2 = YellowLightTime;
                return;
            }
            if (yellowFlag2) // Yellow to something
            {
                if (wasGreen2 == true) // Yellow to Red
                {
                    redFlag2 = true;
                    yellowFlag2 = false;
                    wasGreen2 = false;
                    AssignProperLightTotimer(timer2, "red");
                    DestroyPrevious(timer2);
                    InstantiateProperLights(timer2);
                    timer2 = RedLightTime;
                    return;
                }
                if (wasGreen2 == false) // Yellow to Green
                {
                    greenFlag2 = true;
                    yellowFlag2 = false;
                    wasGreen2 = true;
                    AssignProperLightTotimer(timer2, "green");
                    DestroyPrevious(timer2);
                    InstantiateProperLights(timer2);
                    timer2 = GreenLightTime;
                    return;
                }
            }
            if (redFlag2) // Red to yellow
            {
                Debug.Log("2 - redToYellow - upper");

                redFlag2 = false;
                yellowFlag2 = true;
                wasGreen2 = false;
                AssignProperLightTotimer(timer2, "redYellow");
                DestroyPrevious(timer2);
                InstantiateProperLights(timer2);
                timer2 = YellowLightTime;
                return;
            }
        }
    }

    void CheckLightStatus3()
    {
        if (timer3 <= 0)
        {
            if (greenFlag3) // Green to yellow
            {
                greenFlag3 = false;
                yellowFlag3 = true;
                wasGreen3 = true;
                AssignProperLightTotimer(timer3, "yellow");
                DestroyPrevious(timer3);
                InstantiateProperLights(timer3);
                timer3 = YellowLightTime;
                return;
            }
            if (yellowFlag3) // Yellow to something
            {
                if (wasGreen3 == true) // Yellow to Red
                {
                    redFlag3 = true;
                    yellowFlag3 = false;
                    wasGreen3 = false;
                    AssignProperLightTotimer(timer3, "red");
                    DestroyPrevious(timer3);
                    InstantiateProperLights(timer3);
                    timer3 = RedLightTime;
                    return;
                }
                if (wasGreen3 == false) // Yellow to Green
                {
                    //Debug.Log("im where I should be");
                    greenFlag3 = true;
                    yellowFlag3 = false;
                    wasGreen3 = true;
                    AssignProperLightTotimer(timer3, "green");
                    DestroyPrevious(timer3);
                    InstantiateProperLights(timer3);
                    timer3 = GreenLightTime;
                    return;
                }
            }
            if (redFlag3) // Red to yellow
            {
                Debug.Log("3 - redToYellow - upper");
                redFlag3 = false;
                yellowFlag3 = true;
                wasGreen3 = false;
                AssignProperLightTotimer(timer3, "redYellow");
                DestroyPrevious(timer3);
                InstantiateProperLights(timer3);
                timer3 = YellowLightTime;
                return;
            }
        }
    }

    void RunTime()
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        timer3 -= Time.deltaTime;
    }

    void AssignProperLightTotimer(float timer, string name)
    {
        if(name == "red")
        {
            if(timer == timer1)
            {
                current1_1 = lightRed1_1;
                current1_2 = lightRed1_2;
            }
            if (timer == timer2)
            {
                current2_1 = lightRed2_1;
                current2_2 = lightRed2_2;
            }
            if (timer == timer3)
            {
                current3_1 = lightRed3_1;
                current3_2 = lightRed3_2;
            }
        }
        if(name == "yellow")
        {
            if (timer == timer1)
            {
                current1_1 = lightYellow1_1;
                current1_2 = lightYellow1_2;
            }
            if (timer == timer2)
            {
                current2_1 = lightYellow2_1;
                current2_2 = lightYellow2_2;
            }
            if (timer == timer3)
            {
                current3_1 = lightYellow3_1;
                current3_2 = lightYellow3_2;
            }
        }
        if(name == "green")
        {
            if (timer == timer1)
            {
                current1_1 = lightGreen1_1;
                current1_2 = lightGreen1_2;
            }
            if (timer == timer2)
            {
                current2_1 = lightGreen2_1;
                current2_2 = lightGreen2_2;
            }
            if (timer == timer3)
            {
                current3_1 = lightGreen3_1;
                current3_2 = lightGreen3_2;
            }
        }
        if (name == "redYellow")
        {
            Debug.Log("redToYellow");
            if (timer == timer1)
            {
                current1_1 = lightRedYellow1_1;
                current1_2 = lightRedYellow1_2;
            }
            if (timer == timer2)
            {
                current2_1 = lightRedYellow2_1;
                current2_2 = lightRedYellow2_2;
            }
            if (timer == timer3)
            {
                current3_1 = lightRedYellow3_1;
                current3_2 = lightRedYellow3_2;
            }
        }
    }

    void InstantiateProperLights(float timer)
    {
        if(timer == timer1)
        {
            currentForDestroy1_1 = Instantiate(current1_1);
            currentForDestroy1_2 = Instantiate(current1_2);
        }
        if (timer == timer2)
        {
            currentForDestroy2_1 = Instantiate(current2_1);
            currentForDestroy2_2 = Instantiate(current2_2);
        }
        if (timer == timer3)
        {
            currentForDestroy3_1 = Instantiate(current3_1);
            currentForDestroy3_2 = Instantiate(current3_2);
        }
    }

    void DestroyPrevious(float timer)
    {
        if (timer == timer1)
        {
            Destroy(currentForDestroy1_1);
            Destroy(currentForDestroy1_2);
        }
        if (timer == timer2)
        {
            Destroy(currentForDestroy2_1);
            Destroy(currentForDestroy2_2);
        }
        if (timer == timer3)
        {
            Destroy(currentForDestroy3_1);
            Destroy(currentForDestroy3_2);
        }
    }
}
