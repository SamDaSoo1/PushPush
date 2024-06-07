using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageText_Editor : MonoBehaviour
{
    // component의 text라 해서 cText라고 지음. 내 스스로의 네이밍 규칙
    TextMeshProUGUI cText;

    private void Awake()
    {
        cText = GetComponent<TextMeshProUGUI>();
        PlayerPrefs.SetInt("Editor Stage", 1);
        PlayerPrefs.Save();
        TextUpdate();
    }

    public void TextUpdate()
    {
        cText.text = $"Stage {PlayerPrefs.GetInt("Editor Stage")}";
    }
}
