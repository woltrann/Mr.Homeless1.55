using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;

public class BuildingPanelManager : MonoBehaviour
{
    public static BuildingPanelManager Instance;
    [HideInInspector] public ConstructionSite currentConstructionSite; // O anki tıklanan alan
    [HideInInspector] public Constructions selectedConstruction; // geçici olarak seçilen bina

    [Header("İnşaat UI Referansları")]
    public GameObject buildingDetailPanel;
    public TextMeshProUGUI detailTitle;
    public TextMeshProUGUI detailDescription;
    public TextMeshProUGUI detailCost;
    public Image detailIcon;

    [Header("İnşaa Edilen UI Referansları")]    
    public GameObject constructionDetailPanel;
    public TextMeshProUGUI constructionDetailTitle;
    public TextMeshProUGUI constructionDetailDescription;
    public TextMeshProUGUI constructionDetailPrice;
    public Image constructionDetailIcon;

    [Header("Panel UI Referansları")]    
    public GameObject panelBuilding;
    public GameObject MarketPaneli;
    public GameObject ClosepanelButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image buildingImage;
    public Transform buttonParent;
    public GameObject buttonPrefab; // Tek, generic prefab
    private Vector2 touchStartPos;
    public float dragThreshold = 20f; // Ekranda 20 pikselden az hareket tıklama sayılır
    private List<GameObject> activeButtons = new List<GameObject>();

    public GameObject storepanel;
    public CameraDragMobile[] cameraDragScripts; // Hierarchy’deki 2 kamerayı buraya sürükle
    public bool isOpen = false;

    public GameObject buyPanel;
    public GameObject chooseBuildingPanel;
    public GameObject infoPanel;


    void Awake()
    {
        Instance = this;
 

        panelBuilding.SetActive(false);
        MarketPaneli.SetActive(false); 
        ClosepanelButton.SetActive(false);

    }
    void Update()
    {
        if (panelBuilding.activeSelf || DialogueManager.Instance.dialoguePanel.activeSelf || isOpen || buyPanel.activeSelf || chooseBuildingPanel.activeSelf || infoPanel.activeSelf || constructionDetailPanel.activeSelf || StoryManager.Instance.storyUI.activeSelf || MainMenu.Instance.mainMenu.activeSelf)
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
    
                // Eğer mesafe büyükse kaydırma yapılmıştır, panel açılmaz.
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
            // 1. Önce bina kontrolü
            var buildingInfo = hit.collider.GetComponent<BuildingInfo>();
            if (buildingInfo != null && buildingInfo.buildingData != null)
            {
                ShowBuildingInfo(buildingInfo.buildingData);
                return;
            }

            // 2. Eğer bina değilse ConstructionSite kontrolü
            var site = hit.collider.GetComponent<ConstructionSite>();
            if (site != null)
            {
                site.OnClick();
                return;
            }

            var construction = hit.collider.GetComponent<BuildingInfo>();
            if (construction != null && construction.constructions != null)
            {
                construction.OnTopConstruction();
                return;
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
            cooldown.SetNeedText(action.statChanges);

            cooldown.SetCallback(() => {
                ExecuteAction(action.actionType, action);
                CooldownManager.Instance.StartCooldown(id, action.cooldownDuration);
            });

            activeButtons.Add(btn);
        }

        ClosepanelButton.SetActive(true);

        if (MarketUIManager.Instance != null)
        {
            MarketUIManager.Instance.OpenMarket(data);
        }
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
        if (action.jobDeffance > StatManager.Instance.totalPower)
        {
            StatManager.Instance.ApplyStatChanges(new List<StatChange> {
                new StatChange { statType = StatType.Energy, amount = action.statChanges.Find(a => a.statType == StatType.Energy).amount },
                new StatChange { statType = StatType.Health, amount = action.statChanges.Find(c => c.statType == StatType.Health).amount }});
            Congrats.Instance.panelTitle.text = "<color=#A63A3A>BAŞARAMADIN</color>";
            Congrats.Instance.panelText.text = "İşlem başarısız. Gücünüzü artırıp tekrar denemelisiniz.";
            Congrats.Instance.OpenResultPanel();
        }
        else
        {
            StatManager.Instance.ApplyStatChanges(action.statChanges);
            Congrats.Instance.panelTitle.text = "<color=#5EA63A>TEBRİKLER</color>";
            Congrats.Instance.panelText.text = "İşlem Başarılı. Gücünüze güç katarak daha iyi işler çıkarabilirsiniz. ";


            switch (type)
            {
                case ButtonType.OpenMarket:
                    MarketPaneli.SetActive(true);
                    Debug.Log("Market açıldı.");
                    break;
                case ButtonType.StartJob:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.StartJob1:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.StartJob2:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.StartJob3:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.StartJob4:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.StartJob5:
                    Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.TalkToNPC:
                    //Congrats.Instance.OpenResultPanel();
                    break;
                case ButtonType.UpgradeBuilding:
                    //Congrats.Instance.OpenResultPanel();
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
    public void ToggleInventory()
    {
        isOpen = true;

        // Kamera kontrol scriptlerini aç/kapat
        foreach (var camDrag in cameraDragScripts)
        {
            if (camDrag != null)
                camDrag.enabled = !isOpen;
        }
    }

    public void CloseConstructionPanel()
    {
        buyPanel.SetActive(false);
        chooseBuildingPanel.SetActive(false);
        buildingDetailPanel.SetActive(false);
    }
    public void BuyConstructionArea()
    {
        if (currentConstructionSite != null)
            currentConstructionSite.BuyLand();
    }
    public void SelectBuilding(Constructions constructionData)
    {
        selectedConstruction = constructionData;

        // Detay panelini aç
        ShowConstructionDetails(constructionData);
    }

    public void ShowConstructionDetails(Constructions data)
    {
        buildingDetailPanel.SetActive(true);

        detailTitle.text = data.buildingName;
        detailDescription.text = data.buyDescription;
        detailCost.text = $"Maliyet: {data.cost}";
        detailIcon.sprite = data.buildingIcon;
    }
    public void ShowConstructionBuildingDetails(Constructions data)
    {
        constructionDetailPanel.SetActive(true);

        constructionDetailTitle.text = data.buildingName;
        constructionDetailDescription.text = data.infoDescription;
        constructionDetailPrice.text = $"Kazanç: {data.incomePerHour}";
        detailIcon.sprite = data.buildingIcon;
    }
    public void BuySelectedBuilding()
    {
        if (selectedConstruction != null && currentConstructionSite != null)
        {
            if (selectedConstruction.cost <= StatManager.Instance.money)
            {
                StatManager.Instance.ChangeMoney(-selectedConstruction.cost);
                // Parametre olarak prefab ver
                BuildSelectedBuilding(selectedConstruction.prefab);

                buildingDetailPanel.SetActive(false);
                selectedConstruction = null; // temizle
            }
            else
            {

                BuildingPanelManager.Instance.infoPanel.SetActive(true);
            }

        }
    }
    public void BuildSelectedBuilding(GameObject buildingPrefab)
    {
        if (currentConstructionSite != null)
            currentConstructionSite.Build(buildingPrefab);
    }
}
