using TMPro;
using UnityEngine;

public class FaultManager : MonoBehaviour
{



    private int faultCounter = 0;

    [Header("Possible Text When Censoring")]
    [SerializeField] private string[] textInnocent;
    [SerializeField] private string[] textGroup;
    [SerializeField] private string[] textTalking;
    [SerializeField] private string[] textFashion;
    [SerializeField] private string[] textItem;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI[] noteBookLines;
    private int currentLine = 0;
    [SerializeField] private TextMeshProUGUI faultCounterUI;

    [SerializeField] private LevelManager levelManager;

    public static FaultManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ResetFaults()
    {
        faultCounter = 0;
        levelManager.anomaliesCount = faultCounter;
    }

    public void AddFault()
    {
        faultCounter++;
        levelManager.anomaliesCount = faultCounter;
        faultCounterUI.text = faultCounter.ToString();
    }

    public void RemoveFault()
    {
        faultCounter--;
        levelManager.anomaliesCount = faultCounter;
        faultCounterUI.text = faultCounter.ToString();
    }

    public void ClearFault(CivilianFaultType faultType)
    {
        if (faultType == CivilianFaultType.None)
        {
            // Add badge censoring code
            return;
        }
            

        switch(faultType)
        {
            case CivilianFaultType.Group:
                noteBookLines[currentLine].text = textGroup[Random.Range(0, textGroup.Length)];
                break;
            case CivilianFaultType.Talking:
                noteBookLines[currentLine].text = textTalking[Random.Range(0, textTalking.Length)];
                break;

            case CivilianFaultType.Fashion:
                noteBookLines[currentLine].text = textFashion[Random.Range(0, textFashion.Length)];
                break;

            case CivilianFaultType.Item:
                noteBookLines[currentLine].text = textItem[Random.Range(0, textItem.Length)];
                break;

        }
        currentLine++;

        RemoveFault();

    }


}
