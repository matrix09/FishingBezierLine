using UnityEngine;
using System.Collections;

public class AutoScale : MonoBehaviour {
    public int height = 640;
    public int width = 1136;

    void Start()
    {
        float baseF = (float)height / width;
        float currentF = (float)Screen.height / Screen.width;
        if (currentF > baseF)
        {
            transform.localScale *= baseF / currentF;
        }
    }
}
