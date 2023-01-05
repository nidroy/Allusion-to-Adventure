using UnityEngine;

/// <summary>
/// слой параллакса
/// </summary>
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float offset; // смещение


    public void Move(float delta)
    {
        Vector3 position = transform.localPosition;
        position.x -= delta * offset;
        transform.localPosition = position;
    }
}
