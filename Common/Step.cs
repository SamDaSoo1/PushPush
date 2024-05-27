using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI step;
    int stepCount = 0;

    void Start()
    {
        step = GetComponent<TextMeshProUGUI>();
        step.text = "Step : " + stepCount.ToString();
    }

    public void OneStep()
    {
        stepCount++;
        step.text = "Step : " + stepCount.ToString();
    }

    public void Reset_Step()
    {
        stepCount = 0;
        step.text = "Step : " + stepCount.ToString();
    }

    public void TheEnd()
    {
        step.text = $"The End.";
    }
}
