using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnEnemyKilled;
    public static Action<int> OnPlayerHit; 
    public static Action<EnemyController, int> OnEnemyHit;
    public static event Action OnGameOver;
}
