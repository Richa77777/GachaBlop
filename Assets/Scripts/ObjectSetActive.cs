using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSetActive : MonoBehaviour
{
    public void OffObject()
    {
        gameObject.SetActive(false);
    }
}
