using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Rewards
{
    public RewardType rewardType;
    public int amount; // + art��, - azal��
}

public enum RewardType
{
    Money,
    Fame
}
public enum TaskType
{
    CollectItem,
    ReachLocation,
    TalkToNPC,
    KillEnemy,
    PressButton,
    Custom // �zel fonksiyonla kontrol edilir
}

[CreateAssetMenu(fileName = "Tasks", menuName = "Scriptable Objects/Tasks")]
public class Tasks : ScriptableObject
{
    public bool isCompleted;
    public string taskName;
    [TextArea] public string description;
    public List<Rewards> rewardGain;

    public TaskType taskType;

    public string requiredID; // �rne�in: itemID, NPC ad�, d��manID

    public bool CheckIfCompleted()
    {
        switch (taskType)
        {
            //case TaskType.CollectItem:
            //    return InventoryManager.Instance.HasItem(requiredID);
            //case TaskType.ReachLocation:
            //    return LocationTracker.Instance.HasReached(requiredID);
            //case TaskType.TalkToNPC:
            //    return DialogueManager.Instance.HasTalkedTo(requiredID);
            //case TaskType.KillEnemy:
            //    return EnemyTracker.Instance.IsDefeated(requiredID);
            //case TaskType.PressButton:
            //    return ButtonTracker.Instance.IsPressed(requiredID);
            case TaskType.Custom:
                return false; // �zel g�revlerde elle tamamlan�r
        }
        return false;
    }
}