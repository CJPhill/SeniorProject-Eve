using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public static Camera inventoryCamera;
    [SerializeField] RectTransform fader;
    [SerializeField] string sceneToLoad;
    public string startScene;

    public static GameManager Instance;

    // Static flag to track if the video has already been played
    private static bool hasVideoPlayed = false;

    // Reference to RawImage and VideoPlayer
    public RawImage videoRawImage;
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fader.gameObject.SetActive(true);
        fader.localScale = new Vector3(1, 1, 1);

        PlayFadeIn(() =>
        {
            StartCoroutine(LoadAdditionalScenes());
        });
    }

    public void sceneCall(string scene)
    {
        sceneToLoad = scene;
        PlayFadeOut(() =>
        {
            Invoke(nameof(LoadScene), 0.1f); // small delay for visual polish
        });
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            string previousScene = SceneManager.GetActiveScene().name;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneToLoad);

            if (sceneToLoad == "VanScene")
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.SetActive(false);
                }
            }
            else if (previousScene == "VanScene")
            {
                GameObject scenePlayer = GameObject.FindWithTag("Player");

                // Only destroy scenePlayer if it’s not the persistent one
                if (scenePlayer != null)
                {
                    Destroy(scenePlayer);
                }

                GameObject persistentPlayer = GameObject.FindWithTag("Player");
                if (persistentPlayer != null)
                {
                    persistentPlayer.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("GameManager: sceneToLoad is not set!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(LoadAdditionalScenes());
        PlayFadeIn();
    }

    IEnumerator LoadAdditionalScenes()
    {
        Debug.Log("GameManager: Loading InventoryFinish");
        AsyncOperation loadInventory = SceneManager.LoadSceneAsync("InventoryFinish", LoadSceneMode.Additive);
        while (!loadInventory.isDone)
            yield return null;

        // Ensure EventSystem is active
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem != null)
        {
            eventSystem.gameObject.SetActive(true);
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        SetupInventoryCamera();

        // Play video if it hasn't been played already
        if (!hasVideoPlayed)
        {
            PlayIntroVideo();
        }
    }

    void SetupInventoryCamera()
    {
        inventoryCamera = GameObject.FindGameObjectWithTag("InventoryCamera")?.GetComponent<Camera>();

        if (inventoryCamera != null && mainCamera != null)
        {
            inventoryCamera.transform.SetParent(mainCamera.transform, worldPositionStays: false);
            inventoryCamera.transform.localPosition = Vector3.zero;
            inventoryCamera.transform.localRotation = Quaternion.identity;
            inventoryCamera.rect = new Rect(0.2f, 0.2f, 0.6f, 0.6f);
            inventoryCamera.enabled = false; // Initially disable it
        }
    }

    public void PlayIntroVideo()
    {
        // Ensure the video only plays once
        if (videoPlayer != null && videoRawImage != null)
        {
            videoRawImage.gameObject.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    // Called when the video finishes
    private void OnVideoEnd(VideoPlayer vp)
    {
        videoRawImage.gameObject.SetActive(false);  // Hide the video after it finishes
        hasVideoPlayed = true;  // Mark the video as played
        EnableInventoryUI();  // Enable UI interaction after video ends
    }

    // Enable the UI elements in the inventory scene
    void EnableInventoryUI()
    {
        var uiElements = GameObject.FindGameObjectsWithTag("InventoryUI"); // Assuming your UI has a common tag
        foreach (var element in uiElements)
        {
            element.SetActive(true);
        }

        // Enable the inventory camera for interaction
        if (inventoryCamera != null)
        {
            inventoryCamera.enabled = true;
        }
    }

    public void PlayFadeIn(System.Action onComplete = null)
    {
        fader.gameObject.SetActive(true);
        fader.localScale = new Vector3(1, 1, 1);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void PlayFadeOut(System.Action onComplete = null)
    {
        fader.gameObject.SetActive(true);
        fader.localScale = Vector3.zero;
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
