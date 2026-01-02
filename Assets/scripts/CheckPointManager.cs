using UnityEngine;
using System;
using System.IO;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string saveFileName = "checkpoint.json";
    [SerializeField] private bool createBackupOnSave = true;

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);
    private string BackupPath => Path.Combine(Application.persistentDataPath, saveFileName + ".bak");

    [System.Serializable]
    public class CheckpointData
    {
        public Vector3 position = Vector3.zero;
        public long timestamp;           // When this checkpoint was created (UTC unix seconds)
        // You can add more later: int keys, float health, etc.
    }

    private CheckpointData currentData = new CheckpointData();

    // Events for other systems to react (optional but useful)
    public event System.Action<Vector3> OnCheckpointLoaded;
    public event System.Action OnCheckpointSaved;
    public event System.Action OnCheckpointDeleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Saves the current checkpoint position.
    /// </summary>
    public void SaveCheckpoint(Vector3 position)
    {
        currentData = new CheckpointData
        {
            position = position,
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        try
        {
            string json = JsonUtility.ToJson(currentData, true);

            // Optional: create backup before overwriting
            if (createBackupOnSave && File.Exists(SavePath))
            {
                File.Copy(SavePath, BackupPath, true);
            }

            File.WriteAllText(SavePath, json);
            Debug.Log($"[Checkpoint] Saved position {position} at {SavePath} (time: {DateTimeOffset.FromUnixTimeSeconds(currentData.timestamp):yyyy-MM-dd HH:mm})");

            OnCheckpointSaved?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Checkpoint] Save failed: {ex.Message}");
        }
    }
    public bool TryLoadCheckpoint(out Vector3 position)
    {
        position = Vector3.zero;

        if (!File.Exists(SavePath))
        {
            Debug.Log("[Checkpoint] No save file found");
            return false;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<CheckpointData>(json);

            position = data.position;
            currentData = data;

            Debug.Log($"[Checkpoint] Loaded position {position} (saved {DateTimeOffset.FromUnixTimeSeconds(data.timestamp):yyyy-MM-dd HH:mm})");

            OnCheckpointLoaded?.Invoke(position);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Checkpoint] Load failed: {ex.Message}");

            // Try backup
            if (File.Exists(BackupPath))
            {
                Debug.Log("[Checkpoint] Attempting to load from backup...");
                try
                {
                    string backupJson = File.ReadAllText(BackupPath);
                    var backupData = JsonUtility.FromJson<CheckpointData>(backupJson);
                    position = backupData.position;
                    currentData = backupData;
                    Debug.Log("[Checkpoint] Successfully loaded from backup");
                    OnCheckpointLoaded?.Invoke(position);
                    return true;
                }
                catch (Exception backupEx)
                {
                    Debug.LogError($"[Checkpoint] Backup load failed: {backupEx.Message}");
                }
            }

            return false;
        }
    }

    public bool HasCheckpoint()
    {
        return File.Exists(SavePath);
    }

    public void DeleteCheckpoint()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
            if (File.Exists(BackupPath))
            {
                File.Delete(BackupPath);
            }

            currentData = new CheckpointData(); // reset in-memory state
            Debug.Log("[Checkpoint] Checkpoint deleted");

            OnCheckpointDeleted?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Checkpoint] Delete failed: {ex.Message}");
        }
    }

    
    private void Load()
    {
        TryLoadCheckpoint(out _); // just to populate currentData
    }

    // Debug helpers (visible in Inspector context menu)
    [ContextMenu("Delete Checkpoint (Editor)")]
    private void DeleteInEditor() => DeleteCheckpoint();

    [ContextMenu("Save Test Checkpoint (Editor)")]
    private void SaveTestInEditor()
    {
        SaveCheckpoint(new Vector3(0, 5, 0));
    }
}