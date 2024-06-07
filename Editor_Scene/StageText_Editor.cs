using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageText_Editor : MonoBehaviour
{
    // component�� text�� �ؼ� cText��� ����. �� �������� ���̹� ��Ģ
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
