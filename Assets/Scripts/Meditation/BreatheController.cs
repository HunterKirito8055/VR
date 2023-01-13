using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BreatheController : MonoBehaviour
{
    public Text breatheInValue;
    public Text breatheOutValue;
    public Button nextButton;
    public GameObject panel;
    public MeditationHale[] meditationHales;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => Next());
    }

    public void NextButtonActivation()
    {
        bool value = false;
        if (String.IsNullOrEmpty(breatheInValue.text.ToString()) ||
            String.IsNullOrEmpty(breatheOutValue.text.ToString()))
        {
            value = false;
        }
        else
        {
            if (float.IsNaN(float.Parse(breatheInValue.text.ToString())) ||
                float.IsNaN(float.Parse(breatheOutValue.text.ToString())))
            {
                value = false;
            }
            else
            {
                value = true;
            }
        }
        nextButton.gameObject.SetActive(value);
    }

    public void Next()
    {
        float breatheIn = float.Parse(breatheInValue.text.ToString());
        float breatheOut = float.Parse(breatheOutValue.text.ToString());
        foreach (var item in meditationHales)
        {
            item.OnBreatheValues(breatheIn, breatheOut);
        }
        panel.SetActive(true);
        gameObject.SetActive(false);
    }
}
