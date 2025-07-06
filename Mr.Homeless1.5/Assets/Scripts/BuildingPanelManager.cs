using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuildingPanelManager : MonoBehaviour
{
    public static BuildingPanelManager Instance;

    [Header("Panel UI Referanslarý")]
    public GameObject panel;
    public GameObject MarketPaneli;
    public GameObject ClosepanelButton;
    public Text titleText;
    public Text descriptionText;
    public Image buildingImage;
    public Transform buttonParent;
    public GameObject defaultButtonPrefab;

    private List<GameObject> activeButtons = new List<GameObject>();
    public GameObject buttonPrefab; // Tek, generic prefab
    private Vector2 touchStartPos;
    public float dragThreshold = 20f; // Ekranda 20 pikselden az hareket týklama sayýlýr


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
        MarketPaneli.SetActive(false); 
        ClosepanelButton.SetActive(false);

    }

    void Update()
    {
        if (panel.activeSelf)
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
                // Eðer mesafe büyükse kaydýrma yapýlmýþtýr, panel açýlmaz.
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
        panel.SetActive(true);

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
            cooldown.SetActionText(action.buttonText);
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
        panel.SetActive(false);
        MarketPaneli.SetActive(false);
        ClosepanelButton.SetActive(false);

    }





    public void ExecuteAction(ButtonType type, BuildingAction action)
    {
        StatManager.Instance.ApplyStatChanges(action.statChanges);

        switch (type)
        {
            case ButtonType.OpenMarket:
                MarketPaneli.SetActive(true);
                Debug.Log("Market açýldý.");
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
    public void CloseMarketPanel()
    {
        MarketPaneli.SetActive(false);

    }
}
