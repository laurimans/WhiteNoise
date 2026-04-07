using UnityEngine;
using System.Collections;

public class TransitionUI : MonoBehaviour
{
    
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;

        this.gameObject.SetActive(true);
    }

    public IEnumerator PhaseTransition(System.Action changeLogic, AudioClip audioClip)
    {
        // Fade In
        canvas.blocksRaycasts = true; 
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        canvas.alpha = 1;

        changeLogic?.Invoke();

        if(audioClip != null) AudioManager.Instance.PlaySFX(audioClip);
        yield return new WaitForSeconds(1.5f);
        
        // Fade Out 
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
    }
   
}
