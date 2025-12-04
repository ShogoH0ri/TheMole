using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// BGM管理クラス
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;　　// シングルトン

    [SerializeField]
    private MusicLibrary musicLibrary;　　// 曲データライブラリ

    [SerializeField]
    private AudioSource musicSource;　　// BGM再生用AudioSource

    [SerializeField]
    private AudioMixerGroup MusicGroup;　　// オーディオミキサーグループ

    private void Awake()
    {
        musicSource.gameObject.SetActive(true);　　 // AudioSource を有効化

        if (Instance != null)　　// シングルトンのチェック
        {
            Destroy(gameObject);　　// 既に存在する場合は破棄
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);　　// シーンを跨いで保持
        }
    }

    // 曲を再生（クロスフェード
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    // 曲をクロスフェードで切り替えるコルーチン
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;

        // フェードアウト
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;　　// 曲の切り替え
        musicSource.Play();

        percent = 0;

        // フェードイン
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
}
