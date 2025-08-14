using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    public UnityAction<Bubble> OutOfScreen;
    
    private Rigidbody  _rb;
    
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    
    public void PlayCreateAnimation() 
    {
        gameObject.SetActive(true);
        _animator.Play("BubbleCreateAnimation");
    }

    public void PlayDestroyAnimation()
    {
        _rb.linearVelocity = Vector3.zero;
        _animator.Play("BubbleDestroyAnimation");
    }
    public void MoveUp(int speed)
    {
        _rb.linearVelocity = Vector3.up * speed;
    }

    public void DestroyBubble()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DestroyPoint>())
        {
            OutOfScreen?.Invoke(this);
        }
    }
}
