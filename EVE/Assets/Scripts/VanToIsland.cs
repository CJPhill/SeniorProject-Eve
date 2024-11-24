using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VanToIsland : MonoBehaviour, IInteractable
{
    public void receiveInteract()
    {
        //Should be changed to be "current island" later
        SceneManager.LoadScene("GrayBoxingLevel1");
    }
}
