using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerBehavior : MonoBehaviour
{   
    public float MoveSpeed = 2.8f;

    private ClickerState _state;
    private GameObject _player;
    private GameObject _goldenCookie;

    // Attack stuff
    public float PlayerFoundTimeout = 0.5f;
    public float AttackTimeout = 1.0f;
    public float FactorySpawnRange = 5.0f;
    public GameObject FactoryPrefab;

    private float _attackTimeoutDelta;
    private float _playerFoundDelta;
    private bool _inRange;

    public enum ClickerState : int {
        Idle,           // 0
        FollowPlayer,   // 1
        GoldenCookie,    // 2
        Attack          // 3
    }
    
    private void Awake()
    {
        // get a reference to our player
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        _state = ClickerState.FollowPlayer;
    }


    private void Update()
    {
        switch (_state)
        {
            case ClickerState.FollowPlayer:
                MoveTo(_player);
                break;
            case ClickerState.GoldenCookie:
                MoveTo(_goldenCookie);
                break;
            case ClickerState.Attack:
                _attackTimeoutDelta -= Time.deltaTime;
                if (_attackTimeoutDelta <= 0) _state = ClickerState.FollowPlayer;
                break;
        }

        if (_inRange)
        {
            _playerFoundDelta -= Time.deltaTime;

            if (_playerFoundDelta <= 0) 
            {
                _state = ClickerState.Attack;
                SpawnFactory();
            }
        }
    }

    private void MoveTo(GameObject target)
    {
        float step = MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _inRange = true;
            _playerFoundDelta = PlayerFoundTimeout;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _inRange = false;
        }
    }

    private void SpawnFactory()
    {
        Vector2 randomLoc = Random.insideUnitCircle * FactorySpawnRange;

        Vector3 spawnLoc = new Vector3(randomLoc.x, randomLoc.y, 0.0f) + transform.position;

        // Instantiate(FactoryPrefab, spawnLoc, Quaternion.identity);   // need an hp bar first? idk
    }

}
