using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8;
    public float JumpForce = 10;
    public float HorizontalJumpForce = 3;

    private Rigidbody2D _rigidbody2D;
    bool isRigthMove = true;
    bool isJump = true;

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
        if (horizontalMovement < 0 && isRigthMove || horizontalMovement > 0 && !isRigthMove)
        {
            isRigthMove = !isRigthMove;
            gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    private void Move()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        if (!isJump)
        {
            _rigidbody2D.velocity = new Vector2(horizontalMovement * Speed, _rigidbody2D.velocity.y);
        }
        else
        {
            var velocity = _rigidbody2D.velocity.x + horizontalMovement * Speed;
            var sign = Mathf.Sign(velocity);
            _rigidbody2D.velocity = new Vector2(Mathf.Min(Mathf.Abs(velocity), Speed / 2) * sign, _rigidbody2D.velocity.y);
        }
    }

    private void Jump()
    {
        if (!isJump && Input.GetButtonDown("Jump"))
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
            bullet.GetComponent<BulletController>().Shot(isRigthMove ? 1 : -1);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = true;
        }
    }
}
