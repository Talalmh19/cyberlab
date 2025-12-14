using System.Collections.Generic;
using UnityEngine;

public class AlphaUpdate : MonoBehaviour
{
    #region Singleton
    private const string GameObjectName = "Alpha Update";
    private static AlphaUpdate _instance;
    public static AlphaUpdate Instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    GameObject obj = new(GameObjectName);
                    _instance = obj.AddComponent<AlphaUpdate>();
                    DontDestroyOnLoad(obj);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }

            return _instance;
        }
    }
    #endregion

    #region Fields
    private readonly List<IUpdateObserver> updateObservers = new();
    private readonly List<IFixedUpdateObserver> fixedUpdateObservers = new();
    private readonly List<ILateUpdateObserver> lateUpdateObservers = new();

    private readonly object updateLock = new();
    private readonly object fixedUpdateLock = new();
    private readonly object lateUpdateLock = new();
    #endregion

    #region Register Methods
    public void RegisterObserver(IUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (updateLock)
        {
            if (!updateObservers.Contains(observer))
            {
                updateObservers.Add(observer);
            }
        }
    }

    public void RegisterObserver(IFixedUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (fixedUpdateLock)
        {
            if (!fixedUpdateObservers.Contains(observer))
            {
                fixedUpdateObservers.Add(observer);
            }
        }
    }

    public void RegisterObserver(ILateUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (lateUpdateLock)
        {
            if (!lateUpdateObservers.Contains(observer))
            {
                lateUpdateObservers.Add(observer);
            }
        }
    }
    #endregion

    #region Unregister Methods
    public void UnregisterObserver(IUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (updateLock)
        {
            if (updateObservers.Contains(observer))
            {
                _ = updateObservers.Remove(observer);
            }
        }
    }

    public void UnregisterObserver(IFixedUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (fixedUpdateLock)
        {
            if (fixedUpdateObservers.Contains(observer))
            {
                _ = fixedUpdateObservers.Remove(observer);
            }
        }
    }

    public void UnregisterObserver(ILateUpdateObserver observer)
    {
        if (observer == null) { return; }

        lock (lateUpdateLock)
        {
            if (lateUpdateObservers.Contains(observer))
            {
                _ = lateUpdateObservers.Remove(observer);
            }
        }
    }
    #endregion

    #region Unity Methods
    private void Update()
    {
        List<IUpdateObserver> observersCopy;

        lock (updateLock)
        {
            if (updateObservers.Count == 0) { return; }

            observersCopy = new(updateObservers);
        }

        foreach (var observer in observersCopy)
        {
            observer.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        List<IFixedUpdateObserver> observersCopy;

        lock (fixedUpdateLock)
        {
            if (fixedUpdateObservers.Count == 0) { return; }

            observersCopy = new(fixedUpdateObservers);
        }

        foreach (var observer in observersCopy)
        {
            observer.OnFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        List<ILateUpdateObserver> observersCopy;

        lock (lateUpdateLock)
        {
            if (lateUpdateObservers.Count == 0) { return; }

            observersCopy = new(lateUpdateObservers);
        }

        foreach (var observer in observersCopy)
        {
            observer.OnLateUpdate();
        }
    }
    #endregion

    #region Private Methods
    private AlphaUpdate() { }
    #endregion
}

#region Interfaces
public interface IUpdateObserver
{
    void OnUpdate();
}

public interface IFixedUpdateObserver
{
    void OnFixedUpdate();
}

public interface ILateUpdateObserver
{
    void OnLateUpdate();
}
#endregion

//implementations
public class PlayerController : MonoBehaviour, IUpdateObserver, IFixedUpdateObserver, ILateUpdateObserver
{
    void OnEnable()
    {
        AlphaUpdate.Instance.RegisterObserver((IUpdateObserver)this);
        AlphaUpdate.Instance.RegisterObserver((IFixedUpdateObserver)this);
        AlphaUpdate.Instance.RegisterObserver((ILateUpdateObserver)this);
    }

    void OnDisable()
    {
        AlphaUpdate.Instance.UnregisterObserver((IUpdateObserver)this);
        AlphaUpdate.Instance.UnregisterObserver((IFixedUpdateObserver)this);
        AlphaUpdate.Instance.UnregisterObserver((ILateUpdateObserver)this);
    }

    bool updateDone;
    public void OnUpdate()
    {
        if (updateDone) return; updateDone = true;
        Debug.Log("PlayerController Update");
    }

    bool fixedDone;
    public void OnFixedUpdate()
    {
        if (fixedDone) return; fixedDone = true;
        Debug.Log("PlayerController FixedUpdate");
    }

    bool lateDone;
    public void OnLateUpdate()
    {
        if (lateDone) return; lateDone = true;
        Debug.Log("PlayerController LateUpdate");
    }
}
