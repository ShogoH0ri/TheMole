using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private string GameOverScreen;

    private float health;
    private float lerpTimer;
    public Image healoverlay;
    [Header("Health Bar")]
    [SerializeField] private float maxHealth;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]

    public Image overlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        healoverlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    void Update()
    {
        GameOver();
        health = Mathf.Clamp(health,0,maxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r,overlay.color.g,overlay.color.b,tempAlpha);
            }

        }

        if (healoverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = healoverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                healoverlay.color = new Color(healoverlay.color.r, healoverlay.color.g, healoverlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI ()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;

            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;

            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        SoundManager.Instance.PlaySound3D("DamagedPlayer", transform.position);
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        SoundManager.Instance.PlaySound3D("HealedPlayer", transform.position);
        health += healAmount;
        lerpTimer = 0f;
        durationTimer = 0;
        healoverlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void GameOver()
    {
        if (health <= 0f)
        {
            SceneManager.LoadScene(GameOverScreen);
        }
    }
}
