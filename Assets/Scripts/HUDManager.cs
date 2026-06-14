using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("Health")]
    public Image[] hearts;

    [Header("Coins")]
    public TMP_Text coinText;

    private int coins;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateCoins();
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }

    public void AddCoin()
    {
        coins++;
        UpdateCoins();
    }

    void UpdateCoins()
    {
        coinText.text = "x " + coins;
    }
}