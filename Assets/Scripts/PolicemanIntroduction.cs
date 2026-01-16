using UnityEngine;

public class PolicemanIntroduction : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
