using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class BindingManager : MonoBehaviour
{
    public static BindingManager Instance;

    private readonly Dictionary<string, KeyCode> controls = new();

    [Serializable] class Root { public Map[] actionMaps; }
    [Serializable] class Map { public string name; public Action[] actions; }
    [Serializable] class Action { public string name; public string[] bindings; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadControlsOnly();
    }

    public void LoadControlsOnly()
    {
        string path = Path.Combine(Application.persistentDataPath, "keybindings.json");
        Debug.Log($"[Bindings] path: {path}");

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Bindings] keybindings.json not found.");
            return;
        }

        string json = File.ReadAllText(path);
        Root root = JsonUtility.FromJson<Root>(json);

        if (root?.actionMaps == null)
        {
            Debug.LogError("[Bindings] JSON parse failed: actionMaps is null (structure mismatch).");
            return;
        }

        // 1) find ONLY "Controls"
        Map controlsMap = null;
        foreach (var m in root.actionMaps)
            if (m != null && m.name == "Controls") { controlsMap = m; break; }

        if (controlsMap?.actions == null)
        {
            Debug.LogError("[Bindings] 'Controls' map not found (or has no actions).");
            return;
        }

        // 2) load ONLY its actions
        controls.Clear();
        foreach (var a in controlsMap.actions)
        {
            if (a?.bindings == null || a.bindings.Length == 0) continue;

            string keyName = (a.bindings[0] ?? "").Trim();
            if (string.IsNullOrWhiteSpace(keyName)) continue;

            if (TryToKeyCode(keyName, out var code))
            {
                controls[a.name] = code;
                Debug.Log($"[Bindings] {a.name} -> {code}");
            }
            else
            {
                Debug.LogWarning($"[Bindings] Unknown key '{keyName}' for action '{a.name}'");
            }
        }
    }

    static bool TryToKeyCode(string s, out KeyCode code)
    {
        // French aliases from your JSON
        if (s == "Espace") { code = KeyCode.Space; return true; }
        if (s == "Maj") { code = KeyCode.LeftShift; return true; }
        if (s == "Entree") { code = KeyCode.Return; return true; }
        if (s == "Echap") { code = KeyCode.Escape; return true; }

        // Single letter (H/J/K/N etc.)
        if (s.Length == 1 && char.IsLetter(s[0]))
            return Enum.TryParse(s.ToUpperInvariant(), out code);

        // If you later store "Space", "LeftShift", etc.
        return Enum.TryParse(s, true, out code);
    }

    public KeyCode GetControl(string actionName)
        => controls.TryGetValue(actionName, out var code) ? code : KeyCode.None;
}
