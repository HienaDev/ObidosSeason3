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

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;

        if (movEnabled)
        {
            if (Input.GetKey(up))
            {
                velocity.y = 1;
            }

            if (Input.GetKey(down))
            {
                velocity.y = -1;
            }

            if (Input.GetKey(right))
            {
                velocity.x = 1;
                lastSideMovementRight = true;
            }

            if (Input.GetKey(left))
            {
                velocity.x = -1;
                lastSideMovementRight = false;
            }
        }

        rb.linearVelocity = velocity.normalized * movSped;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        bool walking = rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool("walk", walking);

        if (Input.GetKeyDown(slap) && lastSlap)
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
