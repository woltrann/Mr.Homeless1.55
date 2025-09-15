using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Button))]
public class CooldownButton : MonoBehaviour
{
    public string cooldownID; // BuildingName + ActionType gibi benzersiz bir �ey atanacak

    public float cooldownDuration = 5f;
    public TextMeshProUGUI cooldownText; // Soldaki saya�
    public TextMeshProUGUI actionText;   // Butonun anlam� (�rne�in "Market A�")
    public TextMeshProUGUI jobDeffanceText; 

    private Button button;
    private float currentCooldown = 0f;
    private System.Action onClickCallback;

    void Awake()
    {
        button = GetComponent<Button>();

        if (cooldownText != null)
            cooldownText.gameObject.SetActive(false);

        button.onClick.AddListener(() =>
        {
            if (currentCooldown <= 0f)
            {
                onClickCallback?.Invoke(); // As�l i�levi yap
                StartCoroutine(CooldownRoutine());
            }
        });
    }
    void Start()
    {
        float remaining = CooldownManager.Instance.GetRemainingTime(cooldownID);
        if (remaining > 0f)
        {
            StartCoroutine(RunCooldownDisplay());
        }
    }

    public void SetCallback(System.Action callback)
    {
        onClickCallback = callback;
    }

    public void SetActionText(string text, string deffencetext)
    {
        if (actionText != null)
            actionText.text = text;
        if (deffencetext == "0")
        {
            jobDeffanceText.text = "-";  
        }
        else
        {
            jobDeffanceText.text = deffencetext;
        }
        
    }

    IEnumerator CooldownRoutine()
    {
        CooldownManager.Instance.StartCooldown(cooldownID, cooldownDuration);
        yield return RunCooldownDisplay();
    }

    IEnumerator RunCooldownDisplay()
    {
        button.interactable = false;
        cooldownText?.gameObject.SetActive(true);

        float remaining;
        while ((remaining = CooldownManager.Instance.GetRemainingTime(cooldownID)) > 0f)
        {
            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            cooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }

        button.interactable = true;
        cooldownText?.gameObject.SetActive(false);
    }
}
