using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private Crouch _crouch;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _crouch = transform.parent.GetComponent<Crouch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator == null)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Move(x, y);
    }

    private void Move(float x, float y)
    {
        _animator.SetFloat("VelX", x);
        _animator.SetFloat("VelY", y);

        if (Input.GetButton("Sprint"))
        {
            _animator.SetBool("Sprint", true);

            if (_animator.GetFloat("VelY") > 0.0f && (_crouch.IsCrouching || _animator.GetBool("Aiming")))
            {
                _animator.SetBool("Aiming", false);
                _crouch.Crouching();
            }
        }
        else
            _animator.SetBool("Sprint", false);
    }
}
