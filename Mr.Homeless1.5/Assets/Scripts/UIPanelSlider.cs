using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIPanelSlider : MonoBehaviour
{
    public RectTransform panel;
    public GameObject backgroundClickArea;
    public Vector2 onScreenPos;
    public Vector2 offScreenPos;
    public float duration = 0.5f;
    public Ease ease = Ease.OutCubic;

    private bool isOpen = false;

    //  Statik olarak tüm panel slider'larý tut
    private static List<UIPanelSlider> allPanels = new List<UIPanelSlider>();

    private void Awake()
    {
        if (!allPanels.Contains(this))
            allPanels.Add(this);
    }

    private void OnDestroy()
    {
        allPanels.Remove(this);
    }

    private void Start()
    {
        panel.anchoredPosition = offScreenPos;
        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else OpenAndCloseOthers();
    }

    public void OpenAndCloseOthers()
    {
        //  Diðer açýk panelleri kapat
        foreach (var p in allPanels)
        {
            if (p != this && p.isOpen)
                p.Close();
        }

        // Kendi panelini aç
        panel.DOAnchorPos(onScreenPos, duration).SetEase(ease);
        isOpen = true;
        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(true);
    }

    public void Close()
    {
        panel.DOAnchorPos(offScreenPos, duration).SetEase(ease);
        isOpen = false;
        if (backgroundClickArea != null)
            backgroundClickArea.SetActive(false);
    }
}
