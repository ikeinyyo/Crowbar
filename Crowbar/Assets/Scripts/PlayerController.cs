using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10;
    public float JumpForce = 6;

    private Rigidbody2D _rigidbody2D;
    bool isRigthMove = true;
    bool isJump = true;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        MovePlayer(horizontalMovement);
        SetDirection(horizontalMovement);
        Shot();
    }

    private void SetDirection(float horizontalMovement)
    {
        if (horizontalMovement < 0 && isRigthMove || horizontalMovement > 0 && !isRigthMove)
        {
            isRigthMove = !isRigthMove;
            gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    private void MovePlayer(float horizontalMovement)
    {
        _rigidbody2D.velocity = new Vector2(horizontalMovement * Speed, _rigidbody2D.velocity.y);

        if (!isJump && Input.GetButtonDown("Jump"))
        {
            _rigidbody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            isJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
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
}
