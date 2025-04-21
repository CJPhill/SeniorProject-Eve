using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public static Camera inventoryCamera;

    private void Start()
    {
        StartCoroutine(LoadAdditionalScenes());
    }

    IEnumerator LoadAdditionalScenes()
    {
        // Load Level Scene additively into CharacterScene
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        while (!loadLevel.isDone)
            yield return null;

        // Load Inventory Scene additively into CharacterScene
        AsyncOperation loadInventory = SceneManager.LoadSceneAsync("InventoryFinish", LoadSceneMode.Additive);
        while (!loadInventory.isDone)
            yield return null;

        SetupInventoryCamera();
    }

    void SetupInventoryCamera()
    {
        inventoryCamera = GameObject.FindGameObjectWithTag("InventoryCamera").GetComponent<Camera>();

        inventoryCamera.transform.SetParent(mainCamera.transform, worldPositionStays: false);
        inventoryCamera.transform.localPosition = Vector3.zero;
        inventoryCamera.transform.localRotation = Quaternion.identity;

        inventoryCamera.rect = new Rect(0.2f, 0.2f, 0.6f, 0.6f);
        inventoryCamera.enabled = false;
    }
}
