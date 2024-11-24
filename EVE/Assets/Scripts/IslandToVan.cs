using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandToVan : MonoBehaviour, IInteractable
{
    public void receiveInteract()
    {
        SceneManager.LoadScene("VanScene");
    }
}
