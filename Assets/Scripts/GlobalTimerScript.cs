using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalTimerScript : MonoBehaviour
{
    public int time;
    public int amountOfCarsPassed = 0;

    public float timer;
    public List<Tuple<int, Guid, float, string>> listOfDestroyedCars = new List<Tuple<int, Guid, float, string>>();

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
            string fileName = "simulationrecords.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var tuple in listOfDestroyedCars)
                {
                    writer.WriteLine(tuple);
                }
            }
            listOfDestroyedCars = new List<Tuple<int, Guid, float, string>>();
            timer = 0;
        }
    }
}