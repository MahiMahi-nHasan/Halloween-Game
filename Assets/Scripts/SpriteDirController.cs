using UnityEngine;

public class SpriteDirController : MonoBehaviour
{
    [SerializeField] Transform mainTransform;
    [SerializeField] Animator an;
    [SerializeField] SpriteRenderer Spriterenderer;
    [SerializeField] float backAngle = 65f;
    [SerializeField] float sideAngle = 155f;
    void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);
        Vector2 animationDir = new Vector2(0f, -1f);
        float angle = Mathf.Abs(signedAngle);

        if (angle < backAngle)
        {
            animationDir = new Vector2(0f, -1f);
        }
        else if (angle < sideAngle)
        {
            if (signedAngle < 0)
            {
                Spriterenderer.flipX = true;
            }
            else
            {
                Spriterenderer.flipX = false;
            }

            animationDir = new Vector2(1f, 0f);
        }
        else
        {
            animationDir = new Vector2(0f, 1f);
        }
        an.SetFloat("MoveX", animationDir.x);
        an.SetFloat("MoveY", animationDir.y);
    }
}
