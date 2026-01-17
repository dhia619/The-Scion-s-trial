using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    
    [Header("Settings")]
    public float textSpeed = 0.05f;
    public float autoHideTime = 2f; // Time to auto-hide after text is displayed
    
    private string[] currentLines;
    private int currentIndex;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private float hideTimer;
    private bool timerActive = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Hide dialogue on start
        dialoguePanel.SetActive(false);
    }
    
    void Update()
    {
        if (!dialoguePanel.activeInHierarchy) return;
        
        // Update auto-hide timer
        if (timerActive)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                // Time's up - hide dialogue
                HideDialogue();
                return;
            }
        }
        
        // Player input handling
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.S) || Input.GetButton("Skip"))
        {
            // Reset timer on player input
            ResetHideTimer();
            
            if (isTyping)
            {
                // Skip typing animation
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                
                // Show full text immediately
                dialogueText.text = currentLines[currentIndex];
                isTyping = false;
                timerActive = true; // Start timer now that text is complete
                hideTimer = autoHideTime;
            }
            else
            {
                // Go to next line
                currentIndex++;
                if (currentIndex < currentLines.Length)
                {
                    ShowNextLine();
                }
                else
                {
                    // End dialogue
                    HideDialogue();
                }
            }
        }
    }
    
    public void ShowDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;
        
        currentLines = lines;
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        timerActive = false; // Timer starts after typing completes
        ShowNextLine();
    }
    
    public void ShowDialogue(string singleLine)
    {
        ShowDialogue(new string[] { singleLine });
    }
    
    private void ShowNextLine()
    {
        dialogueText.text = "";
        isTyping = true;
        timerActive = false; // Timer inactive during typing
        typingCoroutine = StartCoroutine(TypeText(currentLines[currentIndex]));
    }
    
    private IEnumerator TypeText(string line)
    {
        // Type out the text character by character
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        
        // Typing complete
        isTyping = false;
        
        // Start auto-hide timer
        timerActive = true;
        hideTimer = autoHideTime;
    }
    
    private void ResetHideTimer()
    {
        // Reset the auto-hide timer
        timerActive = true;
        hideTimer = autoHideTime;
    }
    
    public void HideDialogue()
    {
        // Stop any ongoing typing
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        
        // Hide the panel
        dialoguePanel.SetActive(false);
        
        // Reset all state
        isTyping = false;
        timerActive = false;
    }
    
    public bool IsDialogueActive()
    {
        return dialoguePanel.activeInHierarchy;
    }
}