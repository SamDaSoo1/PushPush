using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] Image[] heart;
    [SerializeField] int count;

    private void Awake()
    {
        sprites = new List<Sprite>()
        {
            Resources.Load<Sprite>("Empty_Heart"),
            Resources.Load<Sprite>("Full_Heart"),
        };
    }

    void Start()
    {
        heart = transform.GetChild(0).GetComponentsInChildren<Image>();
        Init();
    }

    public void Init()
    {
        count = 0;
        foreach (Image img in heart)
        {
            img.sprite = sprites[0];
        }
    }

    public void Fill_Heart()
    {
        if(0 <= count && count < 3)
            heart[count].sprite = sprites[1];

        count++;
        if (count > 3)
            count = 3;
    }

    public void Empty_Heart()
    {
        if(0 < count && count <= 3)
            heart[count - 1].sprite = sprites[0];

        count--;
        if (count < 0)
            count = 0;
    }
}
