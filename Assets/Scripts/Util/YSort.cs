using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _maiSr;
    [SerializeField] private float _yOffset = 0;
    [SerializeField] private SpriteRenderer[] _extraSpriteRenderers;

    private void LateUpdate()
    {
        // Update Y sorting
        float newOrderZ = (transform.position.y + _yOffset) * .001f;
        Vector3 newPos = _maiSr.transform.position;
        newPos.z = newOrderZ;
        _maiSr.transform.position = newPos;
        foreach (SpriteRenderer sr in _extraSpriteRenderers)
        {
            newPos = sr.transform.position;
            newPos.z = newOrderZ + 0.0001f;
            sr.transform.position = newPos;
        }
    }
}
