using System.Collections.Generic;
using UnityEngine;

public enum Speaker
{
    Player,
    OldMan
}

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;
    [TextArea] public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/Dialogue")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}
