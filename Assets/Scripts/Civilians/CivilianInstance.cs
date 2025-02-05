using System;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;

public class CivilianInstance : MonoBehaviour
{
    public const int OCCUPIED_NODE_HVALUE = 3;
    public const int WALKING_NODE_HVALUE = 2;
    private const string IDLE_TRIGGER = "idle";
    private const string WALK_TRIGGER = "walk";

    [Header("Mechanic")]
    [SerializeField] private bool _debug;
    [SerializeField] private CivilianState _currentState;
    [Header("Visuals")]
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _maiSr;
    [SerializeField] private SpriteRenderer[] _extraSpriteRenderers;

    private CivilianBrain _brain;

    // Node handling
    private Node _curNode;
    private List<Point> _curPath;
    private int _curPathPointI;
    private NodeTravelingInfo _curTravelInfo;
    private Node _targetFinalNode;

    private float _timeInCurrentState;

    [SerializeField] private TalkingBubble _talkingBubble;

    private void Start()
    {
        if (_debug)
        {
            FindFirstObjectByType<CivilianBrain>().ActiveCivilians.Add(this);
        }
    }

    public void Initialize(CivilianBrain brain, Node startNode)
    {
        _currentState = CivilianState.Idle;
        _brain = brain;

        transform.position = startNode.WorldPosition;

        _curNode = startNode; // Set Default
        ChangeNode(_curNode);
    }

    private void ChangeNode(Node newNode)
    {
        if (_curNode.price == WALKING_NODE_HVALUE)
            _curNode.price = 0;

        if (newNode.price != OCCUPIED_NODE_HVALUE)
            newNode.price = WALKING_NODE_HVALUE;

        _curNode = newNode;
    }

    public void I_Update(float delta)
    {
        _timeInCurrentState += delta;

        StateMachine(delta);
        UpdateVisuals();
    }

    private void SetNewFinalNode(Node nodeToMoveTo)
    {
        if (_targetFinalNode != default)
        {
            _targetFinalNode.price = 0;
        }
        nodeToMoveTo.price = OCCUPIED_NODE_HVALUE;

        _targetFinalNode = nodeToMoveTo;
        _curPath = _brain.PathfindingManager.FindPath(_curNode, _targetFinalNode);
        _curPathPointI = 0;

        TravelToNextNode();
        ChangeState(CivilianState.Traveling);
    }

    private void TravelToNextNode()
    {
        int count = _curPath.Count;
        if (_targetFinalNode == _curTravelInfo.TargetNode || _curPathPointI + 1 == count || count == 0) // Reached Destination
        {
            ChangeState(CivilianState.Idle);
            return;
        }

        _curPathPointI++;
        Node nextTravelNode = _brain.PathfindingManager.Nodes[_curPath[_curPathPointI].x, _curPath[_curPathPointI].y];
        Vector2 dir = new Vector2(nextTravelNode.WorldPosition.x - transform.position.x, nextTravelNode.WorldPosition.y - transform.position.y).normalized;

        _curTravelInfo = new NodeTravelingInfo(dir, nextTravelNode);
        ChangeNode(_curTravelInfo.TargetNode);

        if (dir.x > 0)
            _anim.transform.right = Vector2.right;
        else
            _anim.transform.right = Vector2.left;
    }

    private void UpdateVisuals()
    {
        // Update Y sorting
        int newOrder = -(int)Mathf.Ceil(transform.position.y - Camera.main.transform.position.y) * 10;
        if (newOrder == _maiSr.sortingOrder) return;

        _maiSr.sortingOrder = newOrder;
        foreach (SpriteRenderer sr in _extraSpriteRenderers)
            sr.sortingOrder = newOrder + 1;
    }

    private void ChangeState(CivilianState newState)
    {
        if (newState == _currentState) return;
        _currentState = newState;
        _timeInCurrentState = 0;
        OnStateChanged?.Invoke(newState);

        switch (newState)
        {
            case CivilianState.Idle:
                _anim.SetTrigger(IDLE_TRIGGER);
                break;
            case CivilianState.Traveling:
                _anim.SetTrigger(WALK_TRIGGER);
                break;
            case CivilianState.Talking_Group:
                _talkingBubble.StartTalking(UnityEngine.Random.Range(0, 100) < 40);
                break;
        }
    }

    private void StateMachine(float delta)
    {
        switch (_currentState)
        {
            case CivilianState.Idle:
                if (_timeInCurrentState > 2)
                {
                    Node newNode = _brain.GetFreeCivilianSpot();
                    SetNewFinalNode(newNode);
                }
                break;
            case CivilianState.Traveling:
                transform.position = transform.position + (Vector3)_curTravelInfo.Direction * delta;

                if (Vector2.Distance(transform.position, _curNode.WorldPosition) < 0.3f)
                {
                    TravelToNextNode();
                }
                break;
            case CivilianState.Talking_Group:
                break;
        }
    }

    public enum CivilianState
    {
        Idle,
        Traveling,
        Talking_Group,

    }

    private struct NodeTravelingInfo
    {
        public Vector2 Direction;
        public Node TargetNode;

        public NodeTravelingInfo(Vector2 dir, Node n)
        {
            Direction = dir;
            TargetNode = n;
        }
    }

    public event Action<CivilianState> OnStateChanged;
}
