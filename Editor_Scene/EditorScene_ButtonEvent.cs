using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorScene_ButtonEvent : MonoBehaviour
{
    [SerializeField] GameObject SelectBlock;
    [SerializeField] SpriteRenderer palette;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<GameObject> blocks;
    [SerializeField] Text saveText;
    [SerializeField] StageData stageData;
    [SerializeField] int stageNum;
    

    const float dist = 0.7f;
    const float limit_Up = 4.5f;
    const float limit_Down = -4.5f;
    const float limit_Left = -8.5f;
    const float limit_Right = 1f;
    float textOffset = 15;

    int idx = 84;

    private void Start()
    {
        SelectBlock = GameObject.FindWithTag("SelectBlock");
        SelectBlock.transform.position = new Vector3(-3.8f, 0f, 0f);
        palette = SelectBlock.GetComponent<SpriteRenderer>();

        sprites = new List<Sprite>();
        for(int i = 0; i < 6; i++)
        {
            sprites.Add(Resources.Load<Sprite>("pushpush" + i.ToString()));
        }

        blocks = new List<GameObject>();
        blocks = GameObject.FindGameObjectsWithTag("Block").ToList();

        saveText = GameObject.Find("Save Button").GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
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
        saveText.rectTransform.anchoredPosition = new Vector2(saveText.rectTransform.anchoredPosition.x, saveText.rectTransform.anchoredPosition.y + textOffset);

        if(stageNum > 0)
        {
            foreach(GameObject block in blocks)
            {
                Sprite blockSprite = block.GetComponent<SpriteRenderer>().sprite;
                if (blockSprite == null)
                    continue;
                       
                switch(blockSprite.name)
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

            // �����͸� ������ ��� ����
            string path = Path.Combine(Application.dataPath + "/Resources/MapData/Original", "Stage" + stageNum.ToString() + ".json");
            // ToJson�� ����ϸ� JSON���·� �����õ� ���ڿ��� �����ȴ�  
            string jsonData = JsonUtility.ToJson(stageData, true);
            // ���� ���� �� ����
            File.WriteAllText(path, jsonData);
            print("���� �Ϸ�");

            /// �̹� ������ �������� üũ�ϰ� ������ ���
            //if (File.Exists(path) == false) { }
        }
        else
        {
            print("�������: stageNum�� �Է��� �ּ���.");
        }
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
        Vector3 nextMove = SelectBlock.transform.position + Vector3.up * dist;
        if (nextMove.y > limit_Up)
            return;

        SelectBlock.transform.position = nextMove;
        idx -= 13;
    }

    public void DownArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.down * dist;
        if (nextMove.y < limit_Down)
            return;

        SelectBlock.transform.position = nextMove;
        idx += 13;
    }

    public void LeftArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.left * dist;
        if (nextMove.x < limit_Left)
            return;

        SelectBlock.transform.position = nextMove;
        idx -= 1;
    }

    public void RightArrowClick()
    {
        Vector3 nextMove = SelectBlock.transform.position + Vector3.right * dist;
        if (nextMove.x > limit_Right)
            return;

        SelectBlock.transform.position = nextMove;
        idx += 1;
    }

    public void DrawClick()
    {
        blocks[idx].GetComponent<SpriteRenderer>().sprite = palette.sprite;
    }

    public void EraseClick()
    {
        blocks[idx].GetComponent<SpriteRenderer>().sprite = null;
    }
}
