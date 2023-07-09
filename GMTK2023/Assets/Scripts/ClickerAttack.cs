using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerAttack : MonoBehaviour
{
    public Vector3 Target;
    private Camera _mainCamera;
    private float _spawnAbovePlayer = 300.0f; // change to based on sprite size * transform scale
    public float MoveSpeed = 2.5f;
    public ClickerBehavior Clicker;
    private Vector3 _playerPosAtSpawn;


    private void Start()
    {
        _mainCamera = Camera.main;
        Vector3 top = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, Target.y + _spawnAbovePlayer, _mainCamera.nearClipPlane));
        transform.position = new Vector3(Target.x, Target.y, top.z);
        _playerPosAtSpawn = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        float step = MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, _playerPosAtSpawn, step);
        
        if (transform.position.y <= _playerPosAtSpawn.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Clicker.SpawnFactory();
            Destroy(gameObject);
        }
    }
}
