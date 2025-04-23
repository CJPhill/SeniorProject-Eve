using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class StoryCutscene : MonoBehaviour
{
    [System.Serializable]
    public class StoryBeat
    {
        public Sprite image;
        [TextArea(3, 10)]
        public string text;
    }

    public Image storyImage;
    public TMP_Text storyText;
    public Button nextButton;

    public StoryBeat[] storyBeats;

    private int currentBeat = 0;

    void Start()
    {
        nextButton.onClick.AddListener(NextBeat);
        ShowBeat();
    }

    void ShowBeat()
    {
        if (currentBeat < storyBeats.Length)
        {
            storyImage.sprite = storyBeats[currentBeat].image;
            storyText.text = storyBeats[currentBeat].text;
        }
        else
        {
            EndCutscene();
        }
    }

    public void NextBeat()
    {
        currentBeat++;
        ShowBeat();
    }

    void EndCutscene()
    {
  
        gameObject.SetActive(false);
     
    }
}
