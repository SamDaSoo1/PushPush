using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderData;

public class Administrator : MonoBehaviour
{
    StageText stageText;
    [SerializeField] List<BlockType> blockTypes;
    [SerializeField] SetMap setMap;
    [SerializeField] Player player;
    [SerializeField] List<Ball> balls;
    [SerializeField] List<Home> homes;
    [SerializeField] List<List<BlockType>> past;
    [SerializeField] Step step;
    [SerializeField] Heart heart;
    [SerializeField] GameObject stageUpButton;
    [SerializeField] GameObject stageDownButton;
    [SerializeField] GameObject buttonBlock;

    const int up = -13;
    const int down = 13;
    const int left = -1;
    const int right = 1;

    void Start()
    {
        stageText = FindObjectOfType<StageText>();
        setMap = FindObjectOfType<SetMap>();
        StartCoroutine(WaitSetMap());
        past = new List<List<BlockType>>();
        step = FindObjectOfType<Step>();
        heart = FindObjectOfType<Heart>();

        if (PlayerPrefs.HasKey("First clear") == true)
        {
            stageUpButton.SetActive(true);
            stageDownButton.SetActive(true);
        }
        else
        {
            stageUpButton.SetActive(false);
            stageDownButton.SetActive(false);
        }

        buttonBlock.SetActive(false);
    }

    void StageClearCheck()
    {
        foreach (Home home in homes)
        {
            // �ϳ��� �� ���� �ִٸ� Ŭ����x
            if (home.sr.sprite.name == "pushpush2")
            {
                return;
            }   
        }
        NextStage();
        
    }

    public void PrevStage()
    {
        heart.Init();
        step.Reset_Step();
        setMap.StageInit();
        player = null;
        blockTypes.Clear();
        balls.Clear();
        homes.Clear();
        past.Clear();
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") - 1);

        setMap.StageDataLoad();
        StartCoroutine(WaitSetMap());
    }

    public void NextStage()
    {
        heart.Init();
        step.Reset_Step();
        setMap.StageInit();
        player = null;
        blockTypes.Clear();
        balls.Clear();
        homes.Clear();
        past.Clear();
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);

        if (PlayerPrefs.GetInt("Stage") > 50)
        {
            PlayerPrefs.SetInt("Stage", 1);
            buttonBlock.SetActive(true);
            FindObjectOfType<Background>().BackgroundChange();
            stageUpButton.SetActive(false);
            stageDownButton.SetActive(false);

            if (PlayerPrefs.HasKey("First clear") == false)
                PlayerPrefs.SetInt("First clear", 1);

            heart.Fill_All_Heart();
            stageText.Congratulations();
            step.TheEnd();
            return;
        }

        setMap.StageDataLoad();
        StartCoroutine(WaitSetMap());
    }

    IEnumerator WaitSetMap()
    {
        yield return new WaitUntil(() => setMap.StageDataLoad_Complete == true);

        GameObject stage = GameObject.Find("Stage");
        foreach (Transform child in stage.transform)
        {
            if (child.gameObject.name == "New Game Object")
            {
                if(child.GetComponent<Ball>() != null)
                {
                    balls.Add(child.GetComponent<Ball>());
                }
                continue;
            }

            if (child.GetComponent<SpriteRenderer>().sprite == null || 
                child.GetComponent<SpriteRenderer>().sprite.name == "pushpush5")
                blockTypes.Add(BlockType.None);
            else if (child.GetComponent<SpriteRenderer>().sprite.name == "pushpush0")
            {
                blockTypes.Add(BlockType.Player);
                player = child.GetComponent<Player>();
            }
            else if (child.GetComponent<SpriteRenderer>().sprite.name == "pushpush1")
            {
                blockTypes.Add(BlockType.Ball);
                balls.Add(child.GetComponent<Ball>());
            }
            else if (child.GetComponent<SpriteRenderer>().sprite.name == "pushpush2")
            {
                blockTypes.Add(BlockType.Home);
                homes.Add(child.GetComponent<Home>());
            }
            else if (child.GetComponent<SpriteRenderer>().sprite.name == "pushpush3")
            {
                blockTypes.Add(BlockType.Destroyed_Home);
                homes.Add(child.GetComponent<Home>());
            }
            else if (child.GetComponent<SpriteRenderer>().sprite.name == "pushpush4")
                blockTypes.Add(BlockType.Wall);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
            MovePlayer(up);
         
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MovePlayer(down);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MovePlayer(left);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            MovePlayer(right);

        if (Input.GetKeyDown(KeyCode.Z))
            TimeLeap();

        // ���߿� ���� ��.
        if (Input.GetKeyDown(KeyCode.P))
            NextStage();
    }

    public void Move(ButtonType type)
    {
        switch(type)
        {
            case ButtonType.UpArrow:
                MovePlayer(up);
                break;
            case ButtonType.DownArrow:
                MovePlayer(down);
                break;
            case ButtonType.LeftArrow:
                MovePlayer(left);
                break;
            case ButtonType.RightArrow:
                MovePlayer(right);
                break;
            case ButtonType.TimeLeap:
                TimeLeap();
                break;
            default:
                Debug.LogError("Move Error");
                break;
        }
    }

    void TimeLeap()
    {
        if (past.Count == 0) { return; }

        heart.Empty_Heart();
        // �÷��̾� Ÿ�Ӹ���
        player.TimeLeap();
        // �� Ÿ�Ӹ���
        foreach (Ball ball in balls)
            ball.TimeLeap();
        // �� Ÿ�Ӹ���
        foreach (Home home in homes)
            home.TimeLeap();

        blockTypes = past[past.Count - 1];
        past.RemoveAt(past.Count - 1);
    }

    // ���� �� ���¸� ������(3������ ������ �����)
    void SaveThePast()
    {
        past.Add(new List<BlockType>(blockTypes));
        if (past.Count > 3)
        {
            past.RemoveAt(0);
        }
    }

    void MovePlayer(int dir)
    {
        SaveThePast();

        // ������ ���⿡ ���� �ִٸ� ����
        if (blockTypes[player.Index + dir] == BlockType.Wall)
        {
            past.RemoveAt(past.Count - 1);
            return;
        }

        // ������ ���⿡ ���� �ִٸ� �и鼭 �̵�
        if (blockTypes[player.Index + dir] == BlockType.Ball)
        {
            // ���� ���� ������ �������� 2���̻� �پ��ְų� �� �ʸӿ� ���� �ְų� ���� ���� �ִٸ� ��������
            if (blockTypes[player.Index + dir + dir] == BlockType.Ball || 
                blockTypes[player.Index + dir + dir] == BlockType.Wall ||
                blockTypes[player.Index + dir + dir] == BlockType.Destroyed_Home)
            {
                past.RemoveAt(past.Count - 1);
                return;
            }  

            blockTypes[player.Index + dir] = BlockType.None;
            blockTypes[player.Index + dir + dir] = BlockType.Ball;

            foreach (Ball ball in balls)
            {
                if (player.Index + dir == ball.Index)
                {
                    // �Ű������� ���� �Լ�ȣ��
                    if (dir == up) { ball.MoveUp(); }
                    else if (dir == down) { ball.MoveDown(); }
                    else if (dir == left) { ball.MoveLeft(); }
                    else if (dir == right) { ball.MoveRight(); }

                    foreach (Home home in homes)
                    {
                        // ���� ���� �������� ���ٸ� ���� ���������� �ٲ�
                        if (ball.Index == home.Index)
                        {
                            ball.In_Home();
                            home.Ball_In();
                            blockTypes[home.Index] = BlockType.Destroyed_Home;
                        }
                    }
                    break;
                }
            }
        }

        // ���� ���� ������ ��
        if (blockTypes[player.Index + dir] == BlockType.Destroyed_Home)
        {
            // ���� �� �ʸӿ� ���� �ְų� ���� �ְų� ���� ���� ������ ��������
            if (blockTypes[player.Index + dir + dir] == BlockType.Wall || 
                blockTypes[player.Index + dir + dir] == BlockType.Ball || 
                blockTypes[player.Index + dir + dir] == BlockType.Destroyed_Home)
            {
                past.RemoveAt(past.Count - 1);
                return;
            }

            // ���� �� �ʸӿ� �� ���� �ִٸ� ���� �ű�
            if (blockTypes[player.Index + dir + dir] == BlockType.Home)
            {
                foreach (Home home in homes)
                {
                    if (player.Index + dir + dir == home.Index)
                    {
                        blockTypes[player.Index + dir + dir] = BlockType.Destroyed_Home;
                        home.Ball_In();
                    }
                    else if (player.Index + dir == home.Index)
                    {
                        blockTypes[player.Index + dir] = BlockType.Home;
                        home.Ball_Out();
                    }
                }

                // �������� ������ �����ϴ� ���� �ű��
                foreach (Ball ball in balls)
                {
                    if (player.Index + dir == ball.Index)
                    {
                        // �Ű������� ���� �Լ�ȣ��
                        if (dir == up) { ball.MoveUp(); }
                        else if (dir == down) { ball.MoveDown(); }
                        else if (dir == left) { ball.MoveLeft(); }
                        else if (dir == right) { ball.MoveRight(); }
                    }
                }
            }

            // ���� �� �ʸӿ� �ƹ��͵� ���ٸ� ���� ����
            if (blockTypes[player.Index + dir + dir] == BlockType.None)
            {
                foreach (Home home in homes)
                {
                    if (player.Index + dir == home.Index)
                    {
                        blockTypes[player.Index + dir] = BlockType.Home;
                        home.Ball_Out();
                    }
                }

                foreach (Ball ball in balls)
                {
                    if (player.Index + dir == ball.Index)
                    {
                        blockTypes[player.Index + dir + dir] = BlockType.Ball;

                        // �Ű������� ���� �Լ�ȣ��
                        if (dir == up) { ball.MoveUp(); }
                        else if (dir == down) { ball.MoveDown(); }
                        else if (dir == left) { ball.MoveLeft(); }
                        else if (dir == right) { ball.MoveRight(); }

                        ball.Out_Home();
                    }
                }
            }
        }

        // �÷��̾ �������� �ڸ��� None���� �ٲ۴�
        blockTypes[player.Index] = BlockType.None;

        // �÷��̾ �������� �ڸ��� �� ���̿��ٸ� Home���� �����Ѵ�
        foreach (Home home in homes)
        {
            if (home.GetComponent<SpriteRenderer>().sprite.name != "pushpush3")
                blockTypes[home.Index] = BlockType.Home;
        }

        // �÷��̾��� ���� ��ġ�� Player�� �ٲ۴�
        blockTypes[player.Index + dir] = BlockType.Player;

        // �Ű������� ���� �Լ�ȣ��
        if (dir == up) { player.MoveUp(); }
        else if (dir == down) { player.MoveDown(); }
        else if (dir == left) { player.MoveLeft(); }
        else if (dir == right) { player.MoveRight(); }

        foreach (Ball ball in balls)
        {
            ball.SaveThePast();
        }

        foreach (Home home in homes)
        {
            home.SaveThePast();
        }

        step.OneStep();
        heart.Fill_Heart();
        StageClearCheck();
    }
}
