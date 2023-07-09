using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerBehavior : MonoBehaviour
{   
    public float MoveSpeed = 3.0f;

    private ClickerState _state;
    private GameObject _player;
    private GameObject _goldenCookie; 
    private GameObject[] _attackInstance;

    // Attack stuff
    public float AttackTimeout = 5.0f;
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
        GoldenCookie,   // 2
        PrepAttack,     // 3
        Attack          // 4
    }

    public enum FollowAnchor : int
    {
        Top,         //0
        Center,      //1
        Bottom       //2
    }
    
    private void Awake()
    {
        // get a reference to our player
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        _state = ClickerState.FollowPlayer;
        _attackTimeoutDelta = AttackTimeout;
    }


    private void Update()
    {
        Debug.Log(_state);
        if (GameObject.FindGameObjectWithTag("GoldenCookie") != null)
        {
            _goldenCookie = GameObject.FindGameObjectWithTag("GoldenCookie");
            _state = ClickerState.GoldenCookie;
        }

        switch (_state)
        {
            case ClickerState.FollowPlayer:
                MoveTo(_player,FollowAnchor.Bottom);
                //glide above player to prepare attack
                if (_attackTimeoutDelta <= 0)
                {
                    _state = ClickerState.PrepAttack;
                    _attackTimeoutDelta = AttackTimeout / 2;
                }
                break;
            case ClickerState.GoldenCookie:
                if (GameObject.FindGameObjectWithTag("GoldenCookie") == null) _state = ClickerState.FollowPlayer; //if cookie doesn't exist, go back to following player
                else MoveTo(_goldenCookie, FollowAnchor.Bottom); //chase cookie
                break;
            case ClickerState.PrepAttack:
                MoveTo(_player,FollowAnchor.Top);
                if (_attackTimeoutDelta <= 0) { ClickAttack(); }
                break;
            case ClickerState.Attack:   // change to idle?
                _moveTimeoutDelta -= Time.deltaTime;
                if (_moveTimeoutDelta <= 0) _state = ClickerState.FollowPlayer;
                break;
        }
        switch (CookieManager.Instance.FactoryCount)
        {
            case 1:
                AttackTimeout = 1.0f;
                break;
            case 2:
                AttackTimeout = 2.0f;
                break;
            case 3:
                AttackTimeout = 3.0f;
                break;
            default:
                AttackTimeout = 5.0f;
                break;
        }
        //decrease time until attack prep / attack
        if (_attackTimeoutDelta >= 0) _attackTimeoutDelta -= Time.deltaTime;


        _attackInstance = GameObject.FindGameObjectsWithTag("MouseAttack");
        if (_attackInstance.Length > 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else GetComponent<SpriteRenderer>().enabled = true;
    }

    private void MoveTo(GameObject target, FollowAnchor anchor)
    {
        float step = MoveSpeed * Time.deltaTime;
        float _anchorOffset = 0;
        //mouse follows from below by default, so make anchor offset a bigger number to have it display higher up
        switch (anchor)
        {
            case FollowAnchor.Top:
                _anchorOffset = 6f;
                break;
            case FollowAnchor.Center:
                _anchorOffset = 0.2f;
                break;
            case FollowAnchor.Bottom:
                _anchorOffset = 0.0f;
                break;
        }
        Vector3 desiredPosition = new Vector3(target.transform.position.x, target.transform.position.y+_anchorOffset, target.transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, step);
    }

    public void SpawnFactory()
    {
        const int SPAWN_HEIGHT = 4;
        const int SPAWN_WIDTH = 8;
        var randomLocation = new Vector3(Random.Range(-SPAWN_WIDTH,SPAWN_WIDTH), Random.Range(-SPAWN_HEIGHT, SPAWN_HEIGHT),0.0f);
        
        GameObject newFac = Instantiate(FactoryPrefab, randomLocation, Quaternion.identity);
    }

    private void ClickAttack()
    {   
        // spawn click attack
        GameObject spawning = Instantiate(AttackPrefab, transform.position, Quaternion.identity);
        ClickerAttack attack = spawning.GetComponent<ClickerAttack>();
        attack.Target = transform.position;
        attack.Clicker = this;
        

        _attackTimeoutDelta = AttackTimeout;
        _moveTimeoutDelta = MoveTimeout;
        _state = ClickerState.Attack;
    }

}
