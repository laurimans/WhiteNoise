using System.Collections;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlitchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform fakeCursor;
    [SerializeField] private GameObject errorScreen;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject glitchScreen;

    private bool isLagging = false;

    public void StartFinalSequence()
    {
        Cursor.visible = false; 
        fakeCursor.gameObject.SetActive(true);
        StartCoroutine(FullGlitchRoutine());
    }

    IEnumerator FullGlitchRoutine()
    {
        // Lag Cursor
        isLagging = true;
        yield return new WaitForSeconds(10f);

        // SOS
        isLagging = false;
        yield return StartCoroutine(MoveInSOS());

        // Mensaje error
        TriggerFinalError();
    }

    void Update()
    {
        // Debug
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            Debug.Log("Forzando secuencia final por Debug");
            StartFinalSequence();
        }


       if(isLagging)
        {

            Vector2 currentMousePos = Mouse.current.position.ReadValue();

            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                fakeCursor.parent as RectTransform,
                currentMousePos,
                null,
                out localMousePos
            );

            fakeCursor.anchoredPosition = Vector2.Lerp(fakeCursor.anchoredPosition, localMousePos, Time.deltaTime * 2f);
        }
    }

    IEnumerator MoveInSOS()
    {
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        float size = 150f;
        float spacing = 500f;

        // S
        Vector2[] sPath = {
            center + new Vector2(-spacing + size, 2*size),      // Arriba der
            center + new Vector2(-spacing + -size, 2*size),     // Arriba izq
            center + new Vector2(-spacing + -size, 0),          // Medio izq
            center + new Vector2(-spacing + size, 0),           // Medio der
            center + new Vector2(-spacing + size, -size*2),     // Abajo der
            center + new Vector2(-spacing + -size, -size*2)     // Abajo izq
        };

        // O
        Vector2[] oPath = {
            center + new Vector2(-size, size*2),      // Arriba izq
            center + new Vector2(size, size*2),       // Arriba der
            center + new Vector2(size, -size*2),      // Abajo der
            center + new Vector2(-size, -size*2),     // Abajo izq
            center + new Vector2(-size, size*2)       // Arriba izq
        };

        // S
        Vector2[] sPath2 = {
            center + new Vector2(spacing + size, 2*size),      // Arriba der
            center + new Vector2(spacing + -size, 2*size),     // Arriba izq
            center + new Vector2(spacing + -size, 0),          // Medio izq
            center + new Vector2(spacing + size, 0),           // Medio der
            center + new Vector2(spacing + size, -size*2),     // Abajo der
            center + new Vector2(spacing + -size, -size*2)     // Abajo izq
        };


        yield return StartCoroutine(FollowPath(sPath));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FollowPath(oPath));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FollowPath(sPath2));
        yield return new WaitForSeconds(1f);
    }

    IEnumerator FollowPath(Vector2[] path)
    {
        float speed = 3.5f; // Velocidad
        for (int i = 0; i < path.Length - 1; i++)
        {
            float t = 0;
            Vector2 start = path[i];
            Vector2 end = path[i + 1];

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                fakeCursor.position = Vector2.Lerp(start, end, t);

                // Trazo
                if (Time.frameCount % 3 == 0)
                {
                    GameObject ghost = Instantiate(ghostPrefab, fakeCursor.parent);
                    ghost.transform.position = fakeCursor.position;
                    Destroy(ghost, 2f);
                }

                yield return null;
            }
        }
    }

    IEnumerator RandomScreenGlitches()
    {
        while (true)
        {
            if (GameManager.Instance.GetCurrentPhase() == GamePhase.ThursdayMorning)
            {
                yield return new WaitForSeconds(Random.Range(5f, 15f));

                glitchScreen.SetActive(true);

                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                glitchScreen.SetActive(false);
            }
            yield return null;
        }
    }

    void TriggerFinalError()
    {
        errorScreen.SetActive(true);
        Invoke("ResetToDayOne", 8f);
    }

    void ResetToDayOne()
    {
        Cursor.visible = false;
        fakeCursor.gameObject.SetActive(true);
        glitchScreen.SetActive(false);

        GameManager.Instance.NextPhase();
    }

}
