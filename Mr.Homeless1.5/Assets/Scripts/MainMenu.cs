using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public Button startButton;
    public Button newGameButton;
    public Button continueButton;
    public Button settignsButton;
    public Button exitButton;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    void Awake()=>Instance = this;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void PressStartGame()
    {
        newGameButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }
    public void StartNewGame()
    {
        StoryManager.Instance.StartStory();
    }
    public void ContinueGame()
    {
        MainMenu.Instance.mainMenu.SetActive(false);

    }
    public void SettingsPanelToggle() => settingsMenu.SetActive(!settingsMenu.activeSelf);
    public void ExitGame() => Application.Quit();

}
