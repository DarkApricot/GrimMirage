using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayShaderHandler : MonoBehaviour
{
    public Material ScreenMat;
    public float isForcingBack;
    [SerializeField] private Color col;

    public void OnRenderImage(RenderTexture _src, RenderTexture _dest)
    {
        Graphics.Blit(_src, _dest, ScreenMat);
    }

    private void Update()
    {
        col = ScreenMat.GetColor("_ScreenTint");
        ScreenMat.SetFloat("UnscaledTime", Time.unscaledTime);
        ScreenMat.SetFloat("IsForcingBack", isForcingBack);
    }
}
