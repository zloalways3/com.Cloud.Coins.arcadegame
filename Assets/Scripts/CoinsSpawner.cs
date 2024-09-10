using UnityEngine;
using UnityEngine.UI; // Для работы с UI
using TMPro; // Для работы с TextMeshPro
using System.Collections.Generic; // Для использования списка

public class CoinsSpawner : MonoBehaviour
{
    public GameObject obj; // Префаб UI-объекта
    public RectTransform canvasRect; // RectTransform канваса
    public float SpawnDelay;
    private float nextSpawn = 0.0f;

    public AudioClip coinSound; // Аудио клип для звука монеты

    private AudioSource audioSource; // Источник звука

    private GameManager gameManager; // Ссылка на GameManager

    private List<GameObject> spawnedCoins = new List<GameObject>(); // Список для хранения спавненных монет

    public float spawnYOffset = 100f; // Отступ от верхней границы канваса

    void Start()
    {
        // Проверяю, что все необходимые компоненты назначены
        if (obj == null)
        {
            Debug.LogError("UI-объект не назначен!");
        }
        if (canvasRect == null)
        {
            Debug.LogError("RectTransform канваса не назначен!");
        }
        if (coinSound == null)
        {
            Debug.LogError("Аудио клип для звука монеты не назначен!");
        }

        gameManager = FindObjectOfType<GameManager>(); // Ищу GameManager в сцене

        audioSource = GetComponent<AudioSource>(); // Получаю компонент AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Добавляю AudioSource, если его нет
        }
    }

    void Update()
    {
        if (gameManager == null || gameManager.IsGameEnded)
            return; // Если игра завершена, ничего не делаю

        // Если время и спавн разрешены
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + SpawnDelay;
            float randomX = Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2);
            Vector2 whereToSpawn = new Vector2(randomX, canvasRect.rect.height / 2 - spawnYOffset); // Выставляю позицию немного ниже верхней границы канваса
            SpawnObject(whereToSpawn);
        }
    }

    void SpawnObject(Vector2 position)
    {
        if (obj != null && canvasRect != null && !gameManager.IsGameEnded) // Проверка завершения игры
        {
            // Создаю объект в RectTransform канваса
            GameObject spawnedObj = Instantiate(obj, canvasRect);
            RectTransform rectTransform = spawnedObj.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = position;

                // Добавляю объект в список спавненных объектов
                spawnedCoins.Add(spawnedObj);

                // Проверяю наличие коллайдера и настраиваю его как триггер
                Collider2D collider = spawnedObj.GetComponent<Collider2D>();
                if (collider == null)
                {
                    collider = spawnedObj.AddComponent<BoxCollider2D>(); // Или другой тип коллайдера
                }

                collider.isTrigger = true; // Устанавливаю коллайдер как триггер

                // Добавляю скрипт обработки столкновений
                DestroyOnTouch destroyOnTouch = spawnedObj.GetComponent<DestroyOnTouch>();
                if (destroyOnTouch == null)
                {
                    destroyOnTouch = spawnedObj.AddComponent<DestroyOnTouch>();
                }

                destroyOnTouch.Initialize(); // Инициализирую скрипт
                destroyOnTouch.OnDestroyed += () => 
                {
                    gameManager.UpdateScore(); // Обновляю счет
                    PlayCoinSound(); // Воспроизвожу звук монеты
                }; 
            }
            else
            {
                Debug.LogError("Отсутствует RectTransform в префабе!");
            }
        }
    }

    void PlayCoinSound()
    {
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound); // Воспроизведение звука
        }
    }

    public class DestroyOnTouch : MonoBehaviour
    {
        public delegate void DestroyedEventHandler();
        public event DestroyedEventHandler OnDestroyed;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) // Предполагается, что у игрока установлен тег "Player"
            {
                Destroy(gameObject); // Удаляю объект при столкновении
                OnDestroyed?.Invoke(); // Вызываю событие при уничтожении
            }
        }

        public void Initialize()
        {
            // Настройка коллайдера для триггера (если необходимо)
            var collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true; // Устанавливаю коллайдер как триггер
            }
        }
    }

    public void DestroyAllCoins()
    {
        // Удаляю все спавненные монеты
        foreach (GameObject coin in spawnedCoins)
        {
            if (coin != null)
            {
                Destroy(coin);
            }
        }

        spawnedCoins.Clear(); // Очищаю список
    }
}
