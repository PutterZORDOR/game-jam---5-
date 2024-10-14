using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Trader : MonoBehaviour
{
    public WeightSkill lootTable;

    [Header("Price")]
    public int Price;

    [Header("Show Item Pannael")]
    public GameObject Show_Item_Pannel;
    public Image skill_Sprite;
    public TextMeshProUGUI skill_name;
    public TextMeshProUGUI Type_Skill;
    public TextMeshProUGUI Description;

    [Header("Random_Pannel")]
    public GameObject Random_Pannel;

    [Header("Animator")]
    [SerializeField] Animator anim;

    [Header("Text Near Chest")]
    public List<GameObject> text_skill = new List<GameObject>();

    public bool CanBuy = true;
    [SerializeField] All_Skill item;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (CanBuy)
        {
            if (CoinManager.instance.Coins >= Price)
            {
                CoinManager.instance.SpendCoins(Price);
                CanBuy = false;
                for (int i = 0; i < text_skill.Count; i++)
                {
                    text_skill[i].SetActive(false);
                }
                CanBuy = true;
                //anim เปิดกล่อง เล่นฟังชั่น Showitem
            }
            else
            {
                //เล่นเสียง
            }
        }
    }
    public void Close_Random_Pannel()
    {
        Random_Pannel.SetActive(true);
    }
    public void Collect()
    {
        PlayerManager.instance.AddSkill(item);
        lootTable.RemoveItem(item);
        HideItem();
    }
    public void Drop()
    {
        HideItem();
    }
    void HideItem()
    {
        skill_Sprite.sprite = null;
        skill_name.text = "";
        Description.text = "";
        Show_Item_Pannel.SetActive(false);
        //เล่น anim ปิดกล่อง เล่นฟังชั่น CloseBox
    }
    public void CloseBox()
    {
        for (int i = 0; i < text_skill.Count; i++)
        {
            text_skill[i].SetActive(true);
        }
        CanBuy = true;
    }
    void ShowItem()
    {
        Show_Item_Pannel.SetActive(true);
        item = lootTable.GetRandom();
    }
}
