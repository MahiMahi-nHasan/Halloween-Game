using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 20f;
    public AudioClip bounceSound;

    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter()
    {
        GameManager.active.universalSoundEffect.PlayOneShot(bounceSound);
    }
}