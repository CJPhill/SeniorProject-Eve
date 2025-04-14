using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public GameObject computerTerminal;
    public Camera mainCamera; 
    public static Camera inventoryCamera;

    private void Start()
    {
        // computerTerminal.SetActive(false);
        StartCoroutine(LoadInventoryScene());
        Debug.Log("GameManager started");
    }

    IEnumerator LoadInventoryScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InventoryFinish", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        inventoryCamera = GameObject.FindGameObjectWithTag("InventoryCamera").GetComponent<Camera>();

        inventoryCamera.transform.SetParent(mainCamera.transform, worldPositionStays: false);

        inventoryCamera.transform.localPosition = Vector3.zero;
        inventoryCamera.transform.localRotation = Quaternion.identity;

        inventoryCamera.rect = new Rect(0.2f, 0.2f, 0.6f, 0.6f);
        inventoryCamera.enabled = false;
    }

}
