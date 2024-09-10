using UnityEngine;
using TMPro; // Для работы с TextMeshPro
using UnityEngine.UI; // Для работы с UI
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText; // TextMeshPro для отображения очков
    public Slider timerSlider; // UI Slider для таймера
    public float timerDuration = 60f; // Продолжительность таймера

    public int targetScore = 1000; // Цель очков

    public GameObject winPanel; // Панель победы
    public GameObject losePanel; // Панель поражения

    public TMP_Text winPanelScoreText; // TextMeshPro для очков на панели победы
    public Slider winPanelTimeSlider; // Slider для времени на панели победы
    public TMP_Text losePanelScoreText; // TextMeshPro для очков на панели поражения
    public Slider losePanelTimeSlider; // Slider для времени на панели поражения

    private int score = 0;
    private float timeRemaining;
    private bool gameEnded = false; // Флаг завершения игры

    // Добавляю ссылку на LevelUnlocker
    public LevelUnlocker levelUnlocker;

    void Start()
    {
        // Инициализирую таймер
        timeRemaining = timerDuration;
        if (timerSlider != null)
        {
            timerSlider.maxValue = timerDuration;
            timerSlider.value = timerDuration;
        }
    }

    void Update()
    {
        if (gameEnded)
            return; // Если игра завершена, ничего не делаю

        // Обновляю таймер
        if (timerSlider != null)
        {
            timeRemaining -= Time.deltaTime;
            timerSlider.value = Mathf.Clamp(timeRemaining, 0, timerDuration);

            // Проверяю окончание времени
            if (timeRemaining <= 0)
            {
                EndGame(false); // Время вышло, игра окончена с поражением
                return;
            }
        }
    }

    public void UpdateScore()
    {
        score += 50; // Изменяю количество очков за монету
        scoreText.text = "Points: " + score + "/" + targetScore;

        // Проверяю достижение цели
        if (score >= targetScore)
        {
            EndGame(true); // Игра окончена с победой
        }
    }

    public void EndGame(bool won)
    {
        gameEnded = true; // Устанавливаю флаг завершения игры

        // Останавливаю таймер и спавнер
        enabled = false; // Отключаю скрипт, чтобы остановить все процессы

        if (won)
        {
            // Показываю панель победы
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            // Обновляю текст и слайдер на панели победы
            if (winPanelScoreText != null)
            {
                winPanelScoreText.text = "Points: " + score;
            }
            if (winPanelTimeSlider != null)
            {
                winPanelTimeSlider.value = timerSlider.value; // Отображаю оставшееся время
                winPanelTimeSlider.maxValue = timerDuration; // Устанавливаю максимальное значение, чтобы соответствовать таймеру
            }

            // Вызываю разблокировку следующего уровня
            if (levelUnlocker != null)
            {
                int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                levelUnlocker.CompleteLevel(currentLevel);
            }
        }
        else
        {
            // Показываю панель поражения
            if (losePanel != null)
            {
                losePanel.SetActive(true);
            }

            // Обновляю текст и слайдер на панели поражения
            if (losePanelScoreText != null)
            {
                losePanelScoreText.text = "Points: " + score;
            }
            if (losePanelTimeSlider != null)
            {
                losePanelTimeSlider.value = 0; // Отображаю, что время вышло
                losePanelTimeSlider.maxValue = timerDuration; // Устанавливаю максимальное значение, чтобы соответствовать таймеру
            }
        }

        // Очищаю монеты через CoinsSpawner
        CoinsSpawner coinsSpawner = FindObjectOfType<CoinsSpawner>();
        if (coinsSpawner != null)
        {
            coinsSpawner.DestroyAllCoins();
        }
    }
    public bool IsGameEnded => gameEnded; // Свойство для проверки состояния игры
    
}
