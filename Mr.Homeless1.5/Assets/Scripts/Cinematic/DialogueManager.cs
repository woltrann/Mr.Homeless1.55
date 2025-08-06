using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Text speakerText;
    public Text dialogueText;
    public GameObject dialoguePanel;

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
        ShowNextLine();
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
            speakerText.text = line.speakerName;
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
        dialoguePanel.SetActive(false);

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
