using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SquareCamera : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        float targetAspect = 1.0f; // Square aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = _camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            _camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = _camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            _camera.rect = rect;
        }
    }
}

