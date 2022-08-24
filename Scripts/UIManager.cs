using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _playerOneScoreText, _bestScoreText;
    [SerializeField]
    private Image _LivesImgOne, _LivesImgTwo;
    [SerializeField]
    private Sprite[] _liveSpritesOne, _liveSpriteTwo;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private SceneManager _sceneManager;
    [SerializeField]
    private bool _isCoopMode;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _isCoopMode = _gameManager.isCoopMode();
        _playerOneScoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdateScore(int playerScore)
    {
        _playerOneScoreText.text = "Score: " + playerScore;
    }

    public void UpdateBestScore(int bestScore)
    {
        _bestScoreText.text = "Best: " + bestScore;
    }

    public void UpdateLives(int currentLives, bool isPlayerOne)
    {
        if(isPlayerOne == true)
        {
            _LivesImgOne.sprite = _liveSpritesOne[currentLives];
            _LivesImgOne.sprite = _liveSpritesOne[currentLives];
        }
        else
        {
            _LivesImgTwo.sprite = _liveSpriteTwo[currentLives];
            _LivesImgTwo.sprite = _liveSpriteTwo[currentLives];
        }
        
        if (currentLives == 0 && _gameManager.AllPlayersDead() == true)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResumePlay()
    {
        GameManager gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gm.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
