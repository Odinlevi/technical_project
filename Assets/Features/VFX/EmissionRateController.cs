using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EmissionRateController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private Material _material;

    public float RateChangeSpeed = 0.3f;
    public float MaximumRate = 3f;
    public Color BaseColor = Color.red;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        _material = _meshRenderer.materials[0];
    }

    private void FixedUpdate()
    {
        float emission = Mathf.PingPong(Time.time * RateChangeSpeed, MaximumRate);
        var linearToGamma = Mathf.LinearToGammaSpace(emission);
        Color finalColor = BaseColor * linearToGamma;
        _material.SetColor(EmissionColor, finalColor);
    }
}
