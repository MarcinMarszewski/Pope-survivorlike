using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float dashTime;
    [SerializeField]
    float dashCooldown;

    [SerializeField]
    float killBoostTime;
    [SerializeField]
    public float killBoostMultiplier;
    
    [SerializeField]
    float maxMana;
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float levelOneXp;

    [SerializeField]
    float manaReplenishRate;
    [SerializeField]
    float xpLevelMultiplier;

    [SerializeField]
    float invincibilityTime;
    [SerializeField]
    float damageIndicationTime;
    [SerializeField]
    float spellDelayTime;

    [SerializeField]
    GameControllerScript gameController;

    [SerializeField]
    GameObject damageText;
    [SerializeField]
    List<GameObject> spells;

    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform spellStart;

    [SerializeField]
    Slider manaBar;
    [SerializeField]
    Slider experienceBar;
    [SerializeField]
    Slider healthBar;

    [SerializeField]
    Canvas menu;

    public int skillPoints = 0;

    int currentChosenSpellPos = 0;
    float lastSpellCast = 0;
    float lastDamageTaken = 0;
    float lastDamageIndication = -1;

    float currentKillBoost = 0;
    public float CurrentKillBoost { get { return currentKillBoost; } }

    float lastDash = -1;

    bool isDashing = false;

    System.Random rnd = new System.Random();
    public List<GameObject> Spells { get { return spells; } }
    public int CurrentChosenSpellPos { get { return currentChosenSpellPos; } }

    public SkillsScript skills;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        manaBar.maxValue = maxMana;
        manaBar.value = maxMana;

        experienceBar.maxValue = levelOneXp;

        skills = new SkillsScript();
    }

    void Update()
    {
        int verticalInput = (int)Input.GetAxisRaw("Vertical");
        int horizontalInput = (int)Input.GetAxisRaw("Horizontal");

        if (!isDashing)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput, verticalInput).normalized * speed;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(Input.mousePosition.y - objectPos.y, Input.mousePosition.x - objectPos.x) * Mathf.Rad2Deg + 180;

            animator.SetBool("IsMoving", horizontalInput != 0 || verticalInput != 0);
            animator.SetBool("IsFacingDown 0", angle >= 45 && angle < 135);
            animator.SetBool("IsFacingUp", angle >= 225 && angle < 315);
            animator.SetBool("IsFacingLeft", angle >= 315 || angle < 45);
            animator.SetBool("IsFacingRight", angle >= 135 && angle < 225);
        }
        else if (lastDash + dashTime <= Time.time)
        {
            isDashing = false;
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }


        if (lastDamageIndication + damageIndicationTime > Time.time) GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.white;

        if (!gameController.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Q)) currentChosenSpellPos = Mathf.Clamp(currentChosenSpellPos - 1, 0, spells.Count - 1);
            if (Input.GetKeyDown(KeyCode.E)) currentChosenSpellPos = Mathf.Clamp(currentChosenSpellPos + 1, 0, spells.Count - 1);
            if (Input.GetAxisRaw("Fire1") != 0 && lastSpellCast + spellDelayTime <= Time.time) TrySpellCast();
            if (Input.GetKeyDown(KeyCode.LeftShift) && skills.IsSkillUnlocked(Skills.Dash) && lastDash + dashCooldown <= Time.time)
            {
                lastDash = Time.time;
                isDashing = true;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = transform.position.z;
                GetComponent<Rigidbody2D>().velocity = (mousePos-transform.position).normalized * 15;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
        }

        AddMana(manaReplenishRate * Time.deltaTime);
        killBoostTime = Mathf.Max(killBoostTime - Time.deltaTime, 0);
    }

    private void TrySpellCast()
    {
        if(manaBar.value>=spells[currentChosenSpellPos].GetComponent<BasicSpellScript>().manaCost/*&&skills.IsSkillUnlocked(spells[currentChosenSpellPos].GetComponent<BasicSpellScript>().skillNeeded)*/)
        {
            Instantiate<GameObject>(spells[currentChosenSpellPos], transform.position, Quaternion.identity);
            manaBar.value -= spells[currentChosenSpellPos].GetComponent<BasicSpellScript>().manaCost;
            lastSpellCast = Time.time;
        }
    }

    public void AddHealth(float health)
    {
        GameObject obj = Instantiate<GameObject>(damageText, transform);
        obj.GetComponentInChildren<UnityEngine.UI.Text>().text = health.ToString();
        obj.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.green;
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(rnd.Next(-10, 10) / 10.0f, 3.5f);
        Destroy(obj, 0.5f);

        healthBar.value += health;
    }
    public void AddMana(float mana)
    {
        manaBar.value += mana;
    }
    public void AddExperience(float exp)
    {
        if(experienceBar.value + exp>= experienceBar.maxValue)
        {
            experienceBar.value = experienceBar.value + exp - experienceBar.maxValue;
            LevelUp();
            return;
        }
        experienceBar.value += exp; 
    }   
    public void AddManaRegen(float regen)
    {
        manaReplenishRate += regen;
    }
    public void AddSpeed(float speed)
    {
        this.speed += speed;
    }
    public void AddMaxHealth(float health)
    {
        healthBar.maxValue += health;
    }
    public void AddMaxMana(float mana)
    {
        manaBar.maxValue += mana;
    }
    private void LevelUp()
    {
        skillPoints++;
        experienceBar.value *= xpLevelMultiplier;
        gameController.UpdateSkillPointIndicator();
    }

    public void KillBoost()
    {
        currentKillBoost += killBoostTime;
    }
    public void Damage(float damage)
    {
        if (lastDamageTaken + invincibilityTime < Time.time&&!isDashing)
        {
            GameObject obj = Instantiate<GameObject>(damageText, transform);
            obj.GetComponentInChildren<UnityEngine.UI.Text>().text = damage.ToString();
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(rnd.Next(-10, 10) / 10.0f, 3.5f);
            Destroy(obj, 0.5f);

            healthBar.value -= damage;
            if (healthBar.value <= 0) UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Plains");

            lastDamageTaken = Time.time;
            lastDamageIndication = Time.time;
        }
    }

    public void ChangeDashTime(float time)
    {
        dashTime += time;
    }
    public void ChangeDashCooldown(float time)
    {
        dashCooldown += time;
    }

}
