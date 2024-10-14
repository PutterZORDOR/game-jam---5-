using TMPro;
using UnityEngine;

public class Skill_Trader : MonoBehaviour
{
    public WeightSkill lootTable;

    [Header("Price")]
    public int Price_Life;

    public bool CanBuy = true;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject This_Item;
    public GameObject UI_Buy;
    private Vector3 sizeItem;
    [SerializeField] int current_price;
    [SerializeField] All_Skill item;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("UI Description")]
    public GameObject Description;
    public TextMeshProUGUI textSkill;
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        GetButton = transform.Find("Get_Button").gameObject;
        Description = transform.Find("Description_Skill").gameObject;
        textSkill = Description.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy = transform.Find("UI_Buy").gameObject;
        text = UI_Buy.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        ShowItem();
    }
    void Update()
    {
        if (CanBuy)
        {
            IsPlayerInRange();
        }
    }
    bool IsPlayerInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Found");
                GetButton.SetActive(true);
                Description.SetActive(true);
                UI_Buy.SetActive(true);
                return true;
            }
        }
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        return false;
    }
    public void Buy()
    {
        if (CanBuy && PlayerManager.instance.MaxHealth > current_price)
        {
            PlayerManager.instance.DecreaseMaxHealth(current_price);
            PlayerManager.instance.AddSkill(item.sprite);
            CanBuy = false;
            UI_Buy.SetActive(false);
            Description.SetActive(false);
            GetButton.SetActive(false);
            if (item != null)
            {
                
            }
            This_Item.SetActive(false);
            this.enabled = false;
        }
        else
        {
            Debug.Log("No Money");
        }
    }
    void ShowItem()
    {
        if (item != null)
        {
          
        }
        This_Item = Instantiate(item.gamePrefab, transform);
        sizeItem = item.gamePrefab.transform.localScale;
        This_Item.transform.localScale = sizeItem * 2f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
