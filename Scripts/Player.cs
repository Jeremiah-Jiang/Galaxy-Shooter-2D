//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // : means extends or inherits from, MonoBehaviour is a Unity specific class
{
    //SerializeField enables developer to change this value in inspector mode

    [SerializeField]//Player Speed
    private float _speed = 7.0f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool _isDead = false;


    [SerializeField]
    private bool _isPlayerOne;

    [SerializeField]
    private bool _isPlayerTwo;

    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]//Laser Firing Properties
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    //Laser Spawn Properties
    private float _laserOffset = 0.8f;

    [SerializeField]
    private int _score, _bestScore;

    //Powerup Properties
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private float _tripleShotDuration = 5.0f;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _speedBoostDuration = 5.0f;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private GameObject _thruster, _speedBoostThruster;
    [SerializeField]
    private GameObject _tripleShotVisualiser;
    [SerializeField]
    private Animator _anim;
    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _playerExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Take the current position = new position (0, 0, 0)
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();

        //Null check for Spawn Manager
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        //Null check for UI Manager
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }


        //Null check for Game Manager
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL.");
        }
        else
        {
            if (_gameManager._isCoopMode == false)
            {
                _bestScore = PlayerPrefs.GetInt("HighScore", 0);
                _uiManager.UpdateBestScore(_bestScore);
                transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                _bestScore = PlayerPrefs.GetInt("CoopHighScore", 0);
                _uiManager.UpdateBestScore(_bestScore);
            }
        }

        //Null check for Audio Source
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOne == true)
        {
            PlayerOneMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }

        if (_isPlayerTwo == true)
        {
            PlayerTwoMovement();
            if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        
    }

    void PlayerOneMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
  

        //Mathf.clamp(...) means Limit y position to min of -3.8f and max of 0
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (Mathf.Abs(transform.position.x) >= 11.3f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _anim.SetBool("Turn_Left", true);
            _anim.SetBool("Turn_Right", false);
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", true);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
    }

    void PlayerTwoMovement()
    {

        if(Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Keypad5))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime) ;
        }

        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad4))
        {
            _anim.SetBool("Turn_Left", true);
            _anim.SetBool("Turn_Right", false);

        }
        else if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", true);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
        //Mathf.clamp(...) means Limit y position to min of -3.8f and max of 0
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (Mathf.Abs(transform.position.x) >= 11.3f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        //Vector3 laserSpawn = new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z);
        _canFire = Time.time + _fireRate;
        //Quaternion.identity means default position
        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }

        _audioSource.Play();

    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;
        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives <= 0)
        {
            Instantiate(_playerExplosionPrefab, transform.position, Quaternion.identity);
            _lives = 0;
            _isDead = true;
            _gameManager.PlayerDied();
            Destroy(GetComponent<Collider2D>());
            this.gameObject.SetActive(false);
        }
        _uiManager.UpdateLives(_lives, _isPlayerOne);
 
    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _tripleShotVisualiser.SetActive(true);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _isTripleShotActive = false;
        _tripleShotVisualiser.SetActive(false);
    }

    public void SpeedBoostActive()
    {
        if (_isSpeedBoostActive == false)
        {
            _isSpeedBoostActive = true;
            _thruster.SetActive(false);
            _speedBoostThruster.SetActive(true);
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _isSpeedBoostActive = false;
        _thruster.SetActive(true);
        _speedBoostThruster.SetActive(false);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        Debug.Log("Score: " + _score);
        if(_score > _bestScore )
        {
            _bestScore = _score;
            if (_gameManager._isCoopMode == false)
            {
                PlayerPrefs.SetInt("HighScore", _bestScore);
            }
            else
            {
                PlayerPrefs.SetInt("CoopHighScore", _bestScore);
            }
            _uiManager.UpdateBestScore(_bestScore);
        }
        _uiManager.UpdateScore(_score);
    }

    public void DeductScore(int points)
    {
        _score -= points;
        _uiManager.UpdateScore(_score);
    }

    public int getScore()
    {
        return _score;
    }

    public int getHighScore()
    {
        return _bestScore;
    }

    public bool isDead()
    {
        return _isDead;
    }

    public bool IsPlayerOne()
    {
        return _isPlayerOne;
    }

    public bool IsPlayerTwo()
    {
        return _isPlayerTwo;
    }
}
