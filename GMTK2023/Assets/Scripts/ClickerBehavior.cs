using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerBehavior : MonoBehaviour
{
    private GameObject player;
    private Vector2 goal;
    [SerializeField] private float speed = 2.8f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        goal = player.GetComponent<Transform>().position;
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        }
    }
}
