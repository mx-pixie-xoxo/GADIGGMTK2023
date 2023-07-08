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
    public float AttackTimeout = 10.0f;
    public float FactorySpawnRange = 5.0f;
    public float MoveTimeout = 1.0f;
    public GameObject FactoryPrefab;

    [SerializeField]
    private float _attackTimeoutDelta;
    private float _playerFoundDelta;
    private float _moveTimeoutDelta;
    
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
                _moveTimeoutDelta -= Time.deltaTime;
                if (_moveTimeoutDelta <= 0) _state = ClickerState.FollowPlayer;
                break;
        }

        if (_attackTimeoutDelta >= 0) _attackTimeoutDelta -= Time.deltaTime;

        if (_inRange && _state == ClickerState.FollowPlayer)
        {
            _playerFoundDelta -= Time.deltaTime;

            if (_playerFoundDelta <= 0 && _attackTimeoutDelta <= 0) 
            {
                _state = ClickerState.Attack;
                _attackTimeoutDelta = AttackTimeout;
                SpawnFactory();
            }
        }
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

    private void SpawnFactory()
    {
        Vector2 randomLoc = Random.insideUnitCircle * FactorySpawnRange;
        Vector3 spawnLoc = new Vector3(randomLoc.x, randomLoc.y, 0.0f) + transform.position;
        
        GameObject newFac = Instantiate(FactoryPrefab, spawnLoc, Quaternion.identity) as GameObject;

    }
}
