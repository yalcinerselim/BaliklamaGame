using System;
using UnityEngine;
using UnityEngine.Events;

public class FishController : MonoBehaviour
{
    public UnityAction HitBubble;
    public UnityAction UserReady;
    
    private Camera _mainCamera;
    private Vector3 _offset;
    private float _mZCoord;
    private AudioSource _audioSource;
    
    public bool clicked = false;

    private void Start()
    {
        _mainCamera = Camera.main;
        _audioSource =  GetComponent<AudioSource>();
    }

    public bool IsClicked()
    {
        return clicked;
    }

    private void OnMouseDown()
    { 
        UserReady?.Invoke();
        
        _mZCoord =  Mathf.Abs(_mainCamera.transform.position.z- transform.position.z);
        _offset = transform.position - GetMouseWorldPos();
        _audioSource.Play();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + _offset;
    }

    private void OnMouseUp()
    {
        _audioSource.Stop();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        
        mousePos.z = _mZCoord;
        
        return _mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bubble>())
        {
            HitBubble?.Invoke();
        }
    }
}
