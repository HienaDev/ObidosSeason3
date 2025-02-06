using System.Collections;
using TMPro;
using UnityEngine;

public class FadeBlackScreen : MonoBehaviour
{

    [SerializeField] private GameObject fadeMask;
    private Vector3 initialScale;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private TextMeshProUGUI dayText;

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

    public void SetDay(string day, bool toggle)
    {
        dayText.gameObject.SetActive(toggle);
        dayText.text = day + " April 1974";
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
