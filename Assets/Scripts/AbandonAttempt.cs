using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbandonAttempt : MonoBehaviour
{
    public Button abandonButton;
    private Animator transition = null;
    public float transitionTime = 1f;

    private void Awake()
    {
        transition = GameObject.FindGameObjectWithTag("animator").GetComponent<Animator>();

    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
