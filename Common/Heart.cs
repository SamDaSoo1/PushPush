using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<Image> heart_Imgs;
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
        heart_Imgs = new List<Image>()
        {
            GameObject.Find("Heart 1").GetComponent<Image>(),
            GameObject.Find("Heart 2").GetComponent<Image>(),
            GameObject.Find("Heart 3").GetComponent<Image>()
        };
        Init();
    }

    public void Init()
    {
        count = 0;
        foreach (Image img in heart_Imgs)
        {
            img.sprite = sprites[0];
        }
    }

    public void Fill_Heart()
    {
        if(0 <= count && count < 3)
            heart_Imgs[count].sprite = sprites[1];

        count++;
        if (count > 3)
            count = 3;
    }

    public void Empty_Heart()
    {
        if(0 < count && count <= 3)
            heart_Imgs[count - 1].sprite = sprites[0];

        count--;
        if (count < 0)
            count = 0;
    }

    public void Fill_All_Heart()
    {
        for(int i = 0; i < heart_Imgs.Count; i++)
        {
            heart_Imgs[i].sprite = sprites[1];
        }
    }
}
