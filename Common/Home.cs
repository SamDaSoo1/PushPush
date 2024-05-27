using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [field: SerializeField]
    public int Index { get; set; } = 0;

    [SerializeField] List<Sprite> sprites;
    public SpriteRenderer sr;

    [SerializeField] List<PreviousSprite> past;
    [SerializeField] PreviousSprite ps;

    private void Awake()
    {
        sprites = new List<Sprite>
        {
            Resources.Load<Sprite>("pushpush2"),
            Resources.Load<Sprite>("pushpush3")
        };

        sr = GetComponent<SpriteRenderer>();

        past = new List<PreviousSprite>();

        if (sr.sprite.name == "pushpush2")
            ps = PreviousSprite.Normal;
        else if (sr.sprite.name == "pushpush3")
            ps = PreviousSprite.Destroy;
    }

    // 집안에 공이 들어오면 빨간 집으로 바꿈
    public void Ball_In()
    {
        //ps = PreviousSprite.Destroy;
        sr.sprite = sprites[1];
    }

    // 집 밖으로 공이 나가면 원래 집으로 바꿈
    public void Ball_Out()
    {
        //ps = PreviousSprite.Normal;
        sr.sprite = sprites[0];
    }

    public void SaveThePast()
    {
        past.Add(ps);
        if (past.Count > 3) { past.RemoveAt(0); }

        if (sr.sprite.name == "pushpush2")
            ps = PreviousSprite.Normal;
        else if (sr.sprite.name == "pushpush3")
            ps = PreviousSprite.Destroy;
    }

    public void TimeLeap()
    {
        if (past[past.Count - 1] == PreviousSprite.Normal)
        {
            past.RemoveAt(past.Count - 1);
            Ball_Out();
            ps = PreviousSprite.Normal;
        }
        else if (past[past.Count - 1] == PreviousSprite.Destroy)
        {
            past.RemoveAt(past.Count - 1);
            Ball_In();
            ps = PreviousSprite.Destroy;
        }
    }
}
