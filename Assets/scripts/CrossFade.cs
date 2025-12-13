using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;
    
    private void Start()
    {
        // Ensure proper initial state
        if (crossFade != null)
        {
            crossFade.alpha = 0f;
            crossFade.gameObject.SetActive(false);
        }
    }
    
    public override IEnumerator AnimateTransitionIn()
    {
        if (crossFade == null) yield break;
        
        crossFade.gameObject.SetActive(true);
        crossFade.blocksRaycasts = true;
        crossFade.interactable = true;
        
        float duration = 1f;
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            crossFade.alpha = Mathf.Clamp01(timer / duration);
            yield return null;
        }
        
        crossFade.alpha = 1f;
    }
 
    public override IEnumerator AnimateTransitionOut()
    {
        if (crossFade == null) yield break;
        
        float duration = 1f;
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            crossFade.alpha = Mathf.Clamp01(1f - (timer / duration));
            yield return null;
        }
        
        crossFade.alpha = 0f;
        crossFade.blocksRaycasts = false;
        crossFade.interactable = false;
        crossFade.gameObject.SetActive(false);
    }
}