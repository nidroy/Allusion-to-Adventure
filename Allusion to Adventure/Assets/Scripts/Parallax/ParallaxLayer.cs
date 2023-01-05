using UnityEngine;

/// <summary>
/// ���� ����������
/// </summary>
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float offset; // ��������


    public void Move(float delta)
    {
        Vector3 position = transform.localPosition;
        position.x -= delta * offset;
        transform.localPosition = position;
    }
}
