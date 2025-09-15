using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    [Header("UI Referansları")]
    public Image storyImage;          // Çizgi roman görselini gösterecek
    public Sprite[] storyPanels;      // Hikaye kareleri (Inspector’dan sürükle)
    public Image fadeImage;    // Ekranı karartıp açmak için
    public GameObject storyUI;        // Hikaye paneli (başta kapalı)
    public Button nextButton;         // Ekranı kaplayan buton

    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Awake()
    {
        Instance = this;
        storyUI.SetActive(false);
        nextButton.gameObject.SetActive(false);
        Color c = fadeImage.color;
        float t = 0;
        fadeImage.color = new Color(c.r, c.g, c.b, t);
    }

    public void StartStory()    // Başlangıçta çağrılır
    {
        storyUI.SetActive(true);
        nextButton.gameObject.SetActive(true);
        currentIndex = 0;

        storyImage.sprite = storyPanels[currentIndex];        // İlk kareyi yükle
        StartCoroutine(FadeIn());        // Fade-in ile aç
    }

    public void NextPanel()
    {
        if (isTransitioning) return;

        currentIndex++;
        if (currentIndex >= storyPanels.Length)
        {
            EndStory();            // Hikaye bitti → oyuna geç
            return;
        }

        StartCoroutine(TransitionToNext(storyPanels[currentIndex]));
    }

    IEnumerator TransitionToNext(Sprite nextSprite)
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());        // Fade Out
        storyImage.sprite = nextSprite;        // Yeni kareyi yükle
        yield return StartCoroutine(FadeIn());        // Fade In

        isTransitioning = false;
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f; // hız ayarı (0.5 = 2 saniye sürer)
            fadeImage.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float t = 1;
        Color c = fadeImage.color;
        while (t > 0f)
        {
            t -= Time.deltaTime * 0.5f;
            fadeImage.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }
    }


    void EndStory()
    {
        storyUI.SetActive(false);
        nextButton.gameObject.SetActive(false);
        MainMenu.Instance.mainMenu.SetActive(false);
        Debug.Log("Hikaye bitti, oyuna başla!");
        DialogueData dialogue = Resources.Load<DialogueData>("GameData/Dialogues/1Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
        // Burada sahne geçişi ekleyebilirsin
        // SceneManager.LoadScene("GameScene");
    }
}
