using System.Collections;
using TMPro;
using UnityEngine;

public class FadeBlackScreen : MonoBehaviour
{

    [SerializeField] private GameObject fadeMask;
    private Vector3 initialScale;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField]
    private Instructions instructions;
    [SerializeField]
    private GameObject introduction;

    private bool inIntro = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = fadeMask.transform.localScale;

        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inIntro)
        {
            inIntro = false;
            introduction.SetActive(false);
            instructions.gameObject.SetActive(true);
            instructions.ActiveFromIntro(true);
            FadeIn();
        }
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
            lerpValue += Time.unscaledDeltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, lerpValue);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.1f);

        float lerpValue = 0f;

        Vector3 currentScale = fadeMask.transform.localScale;

        while (lerpValue < 1f)
        {
            lerpValue += Time.unscaledDeltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, initialScale, lerpValue);
            yield return null;
        }
    }

    public void InIntroduction(bool intro)
    {
        introduction.SetActive(intro);
        StartCoroutine(IntroCoroutine(intro));
    }

    private IEnumerator IntroCoroutine(bool intro)
    {
        yield return new WaitForSeconds(0.5f);
        inIntro = intro;
    }
}
