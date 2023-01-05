using UnityEngine;

/// <summary>
/// камера параллакса
/// </summary>
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float delta);
    public ParallaxCameraDelegate cameraDelegate;

    private float position;


    void Start()
    {
        position = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x != position)
        {
            if (cameraDelegate != null)
                cameraDelegate(position - transform.position.x);

            position = transform.position.x;
        }
    }
}
