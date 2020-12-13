using UnityEngine;

public class GlobalTimerScript : MonoBehaviour
{
    public int time;
    public int amountOfCarsPassed = 0;

    public float timer;

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
            timer = 0;
        }
    }
}