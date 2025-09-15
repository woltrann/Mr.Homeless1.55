using UnityEngine;
using DG.Tweening;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Referans Tasarým")]
    public float defaultAspect = 9f / 16f;            // 16:9 dikey ekran oraný
    public float defaultOrthographicSize = 5f;        // Senin referans orto size'ýn
    public float defaultFieldOfView = 50f;

    public Camera camera1;
    public Camera camera2;

    private Texture2D blackTexture;
    private float fadeAlpha = 0f;
    private bool isFading = false;

    public float fadeDuration = 0.5f;

    void Start()
    {
        ApplyCameraScaling(camera1);
        ApplyCameraScaling(camera2);
        ActivateCamera1();

        // Siyah doku oluþtur
        blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();
    }
    private void Update()
    {
        ApplyCameraScaling(camera1);
        ApplyCameraScaling(camera2);
    }
    private void ApplyCameraScaling(Camera cam)
    {
        if (cam != null && cam.orthographic)
        {
            float currentAspect = (float)Screen.width / (float)Screen.height;
            float sizeAdjustment = defaultAspect / currentAspect;
            cam.orthographicSize = defaultOrthographicSize * sizeAdjustment;
        }

    }


    //Kameralar arasý geçiþ
    void OnGUI()
    {
        if (fadeAlpha > 0f)
        {
            GUI.color = new Color(0, 0, 0, fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
        }
    }
    public void SwitchCamera(int x)
    {
        if (isFading) return;
        isFading = true;
        
        DOTween.To(() => fadeAlpha, x => fadeAlpha = x, 1f, fadeDuration).OnComplete(() =>      // Fade In
        {
            if(x==1) ActivateCamera1(); else ActivateCamera2();


            DOTween.To(() => fadeAlpha, x => fadeAlpha = x, 0f, fadeDuration).OnComplete(() =>      // Fade Out
            {
                isFading = false;
            });
        });
    }
    public void ActivateCamera1()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false); 
    }
    public void ActivateCamera2()
    {
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
    }
}
