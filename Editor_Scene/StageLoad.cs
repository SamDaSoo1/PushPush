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
        StageData sd;

#if UNITY_EDITOR || UNITY_STANDALONE
        // JSON 파일의 경로
        filePath = PlayerPrefs.GetString("Custom Stage" + stageNum);

        // JSON 파일 로드
        if (string.IsNullOrEmpty(filePath))
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }

        // JSON 파일 내용 문자열로 변환
        sd = JsonUtility.FromJson<StageData>(filePath);
#elif UNITY_ANDROID
        // JSON 파일 로드
        string _jsonFile = PlayerPrefs.GetString("Custom Stage" + stageNum);

        if (string.IsNullOrEmpty(_jsonFile))
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].sprite = null;
            }
            return;
        }
        // JSON 파일 내용 문자열로 변환
        sd = JsonUtility.FromJson<StageData>(_jsonFile);
#endif

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