using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
  [SerializeField] AudioMixer masterVolumeMixer;
  [SerializeField] GameObject settingsCanvas;
  [SerializeField] Animator _animator;

  private void Start()
  {
    _animator = GetComponent<Animator>();
  }

  public void SetMasterVolume(float volume)
  {
    masterVolumeMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
  }
  
  
  public void OpenSettingPopup()
  {
    settingsCanvas.SetActive(true);
  }
  public void CloseSettingPopup()
  {
    settingsCanvas.SetActive(false);
  }
  
}
