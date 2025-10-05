using UnityEngine;

public class Singleton<TInstance> : MonoBehaviour where TInstance : Singleton<TInstance> 
{
    public static TInstance Instance;

    public virtual void Awake()
    {
        if (!Instance)
            Instance = this as TInstance;
        else
            Destroy(transform.gameObject);
    }
}