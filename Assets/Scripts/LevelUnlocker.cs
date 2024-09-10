using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    public Button[] levelButtons;  // Кнопки для уровней в меню
    
    private void Start()
    {
        // Получаю текущий прогресс (какой уровень разблокирован)
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Разблокирую доступные уровни
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                levelButtons[i].interactable = false;  // Блокирую кнопки для уровней, которые ещё не разблокированы
            }
        }
    }

    // Метод для загрузки уровня
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);  // Загружаю уровень по индексу
    }

    // Метод для завершения уровня и разблокировки следующего
    public void CompleteLevel(int completedLevel)
    {
        int currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        
        if (completedLevel >= currentUnlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", completedLevel + 1);  // Разблокирую следующий уровень
        }
    }

    // Загрузить следующий уровень
    public void LoadNextLevel()
    {
        // Получаю индекс текущей сцены
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current Level: " + currentLevelIndex);  // Лог для проверки текущего уровня
        
        // Загружаю следующий уровень (индекс + 1)
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

    // Переиграть текущий уровень
    public void RestartLevel()
    {
        // Получаю индекс текущей сцены и загружаю её снова
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    // Загрузить сцену меню
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");  
    }

    // Этот метод можно вызывать, чтобы закрыть игру
    public void Quit()
    {
        #if UNITY_EDITOR
        // Закрывает игру в редакторе
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Закрывает игру в сборке
        Application.Quit();
        #endif
    }
}
