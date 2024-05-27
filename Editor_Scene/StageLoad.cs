using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StageLoad : MonoBehaviour
{
    List<Sprite> sprites;

    private void Awake()
    {
        sprites = new List<Sprite>()
        {
            Resources.Load<Sprite>("pushpush0"),
            Resources.Load<Sprite>("pushpush1"),
            Resources.Load<Sprite>("pushpush2"),
            Resources.Load<Sprite>("pushpush3"),
            Resources.Load<Sprite>("pushpush4"),
            Resources.Load<Sprite>("pushpush5")
        };

    }

    public void MapLoad(List<SpriteRenderer> blocks, int stageNum)
    {
        string filePath;

        // JSON ������ ���
        filePath = $"MapData/Custom/Stage{stageNum}";

        // JSON ���� �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }

        // JSON ���� ���� ���ڿ��� ��ȯ
        string jsonText = jsonFile.text;
        StageData sd = JsonUtility.FromJson<StageData>(jsonText);

        // �ѹ� �� �а�
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].sprite = null;
        }

        // �� ������ blocks�� ������Ѽ� �̹����� �ٲٱ�
        for (int i = 0; i < sd.blockType.Count; i++)
        {
            blocks[sd.idx[i]].sprite = sprites[(int)sd.blockType[i]];
        }
    }
}
