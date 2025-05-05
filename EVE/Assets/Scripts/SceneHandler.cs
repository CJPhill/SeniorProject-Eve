using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour, IInteractable
{
    [SerializeField] RectTransform fader;
    [SerializeField] string sceneToLoad;

    private void Start(){
        fader.gameObject.SetActive(true);

        LeanTween.scale(fader, new Vector3(1,1,1), 0);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void receiveInteract()
    {
        Debug.Log("SceneHandler: receiveInteract");
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0f);
        LeanTween.scale(fader, new Vector3(1,1,1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            Invoke("LoadScene", 0.5f);
        });
    }
    private void LoadScene(){
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
}
