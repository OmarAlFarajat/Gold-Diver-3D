// Gives a fog like effect when underwater
// Source code adapted from: https://medium.com/@mukulkhanna/creating-basic-underwater-effects-in-unity-9a9400bde928

using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour
{
    public float waterHeight;

    private bool isUnderwater;
    private Color normalColor;
    private Color underwaterColor;

    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.75f);
    }

    void Update()
    {
        if ((transform.position.y < waterHeight) != isUnderwater)
        {
            isUnderwater = transform.position.y < waterHeight;
            if (isUnderwater) SetUnderwater();
            if (!isUnderwater) SetNormal();
        }
    }

    void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.01f;

    }

    void SetUnderwater()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.1f;

    }
}