using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTimer : MonoBehaviour
{
    public int number;

    public float green;
    public float yellow;
    public float red;
    public float tBlock;

    public float delay;

    private bool greenFlag = false;
    private bool yellowFlag = true;
    private bool tBlockFlag = false;
    private bool redFlag = false;

    private bool wasGreen = false;

    private float timer = 0f;

    private BoxCollider collider;
    private MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        LightsTimeParametrsHolder parent = transform.parent.GetComponent<LightsTimeParametrsHolder>();

        green = parent.GreenLightTime;
        yellow = parent.YellowLightTime;
        tBlock = parent.TBlockTime;

        AssignNumber();

        red = yellow * 4 + green * 2 + tBlock * 2;

        if (number == 1)
        {
            delay = parent.YellowLightTime + parent.GreenLightTime + parent.YellowLightTime + parent.TBlockTime;
        }
        else if (number == 2)    
        {
            delay = 2 * (parent.YellowLightTime + parent.GreenLightTime + parent.YellowLightTime + parent.TBlockTime);
        }
        else if (number == 3)
        {
            delay = 0f;
        }

        Debug.Log(number + "  -  " + delay);

        timer = delay;

        collider = this.GetComponent<BoxCollider>();
        renderer = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(number == 3)
        {
            Debug.Log(timer);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (greenFlag) // Green to yellow
            {
                greenFlag = false;
                yellowFlag = true;
                wasGreen = true;
                renderer.enabled = false;
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
                    renderer.enabled = true;
                    collider.enabled = true;
                    timer = red;
                    return;
                }
                if (wasGreen == false) // Yellow to Green
                {
                    greenFlag = true;
                    yellowFlag = false;
                    wasGreen = true;
                    renderer.enabled = false;
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
                renderer.enabled = true;
                collider.enabled = true;
                timer = yellow;
                return;
            }
        }
    }

    void AssignNumber()
    {
        if(this.tag == "redlightwall1")
        {
            number = 1;
        }
        else if (this.tag == "redlightwall2")
        {
            number = 2;
        }
        else if (this.tag == "redlightwall3")
        {
            number = 3;
        }
    }
}
