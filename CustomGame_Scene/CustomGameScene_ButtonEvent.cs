using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomGameScene_ButtonEvent : MonoBehaviour
{
    Administrator_Custom administrator;

    public bool timeLeap { get; private set; } = false;

    private void Start()
    {
        administrator = FindObjectOfType<Administrator_Custom>();
    }

    public void UpArrowClick()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.AllStopSFX();
        SoundManager.Instance.PlaySFX(Sfx.Move);
        timeLeap = false;
        administrator.Move(ButtonType.UpArrow);
    }

    public void DownArrowClick()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.AllStopSFX();
        SoundManager.Instance.PlaySFX(Sfx.Move);
        timeLeap = false;
        administrator.Move(ButtonType.DownArrow);
    }

    public void LeftArrowClick()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.AllStopSFX();
        SoundManager.Instance.PlaySFX(Sfx.Move);
        timeLeap = false;
        administrator.Move(ButtonType.LeftArrow);
    }

    public void RightArrowClick()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.AllStopSFX();
        SoundManager.Instance.PlaySFX(Sfx.Move);
        timeLeap = false;
        administrator.Move(ButtonType.RightArrow);
    }

    public void TimeLeapClick()
    {
        timeLeap = true;
        administrator.Move(ButtonType.TimeLeap);
    }

    public void HomeButtonClick()
    {
        SoundManager.Instance.AllStopSFX();
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
