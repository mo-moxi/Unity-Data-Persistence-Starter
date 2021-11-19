using UnityEngine;

public class RotateSoundPivot : MonoBehaviour
{
    [SerializeField] private float speed;
    void FixedUpdate() => transform.Rotate(Vector3.up*speed);
}
