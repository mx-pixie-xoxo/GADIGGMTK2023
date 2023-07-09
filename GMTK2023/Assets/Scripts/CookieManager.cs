using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieManager : MonoBehaviour
{
    public static CookieManager Instance { get; private set; }
    public GameObject goldCookiePrefab;
    public int StartingCookies;
    public int FactoryCount;

    private static int _cookieCount;
    [SerializeField]
    private Text _cookieDisplay;

    // Cookie counting stuff
    [Tooltip("Time required to pass before making a new cookie in a factory")]
    public float CookieTimeout = 1.0f;
    private float _cookieTimeoutDelta;

    //golden cookie timer stuff
    [Tooltip("Time required to pass before spawning Golden Cookie")]
    public float GoldCookieMaxTime = 1.0f;
    private float _goldCookieTimeLeft = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        
        FactoryCount = 0;
    }

    private void Start()
    {
        UpdateCount(StartingCookies);
        _cookieTimeoutDelta = CookieTimeout;
    }

    private void Update()
    {
        if (_cookieTimeoutDelta >= 0) _cookieTimeoutDelta -= Time.deltaTime;

        if (_cookieTimeoutDelta <= 0)
        {
            UpdateCount(_cookieCount + FactoryCount);
            _cookieTimeoutDelta = CookieTimeout;
        }

        //spawn gold cookie
        if (_goldCookieTimeLeft >= 0 && GameObject.FindGameObjectWithTag("GoldenCookie") == null) _goldCookieTimeLeft -= Time.deltaTime;
        if (_goldCookieTimeLeft <= 0)
        {
            SpawnGoldenCookie();
            _goldCookieTimeLeft = GoldCookieMaxTime;
        }
    }

    public void UpdateCount(int value)
    {
        _cookieCount = value;
        _cookieDisplay.text = "Cookies: " + _cookieCount;
    }

    public int GetCount()
    {
        return _cookieCount;
    }

    public void SpawnGoldenCookie()
    {
        const int SPAWN_HEIGHT = 2;
        const int SPAWN_WIDTH = 4;
        var randomLocation = new Vector3(Random.Range(-SPAWN_WIDTH, SPAWN_WIDTH), Random.Range(-SPAWN_HEIGHT, SPAWN_HEIGHT), 0.0f);

        GameObject newFac = Instantiate(goldCookiePrefab, randomLocation, Quaternion.identity);
    }
}
