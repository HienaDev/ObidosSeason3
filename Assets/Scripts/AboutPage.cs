using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutPage : MonoBehaviour
{
    [SerializeField]
    private FadeBlackScreen fadeBlackScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReturnDelay());
        }
    }

    private IEnumerator ReturnDelay()
    {
        fadeBlackScreen.Fade(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
