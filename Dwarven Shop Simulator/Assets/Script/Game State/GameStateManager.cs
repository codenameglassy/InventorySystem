using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Normal;

    private readonly List<IGameStateObserver> observers = new List<IGameStateObserver>();
    private int openInventoryCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Observer registration
    public void Register(IGameStateObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void Unregister(IGameStateObserver observer)
    {
        observers.Remove(observer);
    }

    // Called by inventory panels when opened/closed
    public void OnInventoryOpened()
    {
        openInventoryCount++;
        TransitionTo(GameState.Inventory);
    }

    public void OnInventoryClosed()
    {
        openInventoryCount = Mathf.Max(0, openInventoryCount - 1);
        if (openInventoryCount == 0)
            TransitionTo(GameState.Normal);
    }

    private void TransitionTo(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
            observer.OnStateChanged(CurrentState);
    }
}