using UnityEngine;

public class ShotHitEffect : MonoBehaviour, IShotEffect
{
    public void Setup(Color particlesColor)
    {
        ParticleSystem.MainModule settings = transform.Find("Particles").GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(particlesColor);
    }
}
