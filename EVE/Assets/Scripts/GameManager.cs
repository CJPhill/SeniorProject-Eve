using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public static Camera inventoryCamera;
    [SerializeField] RectTransform fader;
    [SerializeField] string sceneToLoad;


    private void Awake()
    {
        // Check if an instance already exists
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(LoadAdditionalScenes());
        fader.gameObject.SetActive(true);

        LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void sceneCall(string scene)
    {
        Debug.Log("SceneHandler: receiveInteract");
        sceneToLoad = scene;
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0f);
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            Invoke("LoadScene", 0.5f);
        });
    }
    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("SceneHandler: Loading scene " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneHandler: targetSceneName is not set!");
        }
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
