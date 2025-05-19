using UnityEngine;

public class CameraDragMobile : MonoBehaviour
{
    public float dragSpeed = 0.1f;

    [Header("Sýnýrlar")]
    public Vector2 minPosition; // Örn: (-5, -5)
    public Vector2 maxPosition; // Örn: (5, 5)

    public Camera cam;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    void Start()
    {
        //cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                Vector3 move = new Vector3(-delta.x * dragSpeed * Time.deltaTime, -delta.y * dragSpeed * Time.deltaTime, 0);

                cam.transform.position += move;
                cam.transform.position = ClampPosition(cam.transform.position);

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(position.y, minPosition.y, maxPosition.y),
            position.z
        );
    }
}
