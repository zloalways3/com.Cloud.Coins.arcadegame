using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BckgrndSnd : MonoBehaviour
{
    private static BckgrndSnd _bckgrndSndInstance; // Синглтон для фонового звука

    void Awake()
    {
        if (_bckgrndSndInstance == null)
        {
            _bckgrndSndInstance = this; // Устанавливаю текущий объект как единственный экземпляр
            DontDestroyOnLoad(_bckgrndSndInstance); // Не уничтожать объект при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject); // Удаляю объект, если экземпляр уже существует, чтобы избежать дублирования
        }
    }
}