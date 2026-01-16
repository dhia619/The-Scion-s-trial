using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

/// <summary>
/// Example script to display the current keybindings in your UI
/// Attach this to a Canvas with Text components to show the keybindings
/// </summary>
public class KeybindingsDisplay : MonoBehaviour
{
    [SerializeField] private Text moveLeftText;
    [SerializeField] private Text moveRightText;
    [SerializeField] private Text jumpText;
    [SerializeField] private Text attackText;

    private InputActionAsset inputActions;
    private SaveKeybindings saveKeybindings;

    void Start()
    {
        // Get the input actions
        inputActions = Resources.Load<InputActionAsset>("InputSystem_Actions");
        
        // Get the SaveKeybindings script (attach to same GameObject or find it)
        saveKeybindings = GetComponent<SaveKeybindings>();
        if (saveKeybindings == null)
            saveKeybindings = FindObjectOfType<SaveKeybindings>();

        UpdateDisplay();
    }

    /// <summary>
    /// Update all keybinding displays
    /// Call this method after rebinding actions
    /// </summary>
    public void UpdateDisplay()
    {
        if (saveKeybindings == null)
            return;

        // Option 1: Get individual action bindings
        if (moveLeftText != null)
            moveLeftText.text = "Move Left: " + saveKeybindings.GetControlsActionBinding("Move Left");

        if (moveRightText != null)
            moveRightText.text = "Move Right: " + saveKeybindings.GetControlsActionBinding("Move Right");

        if (jumpText != null)
            jumpText.text = "Jump: " + saveKeybindings.GetControlsActionBinding("Jump");

        if (attackText != null)
            attackText.text = "Attack: " + saveKeybindings.GetControlsActionBinding("Attack");

        // Option 2: Get all Controls bindings at once
        var allBindings = saveKeybindings.GetAllControlsBindings();
        Debug.Log("Current Keybindings:");
        foreach (var action in allBindings)
        {
            Debug.Log($"{action.Key}: {action.Value[0]}");
        }
    }
}
