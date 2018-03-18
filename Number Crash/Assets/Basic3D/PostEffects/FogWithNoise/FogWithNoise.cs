using UnityEngine;
using System.Collections;

public class FogWithNoise : PostEffectBase
{
    [Range(0f, 3.0f)]
    public float fogDensity = 1.0f;

    public Color fogColor = Color.white;

    public float fogStart = 0.0f;
    public float fogEnd = 2.0f;

    public Texture noiseTexture;

    [Range(-0.5f, 0.5f)]
    public float fogXSpeed = 0.1f;

    [Range(-0.5f, 0.5f)]
    public float fogYSpeed = 0.1f;

    [Range(0.0f, 3.0f)]
    public float noiseAmount = 1.0f;


    void OnEnable() {
        if (_camera.orthographic) {
            enabled = false;
            return;
        }
        _camera.depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fov = _camera.fieldOfView;
        float near = _camera.nearClipPlane;
        float aspect = _camera.aspect;

        float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        Vector3 toRight = cameraTransform.right * halfHeight * aspect;
        Vector3 toTop = cameraTransform.up * halfHeight;

        Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
        float scale = topLeft.magnitude / near;

        topLeft.Normalize();
        topLeft *= scale;

        Vector3 topRight = cameraTransform.forward * near + toRight + toTop;
        topRight.Normalize();
        topRight *= scale;

        Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
        bottomLeft.Normalize();
        bottomLeft *= scale;

        Vector3 bottomRight = cameraTransform.forward * near + toRight - toTop;
        bottomRight.Normalize();
        bottomRight *= scale;

        frustumCorners.SetRow(0, bottomLeft);
        frustumCorners.SetRow(1, bottomRight);
        frustumCorners.SetRow(2, topRight);
        frustumCorners.SetRow(3, topLeft);

        _Material.SetMatrix("_FrustumCornersRay", frustumCorners);

        _Material.SetFloat("_FogDensity", fogDensity);
        _Material.SetColor("_FogColor", fogColor);
        _Material.SetFloat("_FogStart", fogStart);
        _Material.SetFloat("_FogEnd", fogEnd);

        _Material.SetTexture("_NoiseTex", noiseTexture);
        _Material.SetFloat("_FogXSpeed", fogXSpeed);
        _Material.SetFloat("_FogYSpeed", fogYSpeed);
        _Material.SetFloat("_NoiseAmount", noiseAmount);

        Graphics.Blit(src, dest, _Material);
    }
}
