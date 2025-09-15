using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject botImage;
    public GameObject topImage;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    public DialogueData[] dialogues;

    private Coroutine typingCoroutine;  // Yazma Coroutine'i tutacak
    public float typingSpeed = 0.03f; // Harflerin yazýlma hýzý (saniye)

    private void Awake()
    {
        Instance = this;
    }
    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        AnimateImages(() =>
        {
            // Resimler animasyonu bittikten sonra ilk satýrý göster
            ShowNextLine();
        });
    }
    private void AnimateImages(System.Action onComplete)
    {
        // RectTransform referanslarý
        RectTransform botRect = botImage.GetComponent<RectTransform>();
        RectTransform topRect = topImage.GetComponent<RectTransform>();

        // Baþlangýç pozisyonlarýný ayarla (Inspector’da dýþarýda duracak þekilde konumlandýrabilirsin)
        botRect.anchoredPosition = new Vector2(botRect.anchoredPosition.x, -500f);
        topRect.anchoredPosition = new Vector2(topRect.anchoredPosition.x, 500f);

        // Animasyonlar
        Sequence seq = DOTween.Sequence();
        seq.Append(botRect.DOAnchorPosY(0f, 1f).SetEase(Ease.OutCubic)); // 1 saniyede yukarý çýk
        seq.Join(topRect.DOAnchorPosY(0f, 1f).SetEase(Ease.OutCubic));   // ayný anda aþaðý insin
        seq.OnComplete(() =>
        {
            onComplete?.Invoke(); // animasyon bitince callback çalýþsýn
        });
    }
    public void ShowNextLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (currentLineIndex < currentDialogue.lines.Count)
        {
            DialogueLine line = currentDialogue.lines[currentLineIndex];
            speakerText.text = line.speaker.ToString();
            typingCoroutine = StartCoroutine(TypeSentence(line.text));
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        // RectTransform referanslarýný al
        RectTransform botRect = botImage.GetComponent<RectTransform>();
        RectTransform topRect = topImage.GetComponent<RectTransform>();

        // Kapanýþ animasyonu
        Sequence seq = DOTween.Sequence();
        seq.Append(botRect.DOAnchorPosY(-500f, 1f).SetEase(Ease.InCubic)); // Aþaðý insin
        seq.Join(topRect.DOAnchorPosY(500f, 1f).SetEase(Ease.InCubic));   // Yukarý çýksýn
        seq.OnComplete(() =>
        {
            dialoguePanel.SetActive(false); // animasyon bittiðinde paneli kapat
        });
    }


    public void NextLine()
    {
        if (typingCoroutine != null)
        {
            // Yazma iþlemi devam ediyorsa, metni tamamla
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.lines[currentLineIndex - 1].text;
            typingCoroutine = null;
        }
        else
        {
            ShowNextLine();
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

}
