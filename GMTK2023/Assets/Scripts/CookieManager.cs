using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieManager : MonoBehaviour
{
    public static CookieManager Instance { get; private set; }

    public int StartingCookies;
    public int FactoryCount;

    private static int _cookieCount;
    [SerializeField]
    private Text _cookieDisplay;

    // Cookie counting stuff
    [Tooltip("Time required to pass before making a new cookie in a factory")]
    public float CookieTimeout = 1.0f;
    private float _cookieTimeoutDelta;

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
}
