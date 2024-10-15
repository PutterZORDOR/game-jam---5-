using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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
    private Animator anim;
    public bool IsDie;

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

    [Header("Cooldown Skill")]
    public Image cooldown_1;
    public Image cooldown_2;
    public Image cooldown_3;

    private float[] skillCooldownTimers = new float[3];
    private bool[] skillOnCooldown = new bool[3];

    public bool Ability_DoubleJump;
    public bool Immune;

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
        anim = player.GetComponent<Animator>();
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

        cooldown_1.fillAmount = 0;
        cooldown_2.fillAmount = 0;
        cooldown_3.fillAmount = 0;
    }

    private void Update()
    {
        UpdateCooldowns();

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

    private void UpdateCooldowns()
    {
        for (int i = 0; i < skillCooldownTimers.Length; i++)
        {
            if (skillOnCooldown[i])
            {
                skillCooldownTimers[i] -= Time.deltaTime;

                if (skillCooldownTimers[i] <= 0)
                {
                    skillCooldownTimers[i] = 0;
                    skillOnCooldown[i] = false;
                }
                UpdateCooldownUI(i);
            }
        }
    }

    private void UpdateCooldownUI(int slot)
    {
        float fillAmount = skillCooldownTimers[slot] / skills[slot].cd;
        switch (slot)
        {
            case 0:
                cooldown_1.fillAmount = fillAmount;
                break;
            case 1:
                cooldown_2.fillAmount = fillAmount;
                break;
            case 2:
                cooldown_3.fillAmount = fillAmount;
                break;
        }
    }

    private void CooldownSkill(float cd)
    {
        int skillIndex = Array.IndexOf(skills, skills.FirstOrDefault(s => s.cd == cd));
        if (skillIndex != -1)
        {
            skillOnCooldown[skillIndex] = true;
            skillCooldownTimers[skillIndex] = cd;
        }
    }

    private void InitializeStats()
    {
        Health = MaxHealth;
        UpdateUIHp();
    }
    public void IncreaseSpeed(float speed)
    {
        movement.speed = movement.speed + speed;
        movement.jumpingPower = movement.jumpingPower + speed;
        movement.dashingPower = movement.dashingPower + speed;
    }

    public void UseSkill(int slot)
    {
        bool Checks = false;
        if (skills[slot] != null)
        {
            if (!skillOnCooldown[slot])
            {
                if (skills[slot].Ability == Skill_Ability.Dig)
                {
                    movement.DigDown();
                    Immune =true;
                    Checks = true;
                }
                else if (skills[slot].Ability == Skill_Ability.Dash && !movement.IsGrounded())
                {
                    movement.dashCooldown = skills[slot].cd;
                    movement.Dashing();
                    Checks = true;
                }else if (skills[slot] != null)
                {
                    //เล่นเสียง
                }

                if(Checks)
                {
                    CooldownSkill(skills[slot].cd);
                }
            }
        }
    }

    public void TakeDamgeAll(int damage)
    {
        if (!Immune)
        {
            if (isInvulnerable) return;

            TakeDamageHp(damage);
            StartCoroutine(InvulnerabilityTimer());
        }
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
            IsDie = true;
            player.tag = "Untagged";
            anim.Play("Cat_Die");
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
    public void Die()
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
    public Sprite GetDebuffSprite(string debuffName)
    {
        // ดึง Sprite ของ Debuff จากชื่อ
        if (debuff_storage.TryGetValue(debuffName, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            return null;
        }
    }

    public void ApplyDebuff(string debuffName, int timer)
    {
        Sprite debuffSprite = GetDebuffSprite(debuffName);
        if (debuffSprite != null)
        {
            if (debuff_1.sprite == null || debuff_1.sprite == debuffSprite)
            {
                ApplyDebuffToSlot(debuff_1, text_debuff_1, ref debuffCoroutine1, debuffSprite, debuffName, timer);
            }
            else if (debuff_2.sprite == null || debuff_2.sprite == debuffSprite)
            {
                ApplyDebuffToSlot(debuff_2, text_debuff_2, ref debuffCoroutine2, debuffSprite, debuffName, timer);
            }
        }
    }

    private void ApplyDebuffToSlot(Image debuffIcon, TextMeshProUGUI debuffText, ref Coroutine debuffCoroutine, Sprite debuffSprite, string debuffName, int timer)
    {
        debuffIcon.sprite = debuffSprite;
        debuffIcon.gameObject.SetActive(true);
        debuffText.gameObject.SetActive(true);

        if (debuffCoroutine != null)
        {
            StopCoroutine(debuffCoroutine);
        }
        debuffCoroutine = StartCoroutine(DebuffCountdown(debuffIcon, debuffText, debuffName, timer));
    }

    private IEnumerator DebuffCountdown(Image debuffIcon, TextMeshProUGUI debuffText, string debuffName, int timer)
    {
        float countdownTime = timer;
        debuffText.text = $"{countdownTime.ToString("F0")}s";

        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime--;
            debuffText.text = $"{countdownTime.ToString("F0")}s";
        }

        RemoveDebuff(debuffIcon, debuffText, debuffName);
    }

    private void RemoveDebuff(Image debuffIcon, TextMeshProUGUI debuffText, string debuffName)
    {
        if (debuff_2.sprite != null && debuffIcon == debuff_1)
        {
            debuff_1.sprite = debuff_2.sprite;
            debuff_1.gameObject.SetActive(true);
            text_debuff_1.gameObject.SetActive(true);
            text_debuff_1.text = text_debuff_2.text;

            StopCoroutine(debuffCoroutine2);
            debuffCoroutine1 = StartCoroutine(DebuffCountdown(debuff_1, text_debuff_1, debuffName, GetRemainingDebuffTime(text_debuff_2.text)));

            debuff_2.sprite = null;
            debuff_2.gameObject.SetActive(false);
            text_debuff_2.text = "";
            text_debuff_2.gameObject.SetActive(false);
        }
        else
        {
            debuffIcon.sprite = null;
            debuffIcon.gameObject.SetActive(false);
            debuffText.text = "";
            debuffText.gameObject.SetActive(false);
        }
    }

    private int GetRemainingDebuffTime(string debuffText)
    {
        if (int.TryParse(debuffText.Replace("s", ""), out int remainingTime))
        {
            return remainingTime;
        }
        return 0;
    }
}
