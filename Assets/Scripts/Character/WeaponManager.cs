using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;

    private int _index;
    private float _switchDelay = 0.5f;
    private bool _isSwitching;

    // Use this for initialization
    void Start()
    {
        InitializeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !_isSwitching)
        {
            _index++;

            if (_index >= weapons.Length)
                _index = 0;

            StartCoroutine(SwitchAfterDelay(_index));
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !_isSwitching)
        {
            _index--;

            if (_index < 0)
                _index = weapons.Length - 1;

            StartCoroutine(SwitchAfterDelay(_index));
        }
    }

    private void InitializeWeapons()
    {
        foreach (var weapon in weapons)
            weapon.SetActive(false);

        weapons[0].SetActive(true);
    }

    private void SwitchWeapons(int index)
    {
        foreach (var weapon in weapons)
            weapon.SetActive(false);

        weapons[index].SetActive(true);
    }

    private IEnumerator SwitchAfterDelay(int index)
    {
        _isSwitching = true;
        yield return new WaitForSeconds(_switchDelay);
        SwitchWeapons(index);
        _isSwitching = false;
    }
}
