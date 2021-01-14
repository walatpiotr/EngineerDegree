using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void doExitGame()
    {
        Debug.Log("Quiting");
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
