using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeChangeButton : MonoBehaviour
{
    [SerializeField] private Sprite[] _onOffSprites = new Sprite[2];
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _audioGroupName;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(SwitchButtonState);
    }

    private void SwitchButtonState()
    {
        _audioMixer.SetFloat($"Volume_{_audioGroupName}", _image.sprite == _onOffSprites[0] ? -80f : 0f);
        _image.sprite = _onOffSprites[_image.sprite == _onOffSprites[0] ? 1 : 0];
    }
}