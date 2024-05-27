using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageText_Custom : MonoBehaviour
{
    // component의 text라 해서 cText라고 지음. 내 스스로의 네이밍 규칙
    TextMeshProUGUI cText;

    private void Awake()
    {
        cText = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate()
    {
        cText.text = $"Stage {PlayerPrefs.GetInt("Custom Stage")}";
    }

    public void Congratulations()
    {
        cText.text = $"Congratulations!!";
    }
}
