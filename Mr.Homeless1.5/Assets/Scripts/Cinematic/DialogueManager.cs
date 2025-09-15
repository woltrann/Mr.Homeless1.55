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
    public float typingSpeed = 0.03f; // Harflerin yaz�lma h�z� (saniye)

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
            // Resimler animasyonu bittikten sonra ilk sat�r� g�ster
            ShowNextLine();
        });
    }
    private void AnimateImages(System.Action onComplete)
    {
        // RectTransform referanslar�
        RectTransform botRect = botImage.GetComponent<RectTransform>();
        RectTransform topRect = topImage.GetComponent<RectTransform>();

        // Ba�lang�� pozisyonlar�n� ayarla (Inspector�da d��ar�da duracak �ekilde konumland�rabilirsin)
        botRect.anchoredPosition = new Vector2(botRect.anchoredPosition.x, -500f);
        topRect.anchoredPosition = new Vector2(topRect.anchoredPosition.x, 500f);

        // Animasyonlar
        Sequence seq = DOTween.Sequence();
        seq.Append(botRect.DOAnchorPosY(0f, 1f).SetEase(Ease.OutCubic)); // 1 saniyede yukar� ��k
        seq.Join(topRect.DOAnchorPosY(0f, 1f).SetEase(Ease.OutCubic));   // ayn� anda a�a�� insin
        seq.OnComplete(() =>
        {
            onComplete?.Invoke(); // animasyon bitince callback �al��s�n
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
        // RectTransform referanslar�n� al
        RectTransform botRect = botImage.GetComponent<RectTransform>();
        RectTransform topRect = topImage.GetComponent<RectTransform>();

        // Kapan�� animasyonu
        Sequence seq = DOTween.Sequence();
        seq.Append(botRect.DOAnchorPosY(-500f, 1f).SetEase(Ease.InCubic)); // A�a�� insin
        seq.Join(topRect.DOAnchorPosY(500f, 1f).SetEase(Ease.InCubic));   // Yukar� ��ks�n
        seq.OnComplete(() =>
        {
            dialoguePanel.SetActive(false); // animasyon bitti�inde paneli kapat
        });
    }


    public void NextLine()
    {
        if (typingCoroutine != null)
        {
            // Yazma i�lemi devam ediyorsa, metni tamamla
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
