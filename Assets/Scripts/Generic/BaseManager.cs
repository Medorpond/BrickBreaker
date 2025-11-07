using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T: BaseManager<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError("No Manager Found on Scene");
                }
            }

            return _instance;
        }
    }

    [SerializeField] protected bool dontDestroyOnLoad = false;


    protected virtual void Awake()
    {
        InitInstance();
    }

    protected virtual void InitInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (!dontDestroyOnLoad && _instance == this) _instance = null;
    }
}
