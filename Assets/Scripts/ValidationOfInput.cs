using TMPro;
using UnityEngine;

public class ValidationOfInput : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    public TMP_InputField input;
    

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInParent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.text != "")
            {
                if (Validation(input.text))
                {
                    AssignTextWithValue();
                }
                else
                {
                    input.text = "has to be digit only";
                }
            }
        }
    }

    void AssignTextWithValue()
    {
        inputText.text = input.text;
        input.text = "";
    }

    bool Validation(string input)
    {
        return IsDigitsOnly(input);
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }
}
