using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceLoadScene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadRevolution();
        }
    }

    public void LoadRevolution()
    {
        SceneManager.LoadScene("REVOLUTION");
    }
}
