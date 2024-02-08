using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffectRotation : MonoBehaviour
{
    public float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
