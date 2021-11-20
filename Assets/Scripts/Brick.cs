using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    public int PointValue;
    private ParticleSystem _burst;
    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();
        _burst = GetComponent<ParticleSystem>();
        var color = new Color(1f, 0.69f, 0f);
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", color);
                break;
            case 2:
                block.SetColor("_BaseColor", color);
                break;
            case 5:
                block.SetColor("_BaseColor", color);
                break;
            default:
                block.SetColor("_BaseColor", color);
                break;
        }
        renderer.SetPropertyBlock(block);
    }
    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(PointValue);
        _burst.Play();
        Destroy(gameObject, 0.2f);
    }
}
