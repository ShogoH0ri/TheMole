using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;

    [SerializeField]
    private AudioSource sfx2DSource;

    [SerializeField]
    private AudioSource sfx3DSource;

    [SerializeField] 
    private AudioMixerGroup sfxGroup;

    private void Awake()
    {
        sfx2DSource.gameObject.SetActive(true);

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
            sfx3DSource.transform.position = pos;
            sfx3DSource.spatialBlend = 1.0f;
            sfx3DSource.PlayOneShot(clip);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip == null) return;

        GameObject go = new GameObject("TempSFX2D");
        AudioSource src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = sfxGroup;
        src.PlayOneShot(clip);
        Destroy(go, clip.length + 0.1f);
    }
}
