using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private DetectNpcs detectNpcs;
    [SerializeField]
    private GameObject instructionsCivilian;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private FadeBlackScreen fade;

    private bool activeFromMenu = false;
    private bool activeFromIntroduction = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.StartMoving(true);
            gameObject.SetActive(false);

            if (activeFromMenu)
            {
                activeFromMenu = false;
                instructionsCivilian.SetActive(true);
                detectNpcs.CanInteract(true);
            }

            if (activeFromIntroduction)
            {
                activeFromIntroduction = false;
                fade.FadeOut();
                levelManager.StartLevel(0);
            }
        }
    }

    public void ActiveFromMenu(bool active)
    {
        activeFromMenu = active;
    }

    public void ActiveFromIntro(bool active)
    {
        activeFromIntroduction = active;
    }
}
