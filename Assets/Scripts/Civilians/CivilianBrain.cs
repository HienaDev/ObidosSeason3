using System.Collections;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;

public class CivilianBrain : MonoBehaviour
{
    [SerializeField] private CivilianInstance _civilianPrefab;
    public List<CivilianInstance> ActiveCivilians { get; private set; }

    public PathfindingManager PathfindingManager { get; private set; }

    private void Start()
    {
        PathfindingManager = FindFirstObjectByType<PathfindingManager>();
        ActiveCivilians = new List<CivilianInstance>();

        CreateNewCivilian();
        StartCoroutine(C_ConstantSpawn());
    }


    private IEnumerator C_ConstantSpawn()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(.01f);
            CreateNewCivilian();
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        foreach (CivilianInstance ci in ActiveCivilians)
        {
            ci.I_Update(delta);
        }
    }

    public Node GetFreeCivilianSpot()
    {
        int x = Random.Range(0, PathfindingManager.Nodes.GetLength(1)); // Columns
        int y = Random.Range(0, PathfindingManager.Nodes.GetLength(0)); // Rows
        return PathfindingManager.Nodes[x, y];
    }

    public void CreateNewCivilian()
    {
        Node spawnNode = GetFreeCivilianSpot();
        CivilianInstance newCI = Instantiate(_civilianPrefab);

        ActiveCivilians.Add(newCI);
        newCI.Initialize(this, spawnNode);
    }

}
