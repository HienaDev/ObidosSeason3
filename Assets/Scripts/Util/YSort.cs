using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _maiSr;
    [SerializeField] private SpriteRenderer[] _extraSpriteRenderers;

    private void LateUpdate()
    {
        // Update Y sorting
        float newOrderZ = (transform.position.y - Camera.main.transform.position.y) * .5f;
        Vector3 newPos = _maiSr.transform.position;
        newPos.z = newOrderZ;
        _maiSr.transform.position = newPos;
        foreach (SpriteRenderer sr in _extraSpriteRenderers)
        {
            newPos = sr.transform.position;
            newPos.z = newOrderZ + 0.1f;
            sr.transform.position = newPos;
        }
    }
}
