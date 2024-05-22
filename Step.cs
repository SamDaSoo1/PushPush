using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Step : MonoBehaviour
{
    [SerializeField] Text step;
    int stepCount = 0;

    void Start()
    {
        step = transform.GetChild(3).GetComponent<Text>();
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
}
