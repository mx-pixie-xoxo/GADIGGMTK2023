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
    public float AttackTimeout = 10.0f;
    public float FactorySpawnRange = 5.0f;
    public float MoveTimeout = 1.0f;
    public GameObject FactoryPrefab;
    public GameObject AttackPrefab;

    [SerializeField]
    private float _attackTimeoutDelta;
    private float _moveTimeoutDelta;

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
                if (_attackTimeoutDelta <= 0) ClickAttack();
                break;
            case ClickerState.GoldenCookie:
                MoveTo(_goldenCookie);
                break;
            case ClickerState.Attack:   // change to idle?
                _moveTimeoutDelta -= Time.deltaTime;
                if (_moveTimeoutDelta <= 0) _state = ClickerState.FollowPlayer;
                break;
        }

        if (_attackTimeoutDelta >= 0) _attackTimeoutDelta -= Time.deltaTime;
    }

    private void MoveTo(GameObject target)
    {
        float step = MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
    }

    public void SpawnFactory()
    {
        Vector2 randomLoc = Random.insideUnitCircle * FactorySpawnRange;
        Vector3 spawnLoc = new Vector3(randomLoc.x, randomLoc.y, 0.0f) + transform.position;
        
        GameObject newFac = Instantiate(FactoryPrefab, spawnLoc, Quaternion.identity) as GameObject;
    }

    private void ClickAttack()
    {   
        // spawn click attack
        GameObject spawning = Instantiate(AttackPrefab, transform.position, Quaternion.identity) as GameObject;
        ClickerAttack attack = spawning.GetComponent<ClickerAttack>();
        attack.Target = transform.position;
        attack.Clicker = this;
        

        _attackTimeoutDelta = AttackTimeout;
        _moveTimeoutDelta = MoveTimeout;
        _state = ClickerState.Attack;
    }

}
