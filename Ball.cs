using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [field: SerializeField]
    public int Index { get; set; } = 0;

    SpriteRenderer sr;

    [SerializeField] List<PreviousLocation> past; 
    [SerializeField] List<PreviousSprite> pastSprite;
    PreviousLocation pl;
    PreviousSprite ps;

    const float moveDist = 0.7f;

    const int up = -13;
    const int down = 13;
    const int left = -1;
    const int right = 1;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        past = new List<PreviousLocation>();
        pastSprite = new List<PreviousSprite>();
        pl = PreviousLocation.None;

        if (sr.enabled == true)
            ps = PreviousSprite.Normal;
        else if (sr.enabled == false)
            ps = PreviousSprite.Destroy;
    }

    public void MoveUp(MoveType mt = MoveType.Current)
    {
        if(mt == MoveType.Current)
            pl = PreviousLocation.Down;

        transform.position += Vector3.up * moveDist;
        Index += up;
    }

    public void MoveDown(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
            pl = PreviousLocation.Up;

        transform.position += Vector3.down * moveDist;
        Index += down;
    }

    public void MoveLeft(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
            pl = PreviousLocation.Right;

        transform.position += Vector3.left * moveDist;
        Index += left;
    }

    public void MoveRight(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
            pl = PreviousLocation.Left;

        transform.position += Vector3.right * moveDist;
        Index += right;
    }

    // 공이 집에 들어가면 안보이게 처리
    public void In_Home()
    {
        sr.enabled = false;
    }

    // 공이 집에서 나오면 보이게 처리
    public void Out_Home()
    {
        sr.enabled = true;
    }

    public void SaveThePast()
    {
        past.Add(pl);
        if (past.Count > 3) { past.RemoveAt(0); }
        pl = PreviousLocation.None;

        pastSprite.Add(ps);
        if (pastSprite.Count > 3) { pastSprite.RemoveAt(0); }

        if (sr.enabled == true)
            ps = PreviousSprite.Normal;
        else if (sr.enabled == false)
            ps = PreviousSprite.Destroy;
    }

    public void TimeLeap()
    {
        if (past[past.Count - 1] == PreviousLocation.Up)
        {
            past.RemoveAt(past.Count - 1);
            MoveUp(MoveType.Past);
        }
        else if (past[past.Count - 1] == PreviousLocation.Down)
        {
            past.RemoveAt(past.Count - 1);
            MoveDown(MoveType.Past);
        }
        else if (past[past.Count - 1] == PreviousLocation.Left)
        {
            past.RemoveAt(past.Count - 1);
            MoveLeft(MoveType.Past);
        }
        else if (past[past.Count - 1] == PreviousLocation.Right)
        {
            past.RemoveAt(past.Count - 1);
            MoveRight(MoveType.Past);
        }
        else if (past[past.Count - 1] == PreviousLocation.None)
        {
            past.RemoveAt(past.Count - 1);
        }

        if (pastSprite[pastSprite.Count - 1] == PreviousSprite.Normal)
        {
            pastSprite.RemoveAt(pastSprite.Count - 1);
            Out_Home();
            ps = PreviousSprite.Normal;
        }
        else if (pastSprite[pastSprite.Count - 1] == PreviousSprite.Destroy)
        {
            pastSprite.RemoveAt(pastSprite.Count - 1);
            In_Home();
            ps = PreviousSprite.Destroy;
        }
    }
}
