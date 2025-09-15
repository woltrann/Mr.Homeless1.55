using UnityEngine;
using UnityEngine.UI;

public class MainMenuScroller : MonoBehaviour
{
    [SerializeField] private RawImage img1 ;
    [SerializeField] private float x, y;

    // Update is called once per frame
    void Update()
    {
        img1.uvRect = new Rect(img1.uvRect.position + new Vector2(x, y) * Time.deltaTime, img1.uvRect.size);
    }
}
