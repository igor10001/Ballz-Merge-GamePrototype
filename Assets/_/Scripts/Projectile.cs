using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Vector2 direction;

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            Vector2 collisionNormal = collision.contacts[0].normal;

            if (collisionNormal == Vector2.left)
                block.Move(Vector2Int.right);
            else if (collisionNormal == Vector2.right)
                block.Move(Vector2Int.left);
            else if (collisionNormal == Vector2.up)
                block.Move(Vector2Int.down);
            else if (collisionNormal == Vector2.down)
                block.Move(Vector2Int.up);

            // Reflect the ball's direction
            direction = Vector2.Reflect(direction, collisionNormal);
        }
    }

    public void Launch(Vector2 initialDirection)
    {
        direction = initialDirection;
    }
}
