using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuildingPanelManager : MonoBehaviour
{
    public static BuildingPanelManager Instance;

    [Header("Panel UI Referanslar�")]    
    private List<GameObject> activeButtons = new List<GameObject>();
    public GameObject panelBuilding;
    public GameObject MarketPaneli;
    public GameObject ClosepanelButton;
    public Text titleText;
    public Text descriptionText;
    public Image buildingImage;
    public Transform buttonParent;
    public GameObject buttonPrefab; // Tek, generic prefab
    private Vector2 touchStartPos;
    public float dragThreshold = 20f; // Ekranda 20 pikselden az hareket t�klama say�l�r

    public GameObject storepanel;


    void Awake()
    {
        Instance = this;
 

        panelBuilding.SetActive(false);
        MarketPaneli.SetActive(false); 
        ClosepanelButton.SetActive(false);

    }

    void Update()
    {
        if (panelBuilding.activeSelf || DialogueManager.Instance.dialoguePanel.activeSelf)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float distance = Vector2.Distance(touch.position, touchStartPos);

                if (distance < dragThreshold)
                {
                    HandleTap(touch.position);
                }
                // E�er mesafe b�y�kse kayd�rma yap�lm��t�r, panel a��lmaz.
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float distance = Vector2.Distance((Vector2)Input.mousePosition, touchStartPos);

            if (distance < dragThreshold)
            {
                HandleTap(Input.mousePosition);
            }
        }
#endif
    }

    void HandleTap(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var buildingInfo = hit.collider.GetComponent<BuildingInfo>();
            if (buildingInfo != null && buildingInfo.buildingData != null)
            {
                ShowBuildingInfo(buildingInfo.buildingData);
            }
        }
    }

    public void ShowBuildingInfo(Buildings data)
    {
        panelBuilding.SetActive(true);

        titleText.text = data.buildingName;
        descriptionText.text = data.description;
        buildingImage.sprite = data.buildingImage;

        ClearOldButtons();

        foreach (var action in data.actions)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);
            CooldownButton cooldown = btn.GetComponent<CooldownButton>();

            string id = data.buildingName + "_" + action.actionType.ToString();
            cooldown.cooldownID = id;
            cooldown.cooldownDuration = action.cooldownDuration;
            cooldown.SetActionText(action.buttonText, action.jobDeffance.ToString());
            cooldown.SetCallback(() => {
                ExecuteAction(action.actionType, action);
                CooldownManager.Instance.StartCooldown(id, action.cooldownDuration);
            });

            activeButtons.Add(btn);
        }

        ClosepanelButton.SetActive(true);
    }
    public void ClearOldButtons()
    {
        foreach (var b in activeButtons) Destroy(b);
        activeButtons.Clear();
    }

    public void ClosePanel()
    {
        ClearOldButtons();
        panelBuilding.SetActive(false);
        MarketPaneli.SetActive(false);
        ClosepanelButton.SetActive(false);

    }





    public void ExecuteAction(ButtonType type, BuildingAction action)
    {
        if (action.jobDeffance > StatManager.Instance.power)
        {
            StatManager.Instance.ApplyStatChanges(new List<StatChange> {
                new StatChange { statType = StatType.Energy, amount = action.statChanges.Find(a => a.statType == StatType.Energy).amount },
                new StatChange { statType = StatType.Health, amount = action.statChanges.Find(c => c.statType == StatType.Health).amount }});
            Congrats.Instance.panelTitle.text = "<color=#A63A3A>BA�ARAMADIN</color>";
            Congrats.Instance.panelText.text = "��lem ba�ar�s�z. G�c�n�z� art�r�p tekrar denemelisiniz.";
            Congrats.Instance.OpenResultPanel();
        }
        else
        {
            StatManager.Instance.ApplyStatChanges(action.statChanges);
            Congrats.Instance.panelTitle.text = "<color=#5EA63A>TEBR�KLER</color>";
            Congrats.Instance.panelText.text = "��lem Ba�ar�l�. G�c�n�ze g�� katarak daha iyi i�ler ��karabilirsiniz. ";


            switch (type)
            {
                case ButtonType.OpenMarket:
                    MarketPaneli.SetActive(true);
                    Debug.Log("Market a��ld�.");
                    break;
                case ButtonType.StartJob:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.TalkToNPC:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.UpgradeBuilding:
                    Congrats.Instance.OpenResultPanel();
                    break;
            }
        }
    }

        
    public void CloseMarketPanel()
    {
        MarketPaneli.SetActive(false);

    }

    public void StorePanelOpen()
    {
        storepanel.SetActive(true);
    }
    public void StorePanelClose()
    {
        storepanel.SetActive(false);
    }

}
