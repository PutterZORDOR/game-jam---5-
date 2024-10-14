using TMPro;
using UnityEngine;
public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int Coins; 
    public int StartCoin;
    public int SumCoin;

    [Header("Coin Text")]
    public TextMeshProUGUI coinText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitializeStats();
        UpdateCoinUI();
    }
    private void InitializeStats()
    {
        Coins = StartCoin;
    }
    public void AddCoins(int amount)
    {
        Coins += amount;
        SumCoin += amount;
        UpdateCoinUI();
    }
    public void SpendCoins(int amount)
    {
        Coins -= amount;
        UpdateCoinUI();

    }
    public void UpdateCoinUI()
    {
        coinText.text = $"<sprite name=\"Coin\"> {Coins}";
    }
}
