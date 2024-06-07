using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene_ButtonEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI originalGameText;
    [SerializeField] TextMeshProUGUI customGameText;
    [SerializeField] TextMeshProUGUI cMapEditorText;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] GameObject helpWindow;
    [SerializeField] GameObject soundButton;
    [SerializeField] List<Sprite> soundButtonImg;

    float textOffset = 15;

    bool isMute = false;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BgmSound.Start);
        originalGameText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        customGameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        cMapEditorText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        helpText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        helpWindow = transform.GetChild(5).gameObject;
        helpWindow.SetActive(false);
        soundButton = transform.GetChild(4).gameObject;

        SoundButtonSpriteSetting();
    }

    public void OriginalGameButton_OnPointerDown()
    {
        originalGameText.rectTransform.anchoredPosition = new Vector2(originalGameText.rectTransform.anchoredPosition.x, originalGameText.rectTransform.anchoredPosition.y - textOffset);
    }

    public void OriginalGameButton_OnPointerUp()
    {
        originalGameText.rectTransform.anchoredPosition = new Vector2(originalGameText.rectTransform.anchoredPosition.x, originalGameText.rectTransform.anchoredPosition.y + textOffset);
        SceneManager.LoadScene("OriginalGame");
    }

    public void CustomGameButton_OnPointerDown()
    {
        customGameText.rectTransform.anchoredPosition = new Vector2(customGameText.rectTransform.anchoredPosition.x, customGameText.rectTransform.anchoredPosition.y - textOffset);
    }

    public void CustomGameButton_OnPointerUp()
    {
        customGameText.rectTransform.anchoredPosition = new Vector2(customGameText.rectTransform.anchoredPosition.x, customGameText.rectTransform.anchoredPosition.y + textOffset);
        SceneManager.LoadScene("CustomGame");
    }

    public void CMapEditorButton_OnPointerDown()
    {
        cMapEditorText.rectTransform.anchoredPosition = new Vector2(cMapEditorText.rectTransform.anchoredPosition.x, cMapEditorText.rectTransform.anchoredPosition.y - textOffset);
    }

    public void CMapEditorButton_OnPointerUp()
    {
        cMapEditorText.rectTransform.anchoredPosition = new Vector2(cMapEditorText.rectTransform.anchoredPosition.x, cMapEditorText.rectTransform.anchoredPosition.y + textOffset);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Editor");
    }

    public void HelpButton_OnPointerDown()
    {
        helpText.rectTransform.anchoredPosition = new Vector2(helpText.rectTransform.anchoredPosition.x, helpText.rectTransform.anchoredPosition.y - textOffset);
    }

    public void HelpButton_OnPointerUp()
    {
        helpText.rectTransform.anchoredPosition = new Vector2(helpText.rectTransform.anchoredPosition.x, helpText.rectTransform.anchoredPosition.y + textOffset);
        helpWindow.SetActive(true);
    }

    public void SoundButtonClick()
    {
        if (isMute == false)
            SoundManager.Instance.MuteOn();
        else
            SoundManager.Instance.MuteOff();

        SoundButtonSpriteSetting();
    }

    void SoundButtonSpriteSetting()
    {
        if (SoundManager.Instance.GetBgm().mute)
        {
            isMute = true;
            soundButton.GetComponent<Image>().sprite = soundButtonImg[2];
            SpriteState spriteState = soundButton.GetComponent<Button>().spriteState;
            spriteState.pressedSprite = soundButtonImg[3];
            soundButton.GetComponent<Button>().spriteState = spriteState;
        }
        else
        {
            isMute = false;
            soundButton.GetComponent<Image>().sprite = soundButtonImg[0];
            SpriteState spriteState = soundButton.GetComponent<Button>().spriteState;
            spriteState.pressedSprite = soundButtonImg[1];
            soundButton.GetComponent<Button>().spriteState = spriteState;
        }
    }
}
