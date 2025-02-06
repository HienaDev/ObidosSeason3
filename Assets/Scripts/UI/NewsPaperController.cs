using UnityEngine;

public class NewsPaperController : MonoBehaviour
{
    [SerializeField] private KeyCode toggleNewspaper = KeyCode.Tab;
    [SerializeField] private GameObject newsPaper;

    private LevelManager levelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(toggleNewspaper) && levelManager.isRunning)
        {
            newsPaper.SetActive(!newsPaper.activeSelf);
        }
    }
}
