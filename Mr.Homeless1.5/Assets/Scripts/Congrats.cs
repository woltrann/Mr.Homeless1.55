using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class Congrats : MonoBehaviour
{
    public static Congrats Instance;
    //public GameObject ResultPanel;
    private int a = 0;
    public Text text;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void OpenResultPanel()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        StartCoroutine(ResultPanelClose());
    }
    public void CloseResultPanel()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);

    }
    private IEnumerator ResultPanelClose()
    {
        yield return new WaitForSeconds(2f);
        transform.localScale = new Vector3(0f, 0f, 0f);

    }
}
