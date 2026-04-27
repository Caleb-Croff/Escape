using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCasting : MonoBehaviour
{
    public static float DistanceFromTarget;
    public float ToTarget;

    private Key _currentKey;

    void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, transform.TransformDirection(Vector3.forward), out hit))
        {
            DistanceFromTarget = hit.distance;
            ToTarget = hit.distance;

            Key key = hit.collider.GetComponentInParent<Key>();

            if (key != null && hit.distance <= 5f)
            {
                if (_currentKey != key)
                {
                    if (_currentKey != null) _currentKey.ShowHover(false);
                    _currentKey = key;
                }

                _currentKey.ShowHover(true);

                if (Keyboard.current.eKey.wasPressedThisFrame)
                    _currentKey.TryCollect();
            }
            else
            {
                ClearCurrentKey();
            }
        }
        else
        {
            ClearCurrentKey();
        }
    }

    private void ClearCurrentKey()
    {
        if (_currentKey != null)
        {
            _currentKey.ShowHover(false);
            _currentKey = null;
        }
    }
}
