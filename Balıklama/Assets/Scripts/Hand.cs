using System;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void PlayHandAnimation()
    {
        _animator.Play("HandAnimation");
    }

    public void StopHandAnimation()
    {
        _animator.StopPlayback();
    }
}
