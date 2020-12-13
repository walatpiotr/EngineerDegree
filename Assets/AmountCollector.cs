using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmountCollector : MonoBehaviour
{
    public TextMeshProUGUI textSUT;
    public TextMeshProUGUI textNonSUT;

    public ValueHolderScript values;
    

    // Start is called before the first frame update
    void Start()
    {
        values = GameObject.FindGameObjectWithTag("valueHolder").GetComponent<ValueHolderScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (values.previouseSceneIndex == 1)
        {
            textSUT.text = values.amountOfCars.ToString();
        }
        if (values.previouseSceneIndex == 2)
        {
            textNonSUT.text = values.amountOfCars.ToString();
        }
    }
}
