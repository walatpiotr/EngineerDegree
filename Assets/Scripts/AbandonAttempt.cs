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

    // Start is called before the first frame update
    void Start()
    {
        abandonButton.onClick.AddListener(GoToMainMenu);
    }

    // Update is called once per frame
    void GoToMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
