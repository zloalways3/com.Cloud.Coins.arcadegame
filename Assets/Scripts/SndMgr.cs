using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SndMgr : MonoBehaviour
{
    [SerializeField] Image _sndOnImg; // Иконка для звука включен
    [SerializeField] Image _sndOffImg; // Иконка для звука выключен
    private bool _isMuted = false; // Флаг, указывающий, выключен ли звук

    void Start()
    {
        if (!PlayerPrefs.HasKey("sndMuted"))
        {
            PlayerPrefs.SetInt("sndMuted", 0); // Устанавливаю значение по умолчанию для звука
            _LoadSettings(); // Загружаю настройки
        }
        _UpdateIcons(); // Обновляю иконки в зависимости от состояния звука
        AudioListener.pause = _isMuted; // Устанавливаю состояние звука на основе флага
    }

    public void ToggleSound()
    {
        if (_isMuted)
        {
            _isMuted = false; // Включаю звук
            AudioListener.pause = false;
        }
        else
        {
            _isMuted = true; // Выключаю звук
            AudioListener.pause = true;
        }
        _SaveSettings(); // Сохраняю настройки
        _UpdateIcons(); // Обновляю иконки
    }

    private void _UpdateIcons()
    {
        _sndOnImg.enabled = !_isMuted; // Показываю иконку для звука включен, если звук не выключен
        _sndOffImg.enabled = _isMuted; // Показываю иконку для звука выключен, если звук выключен
    }

    private void _LoadSettings()
    {
        _isMuted = PlayerPrefs.GetInt("sndMuted") == 1; // Загружаю состояние звука из PlayerPrefs
    }

    private void _SaveSettings()
    {
        PlayerPrefs.SetInt("sndMuted", _isMuted ? 1 : 0); // Сохраняю состояние звука в PlayerPrefs
    }
}