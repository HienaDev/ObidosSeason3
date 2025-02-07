using UnityEngine;

public class GuardSalute : MonoBehaviour
{

    [SerializeField] private float saluteCooldown = 2f;
    private float justSaluted = 0f;

    [SerializeField] private LayerMask npcLayer;

    [SerializeField] private AudioClip[] grunts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int x = 1 << collision.gameObject.layer;

        if (x == npcLayer.value && Time.time - justSaluted > saluteCooldown)
        {
            justSaluted = Time.time;
            AudioSystem.PlaySound(grunts);
            GetComponent<Animator>().SetTrigger("Salute");

        }
    }

}
