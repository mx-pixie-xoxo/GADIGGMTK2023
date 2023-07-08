using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryHealth : MonoBehaviour
{
    private Vector3 _localScale;
    private float _hpStart;

    private void Awake()
    {
        _localScale = transform.localScale;
        _hpStart = _localScale.x;
    }

    public void UpdateBar(float scale)
    {
        _localScale.x = scale * _hpStart;
        transform.localScale = _localScale;
    }
}
