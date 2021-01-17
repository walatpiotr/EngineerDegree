using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarAmountViewer : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public GameObject globalTimerObject;
    public GlobalTimerScript globalTimer;

    private float Timer;

    private void Start()
    {
        globalTimer = globalTimerObject.GetComponent<GlobalTimerScript>();
    }

    void Update()
    {
        Text.text = "Amount of cars: "+globalTimer.amountOfCarsPassed.ToString();
    }
}
