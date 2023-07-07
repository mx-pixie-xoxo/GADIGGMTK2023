using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBehavior : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float decrease = 1f; //How fast the health decreases when you stand next to it

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy self when health 0
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            health -= decrease * Time.deltaTime;
        }
    }
}
