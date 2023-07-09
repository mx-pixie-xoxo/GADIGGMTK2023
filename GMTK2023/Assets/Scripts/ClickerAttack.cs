using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerAttack : MonoBehaviour
{
    public Vector3 Target;
    private Camera _mainCamera;
    private float _addAboveScreen = 100.0f; // change to based on sprite size * transform scale
    public float MoveSpeed = 5.0f;
    public ClickerBehavior Clicker;


    private void Start()
    {
        _mainCamera = Camera.main;
        Vector3 top = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, Screen.height + _addAboveScreen, _mainCamera.nearClipPlane));
        transform.position = new Vector3(Target.x, top.y, top.x);
    }

    private void Update()
    {
        float step = MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, Target, step);
        
        if (transform.position.y <= Target.y)
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
