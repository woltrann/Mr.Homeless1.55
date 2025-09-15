using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIPanelSlider : MonoBehaviour
{
    public RectTransform panel;
    public GameObject backgroundClickArea;
    public float duration = 0.3f;
    public Ease ease = Ease.OutBack;

    public bool isOpen = false;

    private static List<UIPanelSlider> allPanels = new List<UIPanelSlider>();

    private void Awake()
    {
        if (!allPanels.Contains(this))
            allPanels.Add(this);

        // Pivot sol üst → aşağı doğru büyümesi için
        if (panel != null)
            panel.pivot = new Vector2(0, 1);
    }

    private void OnDestroy()
    {
        allPanels.Remove(this);
    }

    private void Start()
    {
        // Başlangıçta kapalı: genişlik sabit (X=1), yükseklik 0
        if (panel != null)
            panel.localScale = new Vector3(1, 0, 1);

        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else OpenAndCloseOthers();


        //BuildingPanelManager.Instance.ToggleInventory();
    }

    public void OpenAndCloseOthers()
    {
        isOpen = true;
        BuildingPanelManager.Instance.isOpen = true;
        foreach (var p in allPanels)
        {
            if (p != this && p.isOpen)
                p.Close();
        }

        // Açılırken sadece Y ekseni büyür
        panel.DOScaleY(1, duration).SetEase(ease);
        

        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(true);
    }

    public void Close()
    {
        // Kapanırken sadece Y ekseni küçülür
        panel.DOScaleY(0, duration).SetEase(Ease.InBack);
        isOpen = false;
        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(false);

        InventoryUIManager.Instance.ClearSelection();
    }
    public void CloseBool()
    {
        BuildingPanelManager.Instance.isOpen = false;
        isOpen = false;

    }
}
