using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode slap = KeyCode.Space;


    [SerializeField] private float movSped = 5f;
    private Vector2 velocity = Vector2.zero;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator _animator;

    [SerializeField] private AudioClip[] _pencilHitGroundClips;

    private bool lastSlap = true;
    private bool movEnabled = true;
    private bool lastSideMovementRight = true;
    private bool mouseClick = false;
    private Vector2 targetPosition = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;

        if (movEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetTargetPosition();
            }

            if (mouseClick)
            {
                velocity = (targetPosition - (Vector2)transform.position).normalized;
                LastSideMovement();

                if (Vector2.Distance(targetPosition, (Vector2)transform.position) < 0.1f)
                {
                    mouseClick = false;
                }
            }


            if (Input.GetKey(up))
            {
                velocity.y = 1;
                mouseClick = false;
            }

            if (Input.GetKey(down))
            {
                velocity.y = -1;
                mouseClick = false;
            }

            if (Input.GetKey(right))
            {
                velocity.x = 1;
                LastSideMovement();
                mouseClick = false;
            }

            if (Input.GetKey(left))
            {
                velocity.x = -1;
                LastSideMovement();
                mouseClick = false;
            }
        }

        rb.linearVelocity = velocity.normalized * movSped;

        UpdateAnimation();
    }

    private void LastSideMovement()
    {
        if (velocity.x < 0)
        {
            lastSideMovementRight = false;
        }
        else if (velocity.x > 0)
        {
            lastSideMovementRight = true;
        }
    }

    private void GetTargetPosition()
    {
        Vector3 point = new Vector3();
        Vector2 mousePos = new Vector2();

        mousePos = Input.mousePosition;

        point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

        mouseClick = true;
        targetPosition = new Vector2(point.x, point.y);
    }

    private void UpdateAnimation()
    {
        bool walking = rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool("walk", walking);

        if ((Input.GetKeyDown(slap) || Input.GetMouseButtonDown(1)) && lastSlap)
        {
            _animator.SetTrigger("slap");
            AudioSystem.PlaySound(_pencilHitGroundClips);

            if (!movEnabled)
            {
                lastSlap = false;
            }
        }

        if (walking)
        {

            if (lastSideMovementRight)
                _animator.transform.right = Vector2.right;
            else
                _animator.transform.right = Vector2.left;
        }

        // Debug.Log(rb.linearVelocity.magnitude);
    }

    public void StartMoving(bool canMove)
    {
        movEnabled = canMove;

        if (canMove)
        {
            lastSlap = canMove;
        }
        else
        {
            StartCoroutine(LastSlapCR());
        }
    }

    private IEnumerator LastSlapCR()
    {
        yield return new WaitForSeconds(0.1f);
        lastSlap = false;
    }
}
