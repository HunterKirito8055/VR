using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationHale : MonoBehaviour
{
    // public RectTransform timerText;
    public RectTransform rectImage;
    public float maxSize, minSize;
    public float tempFloat;
    public float inHale, exHale;
    public bool isInHale;

   
    void Start()
    {
        isInHale = true;
        rectImage.sizeDelta = new Vector2(minSize, minSize);
    }
    public void OnBreatheValues(float _inHale,float _exHale)
    {
        inHale = _inHale;
        exHale = _exHale;
    }
    void FixedUpdate()
    {
        if (isInHale)
        {
            tempFloat += Time.fixedDeltaTime / inHale;
            rectImage.sizeDelta = Vector2.Lerp(new Vector2(minSize, minSize), new Vector2(maxSize, maxSize), tempFloat);
            if (tempFloat >= 1)
            {
                isInHale = false;
                tempFloat = 0;
            }
        }
        else
        {
            tempFloat += Time.fixedDeltaTime / exHale;
            rectImage.sizeDelta = Vector2.Lerp(new Vector2(maxSize, maxSize), new Vector2(minSize, minSize), tempFloat);
            if (tempFloat >= 1)
            {
                isInHale = true;
                tempFloat = 0;
            }
        }
    }
}
