using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public int width, height;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
    }
}