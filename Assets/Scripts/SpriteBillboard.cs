using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] bool freezeXZAxis = true;
    [SerializeField] bool bobbing = false;
    [SerializeField] float bobAmplitude = 0.1f;
    [SerializeField] float bobFrequency = 1f;
    Vector3 initialPos;

    [SerializeField] float fogDistance = 30;
    [SerializeField] float fadeDistance = 20;
    SpriteRenderer rend;

    private void Start()
    {
        initialPos = transform.position;
        rend = GetComponent<SpriteRenderer>();
        // Randomize sprite from list
        rend.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void LateUpdate()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
        if (bobbing)
        {
            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = initialPos + new Vector3(0f, bobOffset, 0f);
        }

        // Fade sprite based on distance
        float alpha = (fogDistance - Vector3.Distance(Camera.main.transform.position, initialPos)) / fadeDistance + 1;
        rend.color = new Color(
            rend.color.r,
            rend.color.g,
            rend.color.b,
            alpha
        );
    }
}
