using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorScene_ButtonEvent : MonoBehaviour
{
    StageText_Editor stageText;
    [SerializeField] GameObject SelectBlock;
    [SerializeField] SpriteRenderer palette;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<SpriteRenderer> blocks;
    [SerializeField] TextMeshProUGUI saveText;
    [SerializeField] StageData stageData;
    [SerializeField] TextMeshProUGUI saveTmp;
    StageLoad stageLoad;

    [SerializeField] GameObject line;
    [SerializeField] SetMap_Editor setMap;
    
    const float dist = 0.7f;

    int limitX = 6;
    int limitY = 6;

    float textOffset = 15;

    int idx = 84;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Editor Stage") == false)
        {
            PlayerPrefs.SetInt("Editor Stage", 1);
            PlayerPrefs.Save();
        }

        stageText = FindObjectOfType<StageText_Editor>();
        SelectBlock = GameObject.FindWithTag("SelectBlock");
        SelectBlock.transform.position = new Vector3(-3.8f, 0f, 0f);
        palette = SelectBlock.GetComponent<SpriteRenderer>();

        sprites = new List<Sprite>();
        for(int i = 0; i < 6; i++)
        {
            sprites.Add(Resources.Load<Sprite>("pushpush" + i.ToString()));
        }

        blocks = new List<SpriteRenderer>();
        List<GameObject> goList = GameObject.FindGameObjectsWithTag("Block").ToList();
        goList.Sort((a, b) => 
        {
            int numA = int.Parse(a.name);
            int numB = int.Parse(b.name);
            return numA.CompareTo(numB);
        });

        foreach (GameObject go in goList)
        {
            blocks.Add(go.GetComponent<SpriteRenderer>());
        }

        saveText = GameObject.Find("Save Button").GetComponentInChildren<TextMeshProUGUI>();
        stageLoad = FindObjectOfType<StageLoad>();
        stageLoad.MapLoad(blocks, PlayerPrefs.GetInt("Editor Stage"));

        saveTmp = transform.Find("SaveTmp").GetComponent<TextMeshProUGUI>();
        saveTmp.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            UpArrowClick();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            DownArrowClick();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            LeftArrowClick();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            RightArrowClick();
    }

    public void HomeButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void SaveButton_OnPointerDown()
    {
        saveText.rectTransform.anchoredPosition = new Vector2(saveText.rectTransform.anchoredPosition.x, saveText.rectTransform.anchoredPosition.y - textOffset);
    }

    public void SaveButton_OnPointerUp()
    {
        SoundManager.Instance.PlaySFX(Sfx.Success);
        saveText.rectTransform.anchoredPosition = new Vector2(saveText.rectTransform.anchoredPosition.x, saveText.rectTransform.anchoredPosition.y + textOffset);

        if(saveTmp.enabled == true) 
        {
            StartCoroutine(SaveTmp());
            return; 
        }

        stageData.idx.Clear();
        stageData.blockType.Clear();

        foreach (SpriteRenderer block in blocks)
        {
            Sprite blockSprite = block.sprite;
            if (blockSprite == null)
                continue;

            switch (blockSprite.name)
            {
                case "pushpush0":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Player);
                    break;
                case "pushpush1":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Ball);
                    break;
                case "pushpush2":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Home);
                    break;
                case "pushpush3":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Destroyed_Home);
                    break;
                case "pushpush4":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Wall);
                    break;
                case "pushpush5":
                    stageData.idx.Add(int.Parse(block.name));
                    stageData.blockType.Add(BlockType.Floor);
                    break;
                default:
                    break;
            }
        }

        // 데이터 직렬화
        string jsonData = JsonUtility.ToJson(stageData, true);

        // PlayerPrefs로 저장
        PlayerPrefs.SetString("Custom Stage" + PlayerPrefs.GetInt("Editor Stage"), jsonData);
        PlayerPrefs.Save();

        if (saveTmp.enabled == false)
        {
            saveTmp.enabled = true;
            StartCoroutine(SaveTmp());
        }

        /// 이미 생성된 파일인지 체크하고 싶으면 사용
        // if (File.Exists(path) == true) { }
    }

    IEnumerator SaveTmp()
    {
        yield return new WaitForSeconds(1.5f);
        saveTmp.enabled = false;
    }

    public void Palette1()
    {
        palette.sprite = sprites[0];
    }

    public void Palette2()
    {
        palette.sprite = sprites[1];
    }

    public void Palette3()
    {
        palette.sprite = sprites[2];
    }

    public void Palette4()
    {
        palette.sprite = sprites[3];
    }

    public void Palette5()
    {
        palette.sprite = sprites[4];
    }

    public void Palette6()
    {
        palette.sprite = sprites[5];
    }

    public void UpArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.up * dist * line.transform.localScale.x;
        if (limitY == 0)
            return;

        SoundManager.Instance.PlaySFX(Sfx.Move);
        SelectBlock.transform.position = nextMove;

        limitY -= 1;
        idx -= 13;
    }

    public void DownArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.down * dist * line.transform.localScale.x;
        if (limitY == 12)
            return;

        SoundManager.Instance.PlaySFX(Sfx.Move);
        SelectBlock.transform.position = nextMove;

        limitY += 1;
        idx += 13;
    }

    public void LeftArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.left * dist * line.transform.localScale.x;
        if (limitX == 0)
            return;

        SoundManager.Instance.PlaySFX(Sfx.Move);
        SelectBlock.transform.position = nextMove;

        limitX -= 1;
        idx -= 1;
    }

    public void RightArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.right * dist * line.transform.localScale.x;
        if (limitX == 12)
            return;

        SoundManager.Instance.PlaySFX(Sfx.Move);
        SelectBlock.transform.position = nextMove;

        limitX += 1;
        idx += 1;
    }

    public void DrawClick()
    {
        if (palette.sprite == null) { return; }

        blocks[idx].sprite = palette.sprite;
    }

    public void EraseClick()
    {
        blocks[idx].sprite = null;
    }

    public void AllDeleteClick()
    {
        for(int i = 0; i < blocks.Count; i++)
        {
            blocks[i].sprite = null;
        }
    }

    public void StageUpButton()
    {
        if (PlayerPrefs.GetInt("Editor Stage") == 50)
            return;

        int stageNum = PlayerPrefs.GetInt("Editor Stage");
        PlayerPrefs.SetInt("Editor Stage", stageNum + 1);
        PlayerPrefs.Save();
        stageText.TextUpdate();

        setMap.ResetScale();
        stageLoad.MapLoad(blocks, PlayerPrefs.GetInt("Editor Stage"));
        setMap.SetScale();
    }

    public void StageDownButton()
    {
        if (PlayerPrefs.GetInt("Editor Stage") == 1)
            return;

        int stageNum = PlayerPrefs.GetInt("Editor Stage");
        PlayerPrefs.SetInt("Editor Stage", stageNum - 1);
        PlayerPrefs.Save();
        stageText.TextUpdate();

        setMap.ResetScale();
        stageLoad.MapLoad(blocks, PlayerPrefs.GetInt("Editor Stage"));
        setMap.SetScale();
    }
}
