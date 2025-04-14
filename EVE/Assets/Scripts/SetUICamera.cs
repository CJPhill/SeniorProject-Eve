using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUICamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera uiCamera = GameObject.Find("MenuCamera").GetComponent<Camera>();

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && uiCamera != null)
        {
            canvas.worldCamera = uiCamera;
        }       
    }
}
