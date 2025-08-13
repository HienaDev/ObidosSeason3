using UnityEngine;

public class NewsPaperController : MonoBehaviour
{
    [SerializeField] private KeyCode toggleNewspaper = KeyCode.Tab;
    [SerializeField] private GameObject newsPaper;

    [SerializeField] private AudioClip[] openJournal;
    [SerializeField] private AudioClip[] closeJournal;

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
            if(newsPaper.activeSelf)
            {
                AudioSystem.PlaySound(openJournal);
            }
            else
            {
                AudioSystem.PlaySound(closeJournal);
            }
        }

        if (levelManager.isRunning == false)
        {
            newsPaper.SetActive(false);
        }
    }
}
