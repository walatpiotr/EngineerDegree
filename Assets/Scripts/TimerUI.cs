using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Text TimerText;
    public GameObject globalTimerObject;
    public GlobalTimerScript globalTimer;

    private float Timer;

    private void Start()
    {
        globalTimer = globalTimerObject.GetComponent<GlobalTimerScript>();
        Timer = globalTimer.timer;
    }

    void Update()
    {
        Timer = globalTimer.timer;
        int minutes = Mathf.FloorToInt(Timer / 60F);
        int seconds = Mathf.FloorToInt(Timer % 60F);
        int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
        TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        
    }

}