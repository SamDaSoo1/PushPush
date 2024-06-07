using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SetMap : MonoBehaviour
{
    [SerializeField] StageText stageText;
    [SerializeField] List<GameObject> blocks;
    [SerializeField] List<Sprite> blockSprites;
    [SerializeField] List<GameObject> cloneBlock;
    [SerializeField] List<Vector3> blockPos;

    [SerializeField] GameObject bgd;
    [SerializeField] GameObject canvas;

    public bool StageDataLoad_Complete { get; private set; }

    private void Start()
    {        
        // 안드로이드에선 순서가 뒤죽박죽 바뀐다.. 이름기준으로 정렬 ㄱ
        // 난 당연히 이름순으로 정렬될 줄 알았는데 착각이였다..
        // 플랫폼에 따라 반환되는 순서가 다를 수 있기 때문에
        // 100% 정렬되게 하려면 실제로 정렬하는 코드를 써야했다..
        blocks = GameObject.FindGameObjectsWithTag("Block").ToList();
        blocks.Sort((a, b) => 
        {
            int numA = int.Parse(a.name);
            int numB = int.Parse(b.name);
            return numA.CompareTo(numB);
        });

        blockSprites = new List<Sprite>();
        for (int i = 0; i < 6; i++)
        {
            blockSprites.Add(Resources.Load<Sprite>("pushpush" + i.ToString()));
        }

        if (PlayerPrefs.HasKey("Stage") == false)
        {            
            PlayerPrefs.SetInt("Stage", 1);
            PlayerPrefs.Save();
        }

        stageText = FindObjectOfType<StageText>();
        StageDataLoad();
        transform.position = new Vector3(bgd.GetComponent<RectTransform>().position.x, bgd.GetComponent<RectTransform>().position.y, 0);

        foreach (GameObject block in blocks)
        {
            blockPos.Add(block.transform.position);
        }

        SetScale();
    }

    //-----------테스트용 버튼------------

    public void Btn()
    {
        FindObjectOfType<Administrator>().NextStage();
    }

    //-----------테스트용 버튼------------

    public void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    public void SetScale()
    {
        transform.localScale = new Vector3(canvas.transform.localScale.x / 0.0092592f, canvas.transform.localScale.y / 0.0092592f, canvas.transform.localScale.z / 0.0092592f);
    }

    public void StageInit()
    {
        StageDataLoad_Complete = false;
        int idx = 0;
        foreach (GameObject block in blocks)
        {
            block.transform.position = blockPos[idx++];

            SpriteRenderer sr = block.GetComponent<SpriteRenderer>();
            Home h = block.GetComponent<Home>();
            Ball b = block.GetComponent<Ball>();
            Player p = block.GetComponent<Player>();

            if (sr != null)
            {
                sr.enabled = true;
                sr.sprite = null;
                sr.sortingOrder = 0;
            }

            if (h != null) { DestroyImmediate(h); }
            if (b != null) { DestroyImmediate(b); }
            if (p != null) { DestroyImmediate(p); }
        }

        foreach (GameObject clone in cloneBlock)
        {
            DestroyImmediate(clone);
        }

        cloneBlock.Clear();
    }

    public void StageDataLoad()
    {
        GameObject go;
        string filePath;
        //string jsonData;
        
        stageText.TextUpdate();

        // JSON 파일의 경로
        filePath = "MapData/Original/Stage" + PlayerPrefs.GetInt("Stage").ToString();
        // JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        // JSON 파일 내용 문자열로 변환
        string jsonText = jsonFile.text;
        StageData sd = JsonUtility.FromJson<StageData>(jsonText);

        List<int> blockIdx = sd.idx;
        List<BlockType> blockType = sd.blockType;

       /* // JSON 파일 로드
        filePath = Path.Combine(Application.streamingAssetsPath, "Stage" + PlayerPrefs.GetInt("Stage") + ".json");

#if UNITY_EDITOR
        // JSON 파일 내용 문자열로 변환
        jsonData = File.ReadAllText(filePath);
#elif UNITY_ANDROID
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        www.SendWebRequest();
        while (!www.isDone) { }
        jsonData = www.downloadHandler.text;
#endif
        // 문자열을 기반으로 역직렬화
        StageData sd = JsonUtility.FromJson<StageData>(jsonData);
        blockIdx = sd.idx;
        blockType = sd.blockType;*/

        for (int i = 0; i < blockIdx.Count; i++)
        {
            BlockType bt = blockType[i];

            SpriteRenderer sr = blocks[blockIdx[i]].GetComponent<SpriteRenderer>();

            switch (bt)
            {
                case BlockType.Player:
                    go = new GameObject();
                    go.transform.parent = transform;
                    go.transform.position = blocks[blockIdx[i]].transform.position;
                    go.transform.position = blocks[blockIdx[i]].transform.position;
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = blockSprites[5];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    cloneBlock.Add(go);

                    sr.sprite = blockSprites[0];
                    sr.sortingOrder = 3;
                    blocks[blockIdx[i]].AddComponent<Player>();
                    blocks[blockIdx[i]].GetComponent<Player>().Index = blockIdx[i];
                    break;

                case BlockType.Ball:
                    go = new GameObject();
                    go.transform.position = blocks[blockIdx[i]].transform.position;
                    go.transform.parent = transform;
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = blockSprites[5];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    cloneBlock.Add(go);

                    sr.sprite = blockSprites[1];
                    sr.sortingOrder = 3;
                    blocks[blockIdx[i]].AddComponent<Ball>();
                    blocks[blockIdx[i]].GetComponent<Ball>().Index = blockIdx[i];
                    break;

                case BlockType.Home:
                    sr.sprite = blockSprites[2];
                    sr.sortingOrder = 2;
                    blocks[blockIdx[i]].AddComponent<Home>();
                    blocks[blockIdx[i]].GetComponent<Home>().Index = blockIdx[i];
                    break;

                case BlockType.Destroyed_Home:
                    go = new GameObject();
                    go.transform.position = blocks[blockIdx[i]].transform.position;
                    go.transform.parent = transform;
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = blockSprites[1];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    go.GetComponent<SpriteRenderer>().enabled = false;
                    go.AddComponent<Ball>();
                    go.GetComponent<Ball>().Index = blockIdx[i];
                    cloneBlock.Add(go);

                    sr.sprite = blockSprites[3];
                    sr.sortingOrder = 2;
                    blocks[blockIdx[i]].AddComponent<Home>();
                    blocks[blockIdx[i]].GetComponent<Home>().Index = blockIdx[i];
                    break;

                case BlockType.Wall:
                    sr.sprite = blockSprites[4];
                    sr.sortingOrder = 2;
                    break;

                case BlockType.Floor:
                    sr.sprite = blockSprites[5];
                    sr.sortingOrder = 2;
                    break;

                default:
                    break;
            }
        }

        StageDataLoad_Complete = true;
    }
}