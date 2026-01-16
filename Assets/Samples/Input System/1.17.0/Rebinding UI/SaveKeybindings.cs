using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// Script to save all keybindings to a JSON file.
    /// Attach this to a GameObject and connect it to your "Save Changes" button.
    /// </summary>
    public class SaveKeybindings : MonoBehaviour
    {
        [Tooltip("The input action asset containing all actions to save")]
        public InputActionAsset actions;

        [Tooltip("The filename (without extension) to save the bindings to. Will be saved in persistentDataPath")]
        public string saveFileName = "keybindings";

        [Tooltip("Whether to also log the saved bindings to console")]
        public bool logToConsole = true;

        /// <summary>
        /// Call this method from your "Save Changes" button onClick event
        /// Saves all keybindings as display strings (e.g., "LMB", "Space") to a readable JSON file
        /// </summary>
        public void SaveKeybindingsToFile()
        {
            if (actions == null)
            {
                Debug.LogError("SaveKeybindings: No InputActionAsset assigned!");
                return;
            }

            try
            {
                // Create a dictionary to store all bindings as display strings
                var bindingsData = new Dictionary<string, object>();
                
                // Get all action maps
                foreach (var actionMap in actions.actionMaps)
                {
                    var mapBindings = new Dictionary<string, object>();

                    // Get all actions in this map
                    foreach (var action in actionMap.actions)
                    {
                        var actionBindings = new List<string>();

                        // Get all bindings for this action
                        var bindingIndices = action.bindings;
                        for (int i = 0; i < bindingIndices.Count; i++)
                        {
                            var binding = bindingIndices[i];
                            
                            // Skip composite headers and parts
                            if (binding.isComposite || binding.isPartOfComposite)
                                continue;

                            // Get the display string for this binding (e.g., "LMB", "Space", "W")
                            string displayString = action.GetBindingDisplayString(i);
                            actionBindings.Add(displayString);
                        }

                        mapBindings[action.name] = actionBindings;
                    }

                    bindingsData[actionMap.name] = mapBindings;
                }

                // Convert to JSON
                string json = JsonUtility.ToJson(new BindingsWrapper(bindingsData), true);

                // Get the save path
                string savePath = Path.Combine(Application.persistentDataPath, saveFileName + ".json");

                // Write to file
                File.WriteAllText(savePath, json);

                if (logToConsole)
                {
                    Debug.Log($"✓ Keybindings saved successfully to: {savePath}");
                    Debug.Log($"Content:\n{json}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"SaveKeybindings: Error saving keybindings - {ex.Message}");
            }
        }

        /// <summary>
        /// Load saved keybindings from the JSON file
        /// Returns a Dictionary with action names and their saved key bindings
        /// </summary>
        public Dictionary<string, string> LoadSavedKeybindings()
        {
            var result = new Dictionary<string, string>();

            try
            {
                string savePath = Path.Combine(Application.persistentDataPath, saveFileName + ".json");

                if (!File.Exists(savePath))
                {
                    if (logToConsole)
                        Debug.LogWarning($"SaveKeybindings: No saved keybindings found at {savePath}");
                    return result;
                }

                string json = File.ReadAllText(savePath);
                var wrapper = JsonUtility.FromJson<BindingsWrapper>(json);

                // Extract bindings from the loaded data
                foreach (var actionMap in wrapper.actionMaps)
                {
                    foreach (var action in actionMap.actions)
                    {
                        // Store the first binding for this action (or all if needed)
                        if (action.bindings.Count > 0)
                        {
                            result[action.name] = action.bindings[0]; // Get first binding
                        }
                    }
                }

                if (logToConsole)
                    Debug.Log($"✓ Keybindings loaded successfully from: {savePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"SaveKeybindings: Error loading keybindings - {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Get a specific saved keybinding for an action
        /// </summary>
        public string GetSavedKeybinding(string actionName)
        {
            var saved = LoadSavedKeybindings();
            if (saved.ContainsKey(actionName))
                return saved[actionName];
            return string.Empty;
        }

        /// <summary>
        /// Get all bindings as a formatted string for display
        /// </summary>
        public string GetAllBindingsAsString()
        {
            if (actions == null)
                return "No InputActionAsset assigned";

            var result = new System.Text.StringBuilder();
            result.AppendLine("=== KEYBINDINGS ===\n");

            foreach (var actionMap in actions.actionMaps)
            {
                result.AppendLine($"[{actionMap.name}]");

                foreach (var action in actionMap.actions)
                {
                    result.AppendLine($"  {action.name}:");

                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];
                        if (binding.isComposite || binding.isPartOfComposite)
                            continue;

                        string displayString = action.GetBindingDisplayString(i);
                        result.AppendLine($"    - {displayString}");
                    }
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        /// <summary>
        /// Get the key binding for a specific action in the Controls action map
        /// </summary>
        /// <param name="actionName">Name of the action (e.g., "Move", "Attack", "Jump")</param>
        /// <returns>The display string of the binding (e.g., "W", "Space", "Left Mouse Button"), or empty string if not found</returns>
        public string GetControlsActionBinding(string actionName)
        {
            if (actions == null)
            {
                Debug.LogWarning("SaveKeybindings: No InputActionAsset assigned!");
                return string.Empty;
            }

            // Find the Controls action map
            var controlsMap = actions.FindActionMap("Controls");
            if (controlsMap == null)
            {
                Debug.LogWarning("SaveKeybindings: 'Controls' action map not found!");
                return string.Empty;
            }

            // Find the specific action
            var action = controlsMap.FindAction(actionName);
            if (action == null)
            {
                Debug.LogWarning($"SaveKeybindings: Action '{actionName}' not found in Controls map!");
                return string.Empty;
            }

            // Get the first binding's display string (skip composites)
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite || binding.isPartOfComposite)
                    continue;

                return action.GetBindingDisplayString(i);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get all bindings for a specific action in the Controls action map
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        /// <returns>List of all binding display strings for the action</returns>
        public List<string> GetControlsActionBindings(string actionName)
        {
            var bindings = new List<string>();

            if (actions == null)
            {
                Debug.LogWarning("SaveKeybindings: No InputActionAsset assigned!");
                return bindings;
            }

            // Find the Controls action map
            var controlsMap = actions.FindActionMap("Controls");
            if (controlsMap == null)
            {
                Debug.LogWarning("SaveKeybindings: 'Controls' action map not found!");
                return bindings;
            }

            // Find the specific action
            var action = controlsMap.FindAction(actionName);
            if (action == null)
            {
                Debug.LogWarning($"SaveKeybindings: Action '{actionName}' not found in Controls map!");
                return bindings;
            }

            // Get all bindings
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite || binding.isPartOfComposite)
                    continue;

                bindings.Add(action.GetBindingDisplayString(i));
            }

            return bindings;
        }

        /// <summary>
        /// Get all Controls actions and their bindings
        /// </summary>
        /// <returns>Dictionary with action names as keys and their bindings as values</returns>
        public Dictionary<string, List<string>> GetAllControlsBindings()
        {
            var result = new Dictionary<string, List<string>>();

            if (actions == null)
            {
                Debug.LogWarning("SaveKeybindings: No InputActionAsset assigned!");
                return result;
            }

            // Find the Controls action map
            var controlsMap = actions.FindActionMap("Controls");
            if (controlsMap == null)
            {
                Debug.LogWarning("SaveKeybindings: 'Controls' action map not found!");
                return result;
            }

            // Get all actions in Controls map
            foreach (var action in controlsMap.actions)
            {
                var bindings = new List<string>();

                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (binding.isComposite || binding.isPartOfComposite)
                        continue;

                    bindings.Add(action.GetBindingDisplayString(i));
                }

                if (bindings.Count > 0)
                    result[action.name] = bindings;
            }

            return result;
        }

        // Wrapper class for JsonUtility serialization
        [System.Serializable]
        private class BindingsWrapper
        {
            public List<ActionMapData> actionMaps = new List<ActionMapData>();

            public BindingsWrapper(Dictionary<string, object> data)
            {
                foreach (var mapEntry in data)
                {
                    var mapData = new ActionMapData { name = mapEntry.Key };
                    
                    if (mapEntry.Value is Dictionary<string, object> actions)
                    {
                        foreach (var actionEntry in actions)
                        {
                            var actionData = new ActionData { name = actionEntry.Key };
                            
                            if (actionEntry.Value is List<string> bindings)
                            {
                                actionData.bindings = bindings;
                            }
                            
                            mapData.actions.Add(actionData);
                        }
                    }

                    actionMaps.Add(mapData);
                }
            }
        }

        [System.Serializable]
        private class ActionMapData
        {
            public string name;
            public List<ActionData> actions = new List<ActionData>();
        }

        [System.Serializable]
        private class ActionData
        {
            public string name;
            public List<string> bindings = new List<string>();
        }

        // No longer need wrapper classes - we use the built-in JSON system
    }
}
