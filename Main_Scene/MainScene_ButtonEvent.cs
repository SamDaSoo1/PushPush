using TMPro;
using UnityEditor.SearchService;
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

    float textOffset = 15;


    private void Start()
    {
        originalGameText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        customGameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        cMapEditorText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        helpText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        helpWindow = transform.GetChild(4).gameObject;
        helpWindow.SetActive(false);
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
}
