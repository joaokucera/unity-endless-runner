using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Sound Manager")]
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        private AudioSource m_musicSource;
        [SerializeField]
        private AudioSource m_sfxSource;
        [SerializeField]
        private AudioClip[] sfxClips;

        private Dictionary<string, AudioClip> m_soundDictionary = new Dictionary<string, AudioClip>();

        void Start()
        {
            CreateSoundDictionary();

            StartCoroutine(PlayMusic());
        }

        public static void PlaySoundEffect(string clipName)
        {
            AudioClip originalClip;

            if (Instance.m_soundDictionary.TryGetValue(clipName, out originalClip))
            {
                Instance.MakeSoundEffect(originalClip);
            }
        }

        private void CreateSoundDictionary()
        {
            for (int i = 0; i < sfxClips.Length; i++)
            {
                m_soundDictionary.Add(sfxClips[i].name, sfxClips[i]);
            }
        }

        private IEnumerator PlayMusic()
        {
            m_musicSource.volume = 0f;
            m_musicSource.loop = true;
            m_musicSource.Play();

            while (m_musicSource.volume < 0.5f)
            {
                m_musicSource.volume += Time.deltaTime;

                yield return null;
            }
        }

        private void MakeSoundEffect(AudioClip originalClip)
        {
            m_sfxSource.PlayOneShot(originalClip);
        }
    }
}