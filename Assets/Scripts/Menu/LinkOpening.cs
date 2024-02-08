using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkOpening : MonoBehaviour
{
    [SerializeField] string _link;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL(_link);});
    }
}
