using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountdownUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameCountdownToStartUI;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Hide();
    }


    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.instance.IsCountdownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        StartCoroutine(PlayCountdown());
    }

    private void Hide()
    {
        StopAllCoroutines();
        foreach (GameObject g in _gameCountdownToStartUI)
            g.SetActive(false);
    }

    private IEnumerator PlayCountdown()
    {
        _gameCountdownToStartUI[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        _gameCountdownToStartUI[0].SetActive(false);
        _gameCountdownToStartUI[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        _gameCountdownToStartUI[1].SetActive(false);
        _gameCountdownToStartUI[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        _gameCountdownToStartUI[2].SetActive(false);
    }
}
