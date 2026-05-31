using System; 
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightGlitch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float shortTab = 0.2f;
    [SerializeField] private float longTab = 0.6f;
    [SerializeField] private float pause = 0.2f;
    [SerializeField] private int maxRepetitions = 5;

    public static event Action<bool> OnSOSLightPulse;

    private bool isSOSActive = false;

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            isSOSActive = false;
            StartSOS();
            Debug.Log("Debugueando luces");
        }
    }

    public void StartSOS()
    {
        if (!isSOSActive)
        {
            StartCoroutine(SOSRoutine());
        }
    }

    public void StopSOS()
    {
        StopAllCoroutines();
        isSOSActive = false;
        OnSOSLightPulse?.Invoke(true);
    }

    IEnumerator SOSRoutine()
    {
        isSOSActive = true;

        for (int iteration = 0; iteration < maxRepetitions; iteration++)
        {
            for (int i = 0; i < 3; i++) yield return StartCoroutine(Pulse(shortTab));
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++) yield return StartCoroutine(Pulse(longTab));
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++) yield return StartCoroutine(Pulse(shortTab));

            yield return new WaitForSeconds(2f);
        }

        isSOSActive = false;
        OnSOSLightPulse?.Invoke(true); // Se queda encendido
    }

    IEnumerator Pulse(float duration)
    {
        OnSOSLightPulse?.Invoke(true); 
        yield return new WaitForSeconds(duration);
        OnSOSLightPulse?.Invoke(false);
        yield return new WaitForSeconds(pause);
    }
}