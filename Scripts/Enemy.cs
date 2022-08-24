using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    //[SerializeField]
    //private float _increaseSpeedMultiplier = 1.05f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _playerOne;
    private Player _playerTwo;
    private Animator _anim;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private Player[] _players;

    private float _canFire = -1f;
    private float _fireRate;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _anim = GetComponent<Animator>();
        _spawnManager = transform.parent.parent.GetComponent<SpawnManager>();
        _speed = _spawnManager.getSpeed();
        _audioSource = GetComponent<AudioSource>();
        
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL");
        }
        else
        { 
            if(_gameManager.isCoopMode() == true)
            {
                Debug.Log("Enemy searching for Coop");
                _players = _gameManager.GetPlayers();
                for (int i = 0; i < _players.Length; i++)
                {
                    if(i == 0 && _players[0].isDead() == false)
                    {
                        _playerOne = GameObject.Find("PlayerOne").GetComponent<Player>();
                    }
                    else if(i == 1 && _players[1].isDead() == false)
                    {
                        _playerTwo = GameObject.Find("PlayerTwo").GetComponent<Player>();
                    }    
                }
            }
            else
            {
                Debug.Log("Enemy searching for Single Player");
                _playerOne = GameObject.Find("PlayerOne").GetComponent<Player>();
                Debug.Log("There is a playerOne: " + _playerOne != null);
            }
        }
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_speed != 0)
        {
            _speed = _spawnManager.getSpeed(); //since we are reusing instances
            _speed = _spawnManager.getSpeed(); //since we are reusing instances
        }
        EnemyMovement();
        if (Time.time > _canFire && _speed != 0)
        {
            Debug.Log("Enemy firing");
            _fireRate = Random.Range(6f, 10f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5f)
        {
            /*
            if (_playerOne != null)
            {
                _playerOne.DeductScore(10); //If it is not 10, there will be a bug in the code logic for increasing difficulty
            }
            */
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();
            //Null Checking very impt
            if (player.IsPlayerOne() == true)
            {
                _playerOne.Damage();
            }
            else if(player.IsPlayerTwo() == true)
            {
                _playerTwo.Damage();
            }

            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_playerOne != null)
            {
                Debug.Log("Adding score");
                _playerOne.AddScore(10);
 
            }
            if (_playerTwo != null)
            {
                _playerTwo.AddScore(10);

            }
 
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
        

    }
}
