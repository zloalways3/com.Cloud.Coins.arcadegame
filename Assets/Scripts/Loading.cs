using UnityEngine;

public class Loading : MonoBehaviour
{
    public GameObject Loadingscene; // Объект, который нужно скрыть

    void Start()
    {
        // Проверяем, был ли объект уже показан
        if (!PlayerPrefs.HasKey("LoadingsceneShown"))
        {
            if (Loadingscene != null)
            {
                Loadingscene.SetActive(true); // Делаю объект видимым
                Invoke("HideObject", 3f); // Через сколько закроется = 3 секунд
            }

            // Сохраняем информацию о том, что объект был показан
            PlayerPrefs.SetInt("LoadingsceneShown", 1);
        }
        else
        {
            // Если объект уже был показан, скрываем его сразу, если он видим
            if (Loadingscene != null && Loadingscene.activeSelf)
            {
                Loadingscene.SetActive(false);
            }
        }
    }

    void HideObject()
    {
        if (Loadingscene != null)
        {
            Loadingscene.SetActive(false); // Скрываю объект
        }
    }
}