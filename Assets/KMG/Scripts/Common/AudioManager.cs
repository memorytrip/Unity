using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance;

        private List<AudioSource> sfxPlayers;
        private const int baseSfxPlayersCount = 8;

        private BGM bgm;
        private AudioSource bgmPlayer;
        
        [Header("오디오 볼륨")]
        [Range(0f, 1f)] public float ambienceVolume = 1f;
        [Range(0f, 1f)] public float bgmVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;
        [Range(0f, 1f)] public float systemVolume = 1f;

        [Header("로컬로 저장되는 볼륨값")]
        private float _savedAmbienceVolume = 1f;
        private float _savedBgmVolume = 1f;
        private float _savedSfxVolume = 1f;
        private float _savedSystemVolume = 1f;
        
        public event Action PlayVideo;

        public void OnVideoPlayed()
        {
            PlayVideo?.Invoke();
        }

        public event Action StopVideo;

        public void OnVideoStopped()
        {
            StopVideo?.Invoke();
        }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
            
            InitPlayers();
        }

        private void InitPlayers()
        {
            sfxPlayers = new List<AudioSource>(baseSfxPlayersCount);
            for (int i = 0; i < baseSfxPlayersCount; i++)
            {
                sfxPlayers.Add(gameObject.AddComponent<AudioSource>());
            }
            bgmPlayer = gameObject.AddComponent<AudioSource>();
            bgmPlayer.loop = true;
        }

        public void PlayBGM(AudioClip audioClip)
        {
            
        }

        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        public void PlaySFX(AudioClip audioClip)
        {
            AudioSource audioSource = sfxPlayers.Find((e) => !e.isPlaying);
            if (audioSource == null)
            {
                Debug.LogWarning("Cannot find empty sfxPlayer");
                return;
            }
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void SetAmbienceVolume(float volume)
        {
            ambienceVolume = volume;
        }

        public void SetBgmVolume(float volume)
        {
            bgmVolume = volume;
            bgm.SetVolume(volume);
        }

        public void SetSfxVolume(float volume)
        {
            sfxVolume = volume;
        }

        public void SetSystemVolume(float volume)
        {
            systemVolume = volume;
        }
    }
}