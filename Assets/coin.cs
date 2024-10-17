using UnityEngine;

public class coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            CoinManager.instance.AddCoins(1);
            gameObject.SetActive(false);
        }
    }
}
