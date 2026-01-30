using System.Collections;
using TMPro;
using UnityEngine;

public class PolicemanIntroduction : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private AudioClip[] letterCloseSounds;

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
        dayText.text = string.Empty;
        CanProgress = false;
        fade.PolicemanFade();
        AudioSystem.PlaySound(letterCloseSounds);
        yield return new WaitForSecondsRealtime(1.2f);
        playerMovement.DelayStartMovement();
    }
}
