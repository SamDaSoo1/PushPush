using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene_ButtonEvent : MonoBehaviour
{
    [SerializeField] Text originalGameText;
    [SerializeField] Text customGameText;
    [SerializeField] Text cMapEditorText;
    [SerializeField] Text helpText;

    float textOffset = 15;


    private void Start()
    {
        originalGameText = transform.GetChild(0).GetComponentInChildren<Text>();
        customGameText = transform.GetChild(1).GetComponentInChildren<Text>();
        cMapEditorText = transform.GetChild(2).GetComponentInChildren<Text>();
        helpText = transform.GetChild(3).GetComponentInChildren<Text>();
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
    }
}
