using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GamePaused,
        GameOver,
    }

    public event EventHandler OnGameStateChanged;

    private GameState _state;
    private float _countdownToStartTimer = 3;
    private float _gamePlayingTimer = 10;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Duplicate GameManager");
            Destroy(this);
            return;
        }
        instance = this;

        _state = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (_state)
        {
            case GameState.WaitingToStart:
                if(Input.GetMouseButtonDown(0))
                {
                    _state = GameState.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0)
                {
                    _state = GameState.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0)
                {
                    _state = GameState.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameOver:
                break;
        }
    }
    public void Restart()
    {
        _countdownToStartTimer = 3f;
        _gamePlayingTimer = 10f;
        _state = GameState.WaitingToStart;
    }

    public bool IsGamePlaying()
    {
        return _state == GameState.GamePlaying;
    }
    public bool IsGamePaused()
    {
        return _state == GameState.GamePaused;
    }
    public bool IsCountdownToStart()
    {
        return _state == GameState.CountdownToStart;
    }
    public bool IsWaitingToStart()
    {
        return _state == GameState.WaitingToStart;
    }
    public bool IsGameOver()
    {
        return _state == GameState.GameOver;
    }
}
