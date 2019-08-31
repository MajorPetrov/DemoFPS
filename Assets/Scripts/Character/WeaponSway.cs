using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6.0f;

    private Vector3 _initialPosition;

    // Use this for initialization
    void Start()
    {
        _initialPosition = transform.localPosition; // Position de l'élément en lui-même et non de toute l'arborescence
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = -Input.GetAxis("Mouse X") * amount;
        float mouseY = -Input.GetAxis("Mouse Y") * amount;

        mouseX = Mathf.Clamp(mouseX, -maxAmount, maxAmount);
        mouseY = Mathf.Clamp(mouseY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(mouseX, mouseY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + _initialPosition, Time.deltaTime * smoothAmount);
    }
}
