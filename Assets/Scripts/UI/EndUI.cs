using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    public TMP_Text candyText;
    [TextArea(5, 20)]
    public string message;

    public void UpdateUI()
    {
        candyText.text = string.Format(message, GameManager.active.candy);
    }
}