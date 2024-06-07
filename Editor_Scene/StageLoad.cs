using System.Collections.Generic;
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
        TextAsset jsonFile;
        string jsonText;
        StageData sd;

#if UNITY_EDITOR || UNITY_STANDALONE
        // JSON ������ ���
        filePath = $"MapData/Custom/Stage{stageNum}";
        
        // JSON ���� �ε�
        jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }

        // JSON ���� ���� ���ڿ��� ��ȯ
        jsonText = jsonFile.text;
        sd = JsonUtility.FromJson<StageData>(jsonText);
#elif UNITY_ANDROID
        // JSON ���� �ε�
        string _jsonFile = PlayerPrefs.GetString("Custom Stage" + stageNum);

        if (string.IsNullOrEmpty(_jsonFile))
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }
        // JSON ���� ���� ���ڿ��� ��ȯ
        sd = JsonUtility.FromJson<StageData>(_jsonFile);
#endif

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