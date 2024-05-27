using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field : SerializeField]
    public int Index { get; set; } = 0;

    [SerializeField] List<PreviousLocation> past;
    PreviousLocation pl;

    const float moveDist = 0.7f;

    const int up = -13;
    const int down = 13;
    const int left = -1;
    const int right = 1;

    private void Awake()
    {
        past = new List<PreviousLocation>();
        pl = PreviousLocation.None;
    }

    public void MoveUp(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
        {
            pl = PreviousLocation.Down;
            SaveThePast();
        }
            
        transform.position += Vector3.up * moveDist;
        Index += up;
    }

    public void MoveDown(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
        {
            pl = PreviousLocation.Up;
            SaveThePast();
        }
            
        transform.position += Vector3.down * moveDist;
        Index += down;
    }

    public void MoveLeft(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
        {
            pl = PreviousLocation.Right;
            SaveThePast();
        }
            
        transform.position += Vector3.left * moveDist;
        Index += left;
    }

    public void MoveRight(MoveType mt = MoveType.Current)
    {
        if (mt == MoveType.Current)
        {
            pl = PreviousLocation.Left;
            SaveThePast();
        }
            
        transform.position += Vector3.right * moveDist;
        Index += right;
    }

    void SaveThePast()
    {
        past.Add(pl);
        if (past.Count > 3) { past.RemoveAt(0); }
        pl = PreviousLocation.None;
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
        else if(past[past.Count - 1] == PreviousLocation.None)
        {
            past.RemoveAt(past.Count - 1);
        }
    }
}
