using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// 効果音管理クラス
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;　　// シングルトン

    [SerializeField]
    private SoundLibrary sfxLibrary;　　// 効果音ライブラリ

    [SerializeField]
    private AudioSource sfx2DSource;　　// 2D再生用 AudioSource

    [SerializeField]
    private AudioSource sfx3DSource;　　// 3D再生用 AudioSource

    [SerializeField]
    private AudioMixerGroup sfxGroup;　　// ミキサーグループ

    private void Awake()
    {
        // AudioSource 有効化
        sfx2DSource.gameObject.SetActive(true);

        // シングルトン処理
        if (Instance != null)
        {
            Destroy(gameObject);　　// 既に存在する場合破棄
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);　　// シーン跨ぎで保持
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)　　// 3D空間で効果音を再生
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);　　// 瞬間再生用

            // sfx3DSourceで再生
            sfx3DSource.transform.position = pos;
            sfx3DSource.spatialBlend = 1.0f;　　// 3D音に設定
            sfx3DSource.PlayOneShot(clip);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)　　// 3D効果音を名前で再生
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)　　// 2D効果音を再生
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip == null) return;

        // 一時オブジェクトを生成して再生後破棄
        GameObject go = new GameObject("TempSFX2D");
        AudioSource src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = sfxGroup;
        src.PlayOneShot(clip);
        Destroy(go, clip.length + 0.1f);　　// 再生後に破棄
    }
}
