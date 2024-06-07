using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] GameObject bgd;
    [SerializeField] GameObject canvas;

    void Start()
    {
        transform.position = new Vector3(bgd.GetComponent<RectTransform>().position.x, bgd.GetComponent<RectTransform>().position.y, 0);
        SetScale();
    }

    public void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    public void SetScale()
    {
        transform.localScale = new Vector3(canvas.transform.localScale.x / 0.0092592f, canvas.transform.localScale.y / 0.0092592f, canvas.transform.localScale.z / 0.0092592f);
    }
}
