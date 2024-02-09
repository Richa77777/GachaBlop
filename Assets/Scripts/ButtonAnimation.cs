using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioMixer _mixer;

    private Animator _animator;
    private AudioMixerGroup _mixerGroup;
    private AudioSource _audioSource;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _mixerGroup = _mixer.FindMatchingGroups("Sounds")[0];
        _audioSource.outputAudioMixerGroup = _mixerGroup;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_animator.GetBool("isClicked") == false)
        {
            PlaySound();
            _animator.SetBool("isClicked", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_animator.GetBool("isClicked") == true)
        {
            _animator.SetBool("isClicked", false);
        }
    }

    private void PlaySound()
    {
        _audioSource.PlayOneShot(_clickSound);
    }
}