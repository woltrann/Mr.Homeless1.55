using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class CooldownData
{
    public List<CooldownEntry> entries = new List<CooldownEntry>();
}

[Serializable]
public class CooldownEntry
{
    public string id;
    public string endTimeString; // JSON'da saklamak için
}


public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance;

    private Dictionary<string, DateTime> cooldownTimers = new Dictionary<string, DateTime>();
    private string savePath => Application.persistentDataPath + "/cooldownData.json";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadCooldowns();
        }

    }

    public void StartCooldown(string id, float duration)
    {
        DateTime endTime = DateTime.Now.AddSeconds(duration);
        cooldownTimers[id] = endTime;
        SaveCooldowns();
    }

    public float GetRemainingTime(string id)
    {
        if (cooldownTimers.TryGetValue(id, out DateTime endTime))
        {
            TimeSpan remaining = endTime - DateTime.Now;
            return Mathf.Max((float)remaining.TotalSeconds, 0f);
        }
        return 0f;
    }


    public bool IsOnCooldown(string id)
    {
        return GetRemainingTime(id) > 0f;
    }

    public void SaveCooldowns()
    {
        CooldownData data = new CooldownData();
        foreach (var kvp in cooldownTimers)
        {
            if (kvp.Value > DateTime.Now)
            {
                data.entries.Add(new CooldownEntry
                {
                    id = kvp.Key,
                    endTimeString = kvp.Value.ToString("o") // ISO 8601 format
                });
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }


    public void LoadCooldowns()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        CooldownData data = JsonUtility.FromJson<CooldownData>(json);

        cooldownTimers.Clear();
        foreach (var entry in data.entries)
        {
            if (DateTime.TryParse(entry.endTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime endTime))
            {
                cooldownTimers[entry.id] = endTime;
            }
        }
    }
    public void ResetAllCooldowns()
    {
        cooldownTimers.Clear();

        // Kaydedilen cooldown dosyasını da sil
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

}
