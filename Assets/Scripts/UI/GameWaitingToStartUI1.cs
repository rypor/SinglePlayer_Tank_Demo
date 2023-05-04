using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Show();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
