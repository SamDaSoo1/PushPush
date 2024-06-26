using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OriginalGameScene_ButtonEvent : MonoBehaviour
{
    Administrator administrator;


    private void Start()
    {
        administrator = FindObjectOfType<Administrator>();
    }

    public void UpArrowClick()
    {
        administrator.Move(ButtonType.UpArrow);
    }

    public void DownArrowClick()
    {
        administrator.Move(ButtonType.DownArrow);
    }

    public void LeftArrowClick()
    {
        administrator.Move(ButtonType.LeftArrow);
    }

    public void RightArrowClick()
    {
        administrator.Move(ButtonType.RightArrow);
    }

    public void TimeLeapClick()
    {
        administrator.Move(ButtonType.TimeLeap);
    }

    public void HomeButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void StageUpButton()
    {
        if (PlayerPrefs.GetInt("Stage") == 50)
            return;

        administrator.NextStage();
    }

    public void StageDownButton()
    {
        if (PlayerPrefs.GetInt("Stage") == 1)
            return;

        administrator.PrevStage();
    }
}
