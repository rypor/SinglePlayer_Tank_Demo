using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GamePaused,
        GameOver,
    }

    private GameState _state;
    private float _waitingToStartTimer = 1;
    private float _countdownToStartTimer = 3;
    private float _gamePlayingTimer = 10;

    private void Awake()
    {
        _state = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (_state)
        {
            case GameState.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if(_waitingToStartTimer < 0)
                    _state = GameState.CountdownToStart;
                break;
            case GameState.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0)
                    _state = GameState.GamePlaying;
                break;
            case GameState.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0)
                    _state = GameState.GameOver;
                break;
            case GameState.GameOver:
                break;
        }
    }
}
