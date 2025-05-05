using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFirstLevel : MonoBehaviour
{
    public string sceneName;

    public void LoadFirstScene()
    {
        GameManager.Instance.sceneCall(sceneName);
    } 
}
