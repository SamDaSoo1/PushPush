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

        // JSON 파일의 경로
        filePath = $"MapData/Custom/Stage{stageNum}";

        // JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }

        // JSON 파일 내용 문자열로 변환
        string jsonText = jsonFile.text;
        StageData sd = JsonUtility.FromJson<StageData>(jsonText);

        // 한번 싹 밀고
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].sprite = null;
        }

        // 그 내용을 blocks에 적용시켜서 이미지만 바꾸기
        for (int i = 0; i < sd.blockType.Count; i++)
        {
            blocks[sd.idx[i]].sprite = sprites[(int)sd.blockType[i]];
        }
    }
}
