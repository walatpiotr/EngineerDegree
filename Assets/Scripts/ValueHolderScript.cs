using UnityEngine;

public class ValueHolderScript : MonoBehaviour
{
    public int minutes;
    public int percentageOfBigCars;
    public int greenLightSeconds;
    public int yellowLightSeconds;
    public int tBlockTimeSeconds;
    public int generatorFrequency;

    public int amountOfCars;
    public int previouseSceneIndex;

    private static ValueHolderScript playerInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }
}
