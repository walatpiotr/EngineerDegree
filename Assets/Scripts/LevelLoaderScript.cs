using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderScript : MonoBehaviour
{
    public Button startSUT;
    public Button startNonSUT;

    private Animator transition = null;

    public float transitionTime = 1f;

    public GlobalTimerScript globalTimer;
    public ValueHolderScript valueHolder;

    public TMP_InputField greenInput;
    public TMP_InputField yellowInput;
    public TMP_InputField tBlockInput;

    private void Awake()
    {
        transition = GameObject.FindGameObjectWithTag("animator").GetComponent<Animator>();

    }

    void Start()
    {
        //Debug.Log(transition.name);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            startSUT.onClick.AddListener(GoToSimulationSUT);
            startNonSUT.onClick.AddListener(GoToSimulationNonSUT);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            valueHolder = GameObject.FindGameObjectWithTag("valueHolder").GetComponent<ValueHolderScript>();
            globalTimer = GameObject.FindGameObjectWithTag("globalTimer").GetComponent<GlobalTimerScript>();
        }
    }

    private void Update()
    {
        try
        {
            if (globalTimer.timer == 0)
            {
                valueHolder.previouseSceneIndex = SceneManager.GetActiveScene().buildIndex;
                valueHolder.amountOfCars = globalTimer.amountOfCarsPassed;
                StartCoroutine(LoadLevel(0));
            }
        }
        catch(Exception e)
        {

        }
    }

    void GoToSimulationSUT()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    void GoToSimulationNonSUT()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log(greenInput.text);
        Debug.Log(yellowInput.text);
        Debug.Log(tBlockInput.text);

        if (CheckIfInputIsValid())
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(levelIndex);
        }
        
    }

    bool CheckIfInputIsValid()
    {
        bool greenIsValid = true;
        bool yellowIsValid = true;
        bool tBlockIsValid = true;

        if (!NotNullChecker())
        {
            return false;
        }

        if (Int32.Parse(greenInput.text) < 8)
        {
            Debug.Log(Int32.Parse(greenInput.text));
            greenInput.text = "Must be >=8";
            greenIsValid = false;
        }
        if (Int32.Parse(yellowInput.text) < 2)
        {
            yellowInput.text = "Must be >=2";
            yellowIsValid = false;
        }
        if (Int32.Parse(tBlockInput.text) < 2)
        {
            tBlockInput.text = "Must be >=2";
            tBlockIsValid = false;
        }
        if(greenIsValid && yellowIsValid && tBlockIsValid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool NotNullChecker()
    {
        bool greenIsValid = true;
        bool yellowIsValid = true;
        bool tBlockIsValid = true;

        if (String.IsNullOrEmpty(greenInput.text))
        {
            greenInput.text = "Please input number";
            greenIsValid = false;
        }
        if (String.IsNullOrEmpty(yellowInput.text))
        {
            yellowInput.text = "Please input number";
            yellowIsValid = false;
        }
        if (String.IsNullOrEmpty(tBlockInput.text))
        {
            tBlockInput.text = "Please input number";
            tBlockIsValid = false;
        }
        if (!greenIsValid || !yellowIsValid || !tBlockIsValid)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void ABC()
    {
        Debug.Log(yellowInput.text);
    }

}
