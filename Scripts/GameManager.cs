using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    public bool _isCoopMode;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    [SerializeField]
    private Animator _pauseAnimator;
    [SerializeField]
    private int _playerCount;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private Player[] _players;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        //_spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_playerCount == 2)
        {
            _isCoopMode = true;
        }
        else
        {
            _isCoopMode = false;
        }
    }

    private void Update()
    {
        if (_isGameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_isCoopMode == false)
                {
                    SceneManager.LoadScene(1); //Current Game Scene
                }
                else
                {
                    SceneManager.LoadScene(2);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {

            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public bool isCoopMode()
    {
        return _isCoopMode;
    }

    public void setGameMode(int mode)
    {
        if(mode == 1)
        {
            _isCoopMode = false;
        }
        else
        {
            _isCoopMode = true;
        }
    }

    public void PlayerDied()
    {
        _playerCount--;
    }
    public bool AllPlayersDead()
    {
        if (_playerCount == 0)
        {
            _spawnManager.OnPlayerDeath();
            return true;
        }
        return false;
    }

    public Player[] GetPlayers()
    {
        return _players;
    }


}
