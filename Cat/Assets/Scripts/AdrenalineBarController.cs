using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdrenalineBarController : MonoBehaviour
{
    public Slider slider;
    public Gradient  gradient;
    public Image filler;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void setAdreanlineValues(float max, float min)
    {
        slider.maxValue = max;
        slider.minValue = min;
    }
    public void setAdrenaline(float value)
    {
        slider.value = value;
        filler.color = gradient.Evaluate(slider.normalizedValue);
    }

}
