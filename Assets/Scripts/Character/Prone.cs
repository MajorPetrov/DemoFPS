using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Prone : MonoBehaviour
{
    private Transform _fpsControllerTransform;
    private CharacterController _characterController;
    private FirstPersonController _firstPersonController;

    public bool IsProning { get; set; }

    // Use this for initialization
    void Start()
    {
        // Oui je sais, c'est dégueulasse, mais je déteste mélanger les prefabs avec mes trucs personnels.
        _fpsControllerTransform = transform.parent.transform.parent;
        _characterController = _fpsControllerTransform.gameObject.GetComponent<CharacterController>();
        _firstPersonController = _fpsControllerTransform.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Prone"))
            Proning();
    }

    private void Proning()
    {
        throw new NotImplementedException();
    }
}
