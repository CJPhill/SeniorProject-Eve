using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _interactSprite;

    private const float INTERACT_RADIUS = 5f;

    private Transform _playerTransform;

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

        if(_interactSprite.gameObject.activeSelf && !isPlayerInRange()){
            _interactSprite.gameObject.SetActive(false);
        }
        else if(!_interactSprite.gameObject.activeSelf && isPlayerInRange()){
            _interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void receiveInteract();

    private bool isPlayerInRange()
    {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}
