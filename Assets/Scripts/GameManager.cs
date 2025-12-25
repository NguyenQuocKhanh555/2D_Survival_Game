using UnityEngine;

public enum GameState
{
    Setup,
    Playing,
    Paused,
    GameOver,
    Stopped
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState CurrentGameState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetGameState(GameState.Playing);
    }

    public void SetGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        HandleState(newState);
        CurrentGameState = newState;
    }

    private void HandleState(GameState state)
    {
        switch (state)
        {
            case GameState.Setup:
                // Initialize game setup
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                // Start gameplay
                break;
            case GameState.Paused:
                // Pause game
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                // Handle game over
                break;
            case GameState.Stopped:
                // Stop all game activities
                break;
        }
    }
}
