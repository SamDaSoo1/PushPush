using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    List<Sprite> backgrounds;
    [SerializeField] Image img;

    private void Awake()
    {
        backgrounds = new List<Sprite>()
        {
            Resources.Load<Sprite>("background"),
            Resources.Load<Sprite>("pushend")
        };

        img = GetComponent<Image>();
    }

    public void BackgroundChange()
    {
        img.sprite = backgrounds[1];
    }
}
