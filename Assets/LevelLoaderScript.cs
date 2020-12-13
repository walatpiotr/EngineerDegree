using System;
using System.Collections;
using System.Collections.Generic;
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
    private void Awake()
    {
        transition = GameObject.FindGameObjectWithTag("animator").GetComponent<Animator>();

    }

    void Start()
    {
        Debug.Log(transition.name);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            startSUT.onClick.AddListener(GoToSimulationSUT);
            //startNonSUT.onClick.AddListener(GoToSimulationNonSUT);
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
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
