using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Elite,
        Boss
    }

    //Trigger Battle
    private void OnTriggerEnter(Collider other) {
        SceneHandler sceneHandler = GetComponent<SceneHandler>();
        if (sceneHandler != null)
        {
            Debug.Log("OnTriggerEnter");
            sceneHandler.receiveInteract();
        }
    }
}
