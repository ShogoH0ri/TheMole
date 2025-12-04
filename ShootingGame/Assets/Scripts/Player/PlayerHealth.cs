using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private string GameOverScreen;　　// ゲームオーバー時に読み込むシーン名

    private float health;　　// 現在のHP
    private float lerpTimer;　　// ヘルスバーアニメーション用タイマー
    public Image healoverlay;　　// 回復時オーバーレイ
    [Header("Health Bar")]
    [SerializeField] private float maxHealth;　　// 最大HP
    public float chipSpeed = 2f;　　// 体力バーが追従する速度
    public Image frontHealthBar;　　// 前面の体力バー
    public Image backHealthBar;　　// 背面の体力バー

    [Header("Damage Overlay")]

    public Image overlay;　　// ダメージ時オーバーレイ
    public float duration;　　// オーバーレイの表示時間
    public float fadeSpeed;　　// フェードアウト速度
    private float durationTimer;　　// オーバーレイ表示時間計測用タイマー
    void Start()
    {
        health = maxHealth;

        // オーバーレイ初期透明化
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        healoverlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    void Update()
    {
        GameOver();　　// HP0チェック
        health = Mathf.Clamp(health, 0, maxHealth);　　 // HPを0～maxで制限
        UpdateHealthUI();　　// 体力バー更新

        // ダメージオーバーレイのフェード処理
        if (overlay.color.a > 0)　　// HP30未満はオーバーレイを消さない
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        // 回復オーバーレイのフェード処理
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

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        // ダメージ時
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;

            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        // 回復時
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

    public void TakeDamage(float damage)　　// ダメージを受けたときの処理
    {
        SoundManager.Instance.PlaySound3D("DamagedPlayer", transform.position);
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)　　// 回復処理
    {
        SoundManager.Instance.PlaySound3D("HealedPlayer", transform.position);
        health += healAmount;
        lerpTimer = 0f;
        durationTimer = 0;
        healoverlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void GameOver()　　// HPが0になったらゲームオーバー
    {
        if (health <= 0f)
        {
            SceneManager.LoadScene(GameOverScreen);
        }
    }
}
