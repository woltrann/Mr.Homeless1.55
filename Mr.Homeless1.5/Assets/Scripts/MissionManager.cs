using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;
    public GameObject taskStartPanel;
    public List<Tasks> missions = new List<Tasks>();
    private int currentMissionIndex = 0;

    public Text TaskNo;
    public Text TaskName;
    public Text TaskDescription;
    public Text TaskReward;

    public Transform completedTaskContent; // Content objesi
    public GameObject completedTaskPanelPrefab; // Panel prefab�

    private float checkInterval = 1f; // her saniyede bir kontrol
    private float checkTimer = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckMissionStatus();
        }
    }

    void CheckMissionStatus()
    {
        Tasks current = GetCurrentMission();
        if (current != null && !current.isCompleted && current.taskType != TaskType.Custom)
        {
            if (current.CheckIfCompleted())
            {
                CompleteCurrentMission();
            }
        }
    }

    public void StartCurrentMission()
    {
        taskStartPanel.SetActive(true);
        if (currentMissionIndex < missions.Count)
        {
            var mission = missions[currentMissionIndex];

            Debug.Log("G�rev Ba�lad�: " + missions[currentMissionIndex].taskName);
            TaskNo.text= "G�REV " + (currentMissionIndex + 1);
            TaskName.text = missions[currentMissionIndex].taskName;
            TaskDescription.text = missions[currentMissionIndex].description;
            string rewardText = "";
            foreach (var reward in mission.rewardGain)
            {
                string sign = reward.amount >= 0 ? "+" : "-";
                switch (reward.rewardType)
                {
                    case RewardType.Money:
                        rewardText += $"{sign}{Mathf.Abs(reward.amount)} Para\n";
                        break;
                    case RewardType.Fame:
                        rewardText += $"{sign}{Mathf.Abs(reward.amount)} ��hret\n";
                        break;
                    default:
                        rewardText += $"{sign}{Mathf.Abs(reward.amount)} {reward.rewardType}\n";
                        break;
                }
            }

            TaskReward.text = rewardText;
        }
        else
        {
            Debug.Log("T�m g�revler tamamland�.");
        }
    }
    public void CompleteCurrentMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            var mission = missions[currentMissionIndex];
            mission.isCompleted = true;

            GameObject newPanel = Instantiate(completedTaskPanelPrefab, completedTaskContent);  //  Paneli olu�tur ve yaz�lar�n� ayarla  
            Text[] texts = newPanel.GetComponentsInChildren<Text>();    // Panelin i�indeki Text objelerini bul (Text yerine TMP_Text kullan�yorsan onu yaz)

            if (texts.Length >= 2)
            {
                texts[0].text = "G�REV " + (currentMissionIndex + 1);
                texts[1].text = mission.taskName;
            }

            TaskNo.text = "";
            TaskName.text = "";
            TaskDescription.text = "";
            TaskReward.text = "";
            currentMissionIndex++;
        }
    }
    public Tasks GetCurrentMission()
    {
        return currentMissionIndex < missions.Count ? missions[currentMissionIndex] : null;
    }
    public void taskStartPanelCloese() => taskStartPanel.SetActive(false);

}
