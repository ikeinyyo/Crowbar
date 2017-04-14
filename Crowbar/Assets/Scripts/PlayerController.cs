using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8;
    public float JumpForce = 10;

    private Rigidbody2D _rigidbody2D;
    private bool _isRightMove = true;
    private bool _isJump = true;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Flip();
        Jump();
        Shot();
    }

    private void Flip()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        if (horizontalMovement < 0 && _isRightMove || horizontalMovement > 0 && !_isRightMove)
        {
            _isRightMove = !_isRightMove;
            gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    private void Move()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var velocity = _isJump ? horizontalMovement * Speed / 2 : horizontalMovement * Speed;
        _rigidbody2D.velocity = new Vector2(velocity, _rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        if (!_isJump && Input.GetButtonDown("Jump"))
        {
            _rigidbody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    private void Shot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bulletSpawn = transform.FindChild("BulletSpawn");
            var bullet = Instantiate(Resources.Load("Bullet"), bulletSpawn.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletController>().Shot(_isRightMove ? 1 : -1);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isJump = true;
        }
    }
}
