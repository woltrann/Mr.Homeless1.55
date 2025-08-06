using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Rewards
{
    public RewardType rewardType;
    public int amount; // + artýþ, - azalýþ
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
    Custom // Özel fonksiyonla kontrol edilir
}

[CreateAssetMenu(fileName = "Tasks", menuName = "Scriptable Objects/Tasks")]
public class Tasks : ScriptableObject
{
    public bool isCompleted;
    public string taskName;
    [TextArea] public string description;
    public List<Rewards> rewardGain;

    public TaskType taskType;

    public string requiredID; // Örneðin: itemID, NPC adý, düþmanID

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
                return false; // Özel görevlerde elle tamamlanýr
        }
        return false;
    }
}