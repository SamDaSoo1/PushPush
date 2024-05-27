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
            // 하나라도 빈 집이 있다면 클리어x
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

        // 나중에 없앨 것.
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
        // 플레이어 타임리프
        player.TimeLeap();
        // 공 타임리프
        foreach (Ball ball in balls)
            ball.TimeLeap();
        // 집 타임리프
        foreach (Home home in homes)
            home.TimeLeap();

        blockTypes = past[past.Count - 1];
        past.RemoveAt(past.Count - 1);
    }

    // 과거 블럭 상태를 저장함(3번까지 행적을 기억함)
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

        // 가려는 방향에 벽이 있다면 못감
        if (blockTypes[player.Index + dir] == BlockType.Wall)
        {
            past.RemoveAt(past.Count - 1);
            return;
        }

        // 가려는 방향에 공이 있다면 밀면서 이동
        if (blockTypes[player.Index + dir] == BlockType.Ball)
        {
            // 만약 공이 가려는 방향으로 2개이상 붙어있거나 공 너머에 벽이 있거나 빨간 집이 있다면 밀지못함
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
                    // 매개변수에 따른 함수호출
                    if (dir == up) { ball.MoveUp(); }
                    else if (dir == down) { ball.MoveDown(); }
                    else if (dir == left) { ball.MoveLeft(); }
                    else if (dir == right) { ball.MoveRight(); }

                    foreach (Home home in homes)
                    {
                        // 만약 공이 집안으로 들어갔다면 집은 빨간색으로 바뀜
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

        // 빨간 집을 만났을 때
        if (blockTypes[player.Index + dir] == BlockType.Destroyed_Home)
        {
            // 빨간 집 너머에 벽이 있거나 공이 있거나 빨간 집이 있으면 못움직임
            if (blockTypes[player.Index + dir + dir] == BlockType.Wall || 
                blockTypes[player.Index + dir + dir] == BlockType.Ball || 
                blockTypes[player.Index + dir + dir] == BlockType.Destroyed_Home)
            {
                past.RemoveAt(past.Count - 1);
                return;
            }

            // 빨간 집 너머에 빈 집이 있다면 공을 옮김
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

                // 보이지는 않지만 존재하는 공도 옮긴다
                foreach (Ball ball in balls)
                {
                    if (player.Index + dir == ball.Index)
                    {
                        // 매개변수에 따른 함수호출
                        if (dir == up) { ball.MoveUp(); }
                        else if (dir == down) { ball.MoveDown(); }
                        else if (dir == left) { ball.MoveLeft(); }
                        else if (dir == right) { ball.MoveRight(); }
                    }
                }
            }

            // 빨간 집 너머에 아무것도 없다면 공을 꺼냄
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

                        // 매개변수에 따른 함수호출
                        if (dir == up) { ball.MoveUp(); }
                        else if (dir == down) { ball.MoveDown(); }
                        else if (dir == left) { ball.MoveLeft(); }
                        else if (dir == right) { ball.MoveRight(); }

                        ball.Out_Home();
                    }
                }
            }
        }

        // 플레이어가 지나갔던 자리는 None으로 바꾼다
        blockTypes[player.Index] = BlockType.None;

        // 플레이어가 지나갔던 자리가 빈 집이였다면 Home으로 변경한다
        foreach (Home home in homes)
        {
            if (home.GetComponent<SpriteRenderer>().sprite.name != "pushpush3")
                blockTypes[home.Index] = BlockType.Home;
        }

        // 플레이어의 현재 위치는 Player로 바꾼다
        blockTypes[player.Index + dir] = BlockType.Player;

        // 매개변수에 따른 함수호출
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
