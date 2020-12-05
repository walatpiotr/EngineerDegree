using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTimer : MonoBehaviour
{
    public int number;

    private float green;
    private float yellow;
    private float red;
    private float tBlock;

    private float delay;

    private bool greenFlag = true;
    private bool yellowFlag = false;
    private bool tBlockFlag = false;
    private bool redFlag = false;

    private bool wasGreen = false;

    private float timer = 0f;

    private BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        LightsTimeParametrsHolder parent = transform.parent.GetComponent<LightsTimeParametrsHolder>();

        green = parent.GreenLightTime;
        yellow = parent.YellowLightTime;
        tBlock = parent.TBlockTime;

        red = yellow * 4 + green * 2 + tBlock * 2;

        if (number == 1)
        {
            delay = parent.delayOne;
        }

        if (number == 2)
        {
            delay = parent.delayTwo;
        }

        if (number == 3)
        {
            delay = 0f;
        }

        timer = delay;

        collider = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (greenFlag) // Green to yellow
            {
                greenFlag = false;
                yellowFlag = true;
                wasGreen = true;
                collider.enabled = false;
                timer = yellow;
                return;
            }
            if (yellowFlag) // Yellow to something
            {
                if (wasGreen == true) // Yellow to Red
                {
                    redFlag = true;
                    yellowFlag = false;
                    wasGreen = false;
                    collider.enabled = true;
                    timer = red;
                    return;
                }
                if (wasGreen == false) // Yellow to Green
                {
                    greenFlag = true;
                    yellowFlag = false;
                    wasGreen = true;
                    collider.enabled = false;
                    timer = green;
                    return;
                }
            }
            if (redFlag) // Red to yellow
            {
                redFlag = false;
                yellowFlag = true;
                wasGreen = false;
                collider.enabled = true;
                timer = yellow;
                return;
            }
        }
    }
}
