using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    GameObject page1;
    GameObject page2;
    GameObject page3;

    int count = 1;

    void Awake()
    {
        page1 = transform.GetChild(0).gameObject;
        page2 = transform.GetChild(1).gameObject;
        page3 = transform.GetChild(2).gameObject;
    }

    private void OnEnable()
    {
        count = 1;
        Page1();
    }


    public void PrevPageButtonClick()
    {
        if (count == 1)
            return;

        count--;
        if (count == 1)
            Page1();
        else if (count == 2)
            Page2();
    }

    public void NextPageButtonClick()
    {
        if (count == 3)
            return;

        count++;
        if (count == 2)
            Page2();
        else if (count == 3)
            Page3();
    }

    public void OKButtonClick()
    {
        gameObject.SetActive(false);
    }

    void Page1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
    }

    void Page2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
    }

    void Page3()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(true);
    }
}
