using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float _velocity = 5.0f;
    
    private Rigidbody2D _rb;
    private Vector2 _movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(InputManager.movement.x, InputManager.movement.y);
        _rb.linearVelocity = _velocity * _movement;
    }
}
