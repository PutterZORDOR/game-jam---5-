using System;
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
    public TextMeshProUGUI Type_Skill_text;
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
            Buy();
        }
    }

    public void Buy()
    {
        if (CoinManager.instance.Coins >= Price)
        {
            CoinManager.instance.SpendCoins(Price);
            CanBuy = false;
            CanBuy = true;
            //anim เปิดกล่อง เล่นฟังชั่น Showitem
        }
        else
        {
            //เล่นเสียง
        }
    }

    public void Close_Random_Pannel()
    {
        Random_Pannel.SetActive(true);
    }
    public void Collect()
    {
        if(item.Type == Type_Skill.Active)
        {
            PlayerManager.instance.AddSkill(item);
            lootTable.RemoveItem(item);
        }
        else
        {
            if(item.Ability == Skill_Ability.Double_Jump)
            {
                lootTable.RemoveItem(item);
                PlayerManager.instance.Ability_DoubleJump = true;
            }
            else if(item.Ability == Skill_Ability.Increase_Hp)
            {
                PlayerManager.instance.IncreaseMaxHealth(1);
            }
            else if (item.Ability == Skill_Ability.Increase_Dmg)
            {
                PlayerManager.instance.damgeMulti ++ ;
            }
            else if (item.Ability == Skill_Ability.Increase_Speed) 
            {
                PlayerManager.instance .IncreaseSpeed (0.5f);
            }
        }
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
        Type_Skill_text.text = "";
        Description.text = "";
        Show_Item_Pannel.SetActive(false);
        for (int i = 0; i < text_skill.Count; i++)
        {
            text_skill[i].SetActive(true);
        }
        CloseBox();
    }
    void CloseBox()
    {
        anim.Play("Chest_Idel");
        CanBuy = true;
    }
    public void ShowItem()
    {
        for (int i = 0; i < text_skill.Count; i++)
        {
            text_skill[i].SetActive(false);
        }
        item = lootTable.GetRandom();
        skill_Sprite.sprite = item.sprite;
        skill_name.text = $": {item.skillname} :";
        Type_Skill_text.text = item.Type.ToString();
        Description.text = item.Description;
        Show_Item_Pannel.SetActive(true);
    }
}
