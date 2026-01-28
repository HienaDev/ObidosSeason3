using System.Collections;
using UnityEngine;

public class PolicemanIntroduction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private FadeBlackScreen fade;

    public bool CanProgress {  get; set; } = true;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        fade = FindAnyObjectByType<FadeBlackScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanProgress)
        {
            StartCoroutine(StartLevelCoroutine());
        }
    }

    private IEnumerator StartLevelCoroutine()
    {
        Time.timeScale = 1f;
        CanProgress = false;
        fade.Fade(false);
        yield return new WaitForSecondsRealtime(1.2f);
        fade.Fade(true);
        playerMovement.DelayStartMovement();
        gameObject.SetActive(false);
    }
}
