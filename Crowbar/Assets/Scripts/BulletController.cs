using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 20;

    public void Shot(float direction)
    {
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(direction * Speed, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
