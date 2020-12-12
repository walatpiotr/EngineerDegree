using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    public TextMeshProUGUI sliderText;
    public Slider slider;

    private void Start()
    {
        slider = GetComponentInParent<Slider>();
    }

    void Update()
    {
        sliderText.text = slider.value.ToString();
    }
}
