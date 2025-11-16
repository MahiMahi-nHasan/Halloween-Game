using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public RenderTexture renderTexture;

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit(renderTexture, dest: null);
    }
}
