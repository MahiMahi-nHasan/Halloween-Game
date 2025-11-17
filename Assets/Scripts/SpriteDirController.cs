using UnityEngine;

public class SpriteDirController : MonoBehaviour
{
    [SerializeField] NPC npc;
    [SerializeField] float threshold = 0f;
    [SerializeField] Transform mainTransform;
    [SerializeField] Animator an;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float backAngle = 65f;
    [SerializeField] float sideAngle = 155f;
    private Vector2 animationDir;

    public void LateUpdate()
    {
        an.SetFloat("MoveX", animationDir.x);
        an.SetFloat("MoveY", animationDir.y);

        if (npc.movement.magnitude <= threshold)
        {
            animationDir = new(0, 0);
            return;
        }

        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);
        animationDir = new Vector2(0f, -1f);
        float angle = Mathf.Abs(signedAngle);

        if (angle < backAngle)
        {
            animationDir = new Vector2(0f, -1f);
        }
        else if (angle < sideAngle)
        {
            if (signedAngle < 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            animationDir = new Vector2(1f, 0f);
        }
        else
        {
            animationDir = new Vector2(0f, 1f);
        }
    }
}
