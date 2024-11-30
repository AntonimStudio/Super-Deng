using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRGBInterferenceScript : MonoBehaviour
{
    [SerializeField] private RGBShiftEffect RGBShiftEffect;

    public float variableValue = 0f; 
    public float targetValue = 0.1f; 
    public float speed = 0.01f;

    public bool isTurnOn = false;
    public bool isIncreasing = false;
    public bool isChanging = false; 
    private float initialValue; 

    private void Start()
    {
        initialValue = variableValue;
    }

    private void Update()
    {
        if (isChanging && isTurnOn)
        {
            RGBShiftEffect.on = true;
            if (isIncreasing)
            {
                
                variableValue = Mathf.MoveTowards(variableValue, targetValue, speed * Time.deltaTime);
                RGBShiftEffect.amount = variableValue;
                if (Mathf.Approximately(variableValue, targetValue))
                {
                    isChanging = false;
                }
            }
            else
            {
                RGBShiftEffect.amount = variableValue;
                variableValue = Mathf.MoveTowards(variableValue, initialValue, speed * Time.deltaTime);
                if (Mathf.Approximately(variableValue, initialValue))
                {
                    isChanging = false;
                    RGBShiftEffect.on = false;
                }
            }
        }
        else if (!isChanging && isTurnOn)
        {
            RGBShiftEffect.on = true;
            RGBShiftEffect.amount = targetValue;
        }
        else if (!isChanging && !isTurnOn)
        {
            RGBShiftEffect.on = false;
        }
    }
}