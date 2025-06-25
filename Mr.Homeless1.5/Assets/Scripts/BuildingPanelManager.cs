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
            return; // Panel açýkken sahne týklamalarýný engelle

        // Mobil dokunma
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleTap(Input.GetTouch(0).position);
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleTap(Input.mousePosition);
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
                ExecuteAction(action.actionType);
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





    public void ExecuteAction(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.OpenMarket:
                MarketPaneli.SetActive(true);
                Debug.Log("Market açýldý.");
                break;
            case ButtonType.StartJob:
                Debug.Log("Ýþe baþlandý.");
                break;
            case ButtonType.TalkToNPC:
                Debug.Log("NPC ile konuþuldu.");
                break;
            case ButtonType.UpgradeBuilding:
                Debug.Log("Bina geliþtirildi.");
                break;
        }
    }
    public void CloseMarketPanel()
    {
        MarketPaneli.SetActive(false);

    }
}
