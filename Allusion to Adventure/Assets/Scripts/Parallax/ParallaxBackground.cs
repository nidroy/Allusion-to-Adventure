using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// задний фон параллакса
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    private ParallaxCamera parallaxCamera;

    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();


    void Start()
    {
        parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
        parallaxCamera.cameraDelegate += Move;

        SetLayers();
    }


    void SetLayers()
    {
        parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
                parallaxLayers.Add(layer);
        }
    }

    void Move(float delta)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
            layer.Move(delta);
    }
}
