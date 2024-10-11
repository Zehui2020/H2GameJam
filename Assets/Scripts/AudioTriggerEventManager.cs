using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerEventManager : MonoBehaviour
{
    [SerializeField] string soundName;

    public void PlayAudio()
    {
        AudioManager.Instance.PlayOneShot(soundName);
    }
}
