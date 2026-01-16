using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// Script to load and display saved keybindings when the controls page opens.
    /// </summary>
    public class ControlsPageLoader : MonoBehaviour
    {
        [SerializeField] private SaveKeybindings saveKeybindings;
        [SerializeField] private Text moveLeftKeyText;
        [SerializeField] private Text moveRightKeyText;
        [SerializeField] private Text jumpKeyText;
        [SerializeField] private Text attackKeyText;

        private void OnEnable()
        {
            if (saveKeybindings == null)
            {
                saveKeybindings = FindObjectOfType<SaveKeybindings>();
            }

            LoadAndDisplayKeybindings();
        }

        public void LoadAndDisplayKeybindings()
        {
            if (saveKeybindings == null)
            {
                Debug.LogWarning("SaveKeybindings not assigned!");
                return;
            }

            var savedKeybindings = saveKeybindings.LoadSavedKeybindings();

            if (moveLeftKeyText != null && savedKeybindings.ContainsKey("Move Left"))
                moveLeftKeyText.text = savedKeybindings["Move Left"];

            if (moveRightKeyText != null && savedKeybindings.ContainsKey("Move Right"))
                moveRightKeyText.text = savedKeybindings["Move Right"];

            if (jumpKeyText != null && savedKeybindings.ContainsKey("Jump"))
                jumpKeyText.text = savedKeybindings["Jump"];

            if (attackKeyText != null && savedKeybindings.ContainsKey("Attack"))
                attackKeyText.text = savedKeybindings["Attack"];
        }
    }
}
