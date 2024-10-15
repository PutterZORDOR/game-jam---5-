using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class All_Debuff
{
    public string Debuff_name;
    public Sprite sprite;
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private Coroutine debuffCoroutine1;
    private Coroutine debuffCoroutine2;
    public int Health;
    private PlayerMovement movement;

    [Header("Start Stat")]
    public int MaxHealth;

    [Header("UI Stat Bar")]
    public Image HpBar;

    [Header("Text Stat")]
    public TextMeshProUGUI textHp;

    [Header("Invulnerability")]
    public float invulnerabilityDuration;
    private bool isInvulnerable = false;

    [Header("UI GameOver")]
    public GameObject UI_GameOver;

    [Header("Player TakeDamge")]
    private SpriteRenderer spriteRenderer;
    public float blinkTime;
    public Color blinkColor;
    public Color ghostColor;
    private GameObject player;

    [Header("Bleed delay")]
    [SerializeField] float delayBleeding;
    public bool decreaseBleeding;

    [Header("Boost Damge")]
    public int damgeMulti;

    [Header("List My Skill")]
    public All_Skill[] skills = new All_Skill[3];

    [Header("Skill Player")]
    public Image skill_1;
    public Image skill_2;
    public Image skill_3;

    [Header("Debuff List")]
    public List<All_Debuff> Debuffs = new List<All_Debuff>();
    private Dictionary<string, Sprite> debuff_storage = new Dictionary<string, Sprite>();

    [Header("Debuff Slot")]
    public Image debuff_1;
    public Image debuff_2;
    public TextMeshProUGUI text_debuff_1;
    public TextMeshProUGUI text_debuff_2;

    public bool Ability_DoubleJump;

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

        foreach (var Debuff in Debuffs)
        {
            debuff_storage[Debuff.Debuff_name] = Debuff.sprite;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        movement = player.GetComponent<PlayerMovement>();
        InitializeStats();

        skill_1.gameObject.SetActive(false);
        skill_2.gameObject.SetActive(false);
        skill_3.gameObject.SetActive(false);
        debuff_1.gameObject.SetActive(false);
        debuff_2.gameObject.SetActive(false);
        text_debuff_1.gameObject.SetActive(false);
        text_debuff_2.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2);
        }
    }

    private void InitializeStats()
    {
        Health = MaxHealth;
        UpdateUIHp();
    }
    public void IncreaseSpeed(float speed)
    {
        movement.speed = movement.speed * speed;
        movement.jumpingPower = movement.jumpingPower * speed;
    }

    public void UseSkill(int slot)
    {
        if (skills[slot] != null)
        {
            
        }
        else
        {
            Debug.Log($"No skill in slot {slot + 1}");
        }
    }

    public void TakeDamgeAll(int damage)
    {
        if (isInvulnerable) return;

        TakeDamageHp(damage);
        StartCoroutine(InvulnerabilityTimer());
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(blinkTime);
        spriteRenderer.color = ghostColor;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        spriteRenderer.color = Color.white;
    }

    public void TakeDamageHp(int damage)
    {
        Health -= damage;
        Health = Mathf.Max(Health, 0);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();

        if (Health <= 0)
        {
            Die();
        }
    }

    public void DecreaseMaxHealth(int amount)
    {
        MaxHealth -= amount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
    }

    public void Heal(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, MaxHealth);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
    }

    private void Die()
    {
        Time.timeScale = 0f;
        UI_GameOver.SetActive(true);
    }

    private Coroutine bleedCoroutine;

    public void StartBleeding(int totalDamage, int timer)
    {
        if (bleedCoroutine != null)
        {
            StopCoroutine(bleedCoroutine);
        }

        bleedCoroutine = StartCoroutine(Bleed(totalDamage, timer));
    }

    private IEnumerator Bleed(float totalDamage, int timer)
    {
        float remainingDamage = totalDamage;
        if (decreaseBleeding)
        {
            remainingDamage = remainingDamage * 0.5f;
        }
        delayBleeding = timer * remainingDamage;

        while (remainingDamage > 0)
        {
            yield return new WaitForSeconds(timer);

            TakeDamgeAll(1);
            remainingDamage--;
        }

        bleedCoroutine = null;
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        Health = Mathf.Min(Health, MaxHealth);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
    }

    private void UpdateUIHp()
    {
        textHp.text = $"{Health}/{MaxHealth}";
    }

    public void AddSkill(All_Skill newSkill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = newSkill;
                UpdateSkillUI(i);
                return;
            }
        }
    }

    private void UpdateSkillUI(int slot)
    {
        if (skills[slot] != null)
        {
            if (slot == 0)
            {
                skill_1.sprite = skills[slot].sprite;
                skill_1.gameObject.SetActive(true);
            }
            else if (slot == 1)
            {
                skill_2.sprite = skills[slot].sprite;
                skill_2.gameObject.SetActive(true);
            }
            else if (slot == 2)
            {
                skill_3.sprite = skills[slot].sprite;
                skill_3.gameObject.SetActive(true);
            }
        }
        else
        {
            if (slot == 0)
            {
                skill_1.gameObject.SetActive(false);
            }
            else if (slot == 1)
            {
                skill_2.gameObject.SetActive(false);
            }
            else if (slot == 2)
            {
                skill_3.gameObject.SetActive(false);
            }
        }
    }
}
