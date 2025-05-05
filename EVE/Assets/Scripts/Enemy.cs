using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    public GameManager Manager;

    private void Start()
    {
        Manager = FindAnyObjectByType<GameManager>();
    }

    public enum EnemyType
    {
        Normal,
        Elite,
        Boss
    }

    public void receiveInteract()
    {
        Manager.sceneCall("CombatTest");
    }

}
