using UnityEngine;

public class GlobalTimerScript : MonoBehaviour
{
    public int time;
    public int amountOfCarsPassed = 0;

    private float timer;

    private void Start()
    {
        timer = (float)time;   
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            //return to mainMenu with value of cars passed
        }
    }
}