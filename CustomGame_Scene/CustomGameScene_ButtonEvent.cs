using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomGameScene_ButtonEvent : MonoBehaviour
{
    Administrator_Custom administrator;

    private void Start()
    {
        administrator = FindObjectOfType<Administrator_Custom>();
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
        if (PlayerPrefs.GetInt("Custom Stage") == 50)
            return;

        administrator.NextStage();
    }

    public void StageDownButton()
    {
        if (PlayerPrefs.GetInt("Custom Stage") == 1)
            return;

        administrator.PrevStage();
    }
}
