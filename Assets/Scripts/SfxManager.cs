using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : MonoBehaviour
{
    [Serializable]
    class AudioClipWithTitle {
        public string title;
        public AudioClip clip;
    }

    [SerializeField] List<AudioClipWithTitle> audioClips;

    public void PlaySoundEffect(string title)
    {
        AudioClipWithTitle curClip = audioClips.Find(x => x.title == title);
        if (curClip != null) GetComponent<AudioSource>().PlayOneShot(curClip.clip);
        else Debug.LogError($"Sound clip with title \"{title}\" not found.");
    }
}
