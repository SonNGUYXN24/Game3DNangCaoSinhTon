using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartusPlayer : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hpSlider;
    public Slider staminaSlider;
    public Slider expSlider;
    public TextMeshProUGUI levelText;

    [Header("Stats")]
    public int level = 1;
    public float currentHP = 100;
    public float maxHP = 100;
    public float currentStamina = 100;
    public float maxStamina = 100;
    public int currentExp = 0;
    public int expToNextLevel = 1000;
    public int maxLevel = 100;

    private float staminaRegenDelayTimer = 0f;
    private float staminaRegenTimer = 0f;
    private float hpRegenTimer = 0f;

    private bool isStaminaDraining = false;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Stamina regeneration delay logic
        if (isStaminaDraining)
        {
            staminaRegenDelayTimer = 0f;
            isStaminaDraining = false;
        }
        else
        {
            staminaRegenDelayTimer += Time.deltaTime;

            if (staminaRegenDelayTimer >= 2f)
            {
                staminaRegenTimer += Time.deltaTime;
                if (staminaRegenTimer >= 0.1f && currentStamina < maxStamina)
                {
                    ChangeStamina(+1);
                    staminaRegenTimer = 0f;
                }
            }
        }

        // HP regen logic
        if (currentHP < maxHP)
        {
            hpRegenTimer += Time.deltaTime;
            if (hpRegenTimer >= 5f)
            {
                ChangeHP(+1);
                hpRegenTimer = 0f;
            }
        }
    }

    public void GainExp(int amount)
    {
        if (level >= maxLevel) return;

        currentExp += amount;

        while (currentExp >= expToNextLevel && level < maxLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        maxHP += 20;
        maxStamina += 20;
        currentHP = maxHP;
        currentStamina = maxStamina;

        expToNextLevel *= 2;

        if (level >= maxLevel)
        {
            currentExp = 0;
            expToNextLevel = 0;
        }

        UpdateUI();
    }

    public void ChangeHP(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        UpdateUI();
    }

    public void ChangeStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);

        // Đánh dấu đang tụt để reset delay hồi
        if (amount < 0)
        {
            isStaminaDraining = true;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;

        expSlider.maxValue = expToNextLevel == 0 ? 1 : expToNextLevel;
        expSlider.value = currentExp;

        levelText.text = level >= maxLevel ? "Level Max" : $"Level {level}";
    }

    public float CurrentStamina => currentStamina;
    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public float MaxStamina => maxStamina;
}
