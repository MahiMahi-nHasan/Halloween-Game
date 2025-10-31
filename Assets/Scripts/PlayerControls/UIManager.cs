using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI candyUI;

    public Sprite healthIndicator1;
    public Sprite healthIndicator2;
    public Sprite healthIndicator3;

    public Image healthIndicator;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public void UpdateHealth(int hp)
    {
        if (hp == 3)
        {
            healthIndicator.sprite = healthIndicator1;
        }
        else if (hp == 2)
        {
            healthIndicator.sprite = healthIndicator2;
        }
        else
        {
            healthIndicator.sprite = healthIndicator3;
        }

    }
    public void UpdateCandy(int candyVal) 
    {
        candyUI.text = candyVal.ToString();
    }

}
