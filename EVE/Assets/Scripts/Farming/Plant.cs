using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Plant : MonoBehaviour, IInteractable
{
    private const float INTERACT_RADIUS = 5f;

    private Transform _playerTransform;
    public bool interacting = false;
    

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if(UserInput.instance.Interact && isPlayerInRange())
        {
            receiveInteract();
        }
    }

    public abstract void receiveInteract();

    private bool isPlayerInRange()
    {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}
