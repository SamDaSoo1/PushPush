using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageText : MonoBehaviour
{
    // component�� text�� �ؼ� cText��� ����. �� �������� ���̹� ��Ģ
    TextMeshProUGUI cText;

    private void Awake()
    {
        cText = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate()
    {
        cText.text = $"Stage {PlayerPrefs.GetInt("Stage")}";
    }

    public void Congratulations()
    {
        cText.text = $"Congratulations!!";
    }
}
