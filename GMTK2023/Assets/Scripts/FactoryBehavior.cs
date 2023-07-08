using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryBehavior : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxhealth = 100f;
    [SerializeField] private float decrease = 1f; //How fast the health decreases when you stand next to it
    [SerializeField] private float increase = 0.5f; //How fast the health increases when you're not standing next to it
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool isTouchingPlayer;

    public GameObject hpSlider; //slider object on canvas
    public Sprite Untouched;
    public Sprite halfDestroyed;
    public Sprite Destroyed;
    public AudioClip[] DamageSounds;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        CookieManager.Instance.FactoryCount++;
    }

    void Update()
    {
        //update HP bar
        hpSlider.GetComponent<Slider>().value = health / maxhealth;

        //heals when not touching player
        if (!isTouchingPlayer && health < maxhealth)
        {
            health += increase * Time.deltaTime;
        }

        //Destroy self when health 0 and set sprite to different states of destruction
        if (health <= (maxhealth * 0.5f) && health > (maxhealth * 0.25f))//half health
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = halfDestroyed;
        }
        else if (health <= (maxhealth * 0.25f) && health > (0)) //quarter health
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Destroyed;
        }
        else if (health <= 0f)//death
        {
            // NOTE: probably change this to a call to some FactoryDestroyed function so that golden cookie can be spawned
            CookieManager.Instance.FactoryCount--;
            Destroy(hpSlider);
            Destroy(gameObject);
        }
        else //healed
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Untouched;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && health > 0.0f)
        {
            health -= decrease * Time.deltaTime;

            //play a damage sound at random if one isn't already playing
            if (!audioSource.isPlaying)
            {
                int _randomNum = Random.Range(0, DamageSounds.Length);
                audioSource.pitch = (Random.Range(0.8f, 1.2f));
                audioSource.PlayOneShot(DamageSounds[_randomNum]);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isTouchingPlayer = false;
        }
    }

}
