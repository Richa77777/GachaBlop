using System.Collections;
using UnityEngine;

public class ButtonShaking : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            _animator.Play("Shake");
        }
    }
}
