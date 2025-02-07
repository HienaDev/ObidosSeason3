using UnityEngine;

public class ApplicationCloser : MonoBehaviour
{

    
    public void CloseGame()
    {
            #if UNITY_EDITOR
            // If in Unity Editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // If not in the Editor (i.e., in a built application), quit the application
            Application.Quit();
            #endif
    }
}