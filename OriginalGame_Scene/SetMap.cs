using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetMap : MonoBehaviour
{
    [SerializeField] Text stageText;
    [SerializeField] List<GameObject> blocks;
    [SerializeField] List<Sprite> blockSprites;
    [SerializeField] List<GameObject> cloneBlock;
    [SerializeField] List<Vector3> blockPos;

    public bool StageDataLoad_Complete {  get; private set; }

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

        if(PlayerPrefs.HasKey("Stage") == false)
        {
            PlayerPrefs.SetInt("Stage", 1);
        }
    }

    private void Start()
    {
        stageText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>();
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

            if (h != null) { Destroy(h); }
            if (b != null) { Destroy(b); }
            if (p != null) { Destroy(p); }
        }

        foreach (GameObject clone in cloneBlock)
        {
            Destroy(clone);
        }

        cloneBlock.Clear();
    }

    public void StageDataLoad()
    {
        GameObject go;

        // JSON 파일의 경로
        //string filePath = "MapData/SavePoint";
        // JSON 파일 로드
        //TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        // JSON 파일 내용 문자열로 변환
        //string jsonText = jsonFile.text;
        //SavePoint savePoint = JsonUtility.FromJson<SavePoint>(jsonText);

        stageText.text = $"Stage {PlayerPrefs.GetInt("Stage")}";

        string filePath2 = "MapData/Original/Stage" + PlayerPrefs.GetInt("Stage").ToString();
        TextAsset jsonFile2 = Resources.Load<TextAsset>(filePath2);
        string jsonText2 = jsonFile2.text;
        StageData sd = JsonUtility.FromJson<StageData>(jsonText2);

        List<int> blockIdx = sd.idx;
        List<BlockType> blockType = sd.blockType;
        for(int i = 0; i < blockIdx.Count; i++)
        {
            BlockType bt = blockType[i];

            SpriteRenderer sr = blocks[blockIdx[i]].GetComponent<SpriteRenderer>();
            switch (bt)
            {
                case BlockType.Player:
                    go = Instantiate(blocks[blockIdx[i]], transform);
                    go.GetComponent<SpriteRenderer>().sprite = blockSprites[5];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    cloneBlock.Add(go);

                    sr.sprite = blockSprites[0];
                    sr.sortingOrder = 3;
                    blocks[blockIdx[i]].AddComponent<Player>();
                    blocks[blockIdx[i]].GetComponent<Player>().Index = blockIdx[i];
                    break;

                case BlockType.Ball:
                    go = Instantiate(blocks[blockIdx[i]], transform);
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
                    go = Instantiate(blocks[blockIdx[i]], transform);
                    go.AddComponent<Ball>();
                    go.GetComponent<Ball>().Index = blockIdx[i];
                    go.GetComponent<SpriteRenderer>().sprite = blockSprites[1];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    go.GetComponent<SpriteRenderer>().enabled = false;
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
