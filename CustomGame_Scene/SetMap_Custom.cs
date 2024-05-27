using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetMap_Custom : MonoBehaviour
{
    [SerializeField] StageText_Custom stageText;
    [SerializeField] List<GameObject> blocks;
    [SerializeField] List<Sprite> blockSprites;
    [SerializeField] List<GameObject> cloneBlock;
    [SerializeField] List<Vector3> blockPos;

    public bool StageDataLoad_Complete { get; private set; }

    private void Awake()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block").ToList();

        foreach (GameObject block in blocks)
        {
            blockPos.Add(block.transform.position);
        }

        blockSprites = new List<Sprite>();
        for (int i = 0; i < 6; i++)
        {
            blockSprites.Add(Resources.Load<Sprite>("pushpush" + i.ToString()));
        }

        if (PlayerPrefs.HasKey("Custom Stage") == false)
        {
            PlayerPrefs.SetInt("Custom Stage", 1);
        }
    }

    private void Start()
    {
        stageText = FindObjectOfType<StageText_Custom>();
        StageDataLoad();
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

    public bool StageDataLoad()
    {
        GameObject go;
        string filePath;

        stageText.TextUpdate();

        // JSON 파일의 경로
        filePath = "MapData/Custom/Stage" + PlayerPrefs.GetInt("Custom Stage").ToString();
        // JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

        if (jsonFile == null)
        {
            return false;
        }
        
        // JSON 파일 내용 문자열로 변환
        string jsonText = jsonFile.text;
        StageData sd = JsonUtility.FromJson<StageData>(jsonText);

        List<int> blockIdx = sd.idx;
        List<BlockType> blockType = sd.blockType;
        for (int i = 0; i < blockIdx.Count; i++)
        {
            BlockType bt = blockType[i];

            SpriteRenderer sr = blocks[blockIdx[i]].GetComponent<SpriteRenderer>();
            switch (bt)
            {
                case BlockType.Player:
                    go = new GameObject();
                    go.transform.position = blocks[blockIdx[i]].transform.position;
                    go.transform.parent = transform;
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
        return true;
    }
}
