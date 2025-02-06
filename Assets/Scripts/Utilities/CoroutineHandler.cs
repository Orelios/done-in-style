using System.Collections;
using UnityEngine;

public static class CoroutineHandler
{
    private static GameObject _coroutineGameObject;

    static CoroutineHandler()
    {
        _coroutineGameObject = new ();
        Object.DontDestroyOnLoad(_coroutineGameObject);
    }

    public static void StartTimeSlowCoroutine(IEnumerator coroutine)
    {
        _coroutineGameObject.AddComponent<CoroutineRunner>().RunCoroutine(coroutine);
    }

    private class CoroutineRunner : MonoBehaviour
    {
        public void RunCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}
