using System.Collections;
using UnityEngine;

public class FadeBlackScreen : MonoBehaviour
{

    [SerializeField] private GameObject fadeMask;
    private Vector3 initialScale;
    [SerializeField] private float fadeSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = fadeMask.transform.localScale;

        StartCoroutine(FadeIn());
    }

    public void Fade(bool fadeIn)
    {
        if(fadeIn) StartCoroutine(FadeIn());
        else StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        float lerpValue = 0f;

        Vector3 currentScale = fadeMask.transform.localScale;

        while(lerpValue < 1f)
        {
            lerpValue += Time.deltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, lerpValue);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float lerpValue = 0f;

        Vector3 currentScale = fadeMask.transform.localScale;

        while (lerpValue < 1f)
        {
            lerpValue += Time.deltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, initialScale, lerpValue);
            yield return null;
        }
    }
}
