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
            bgm.SetVolume(bgmVolume);
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