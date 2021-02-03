using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShot : MonoBehaviour
{
    /// <summary>
    /// Direction of movement, in 2D
    /// </summary>
    public Vector2 Direction;
    /// <summary>
    /// Speed of movement, in m/s in the Direction
    /// </summary>
    public float Speed = 5;

    private Rigidbody2D mRigidbody;
    private SpriteRenderer mRender;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        this.mRigidbody = this.GetComponent<Rigidbody2D>();
        this.mRender = this.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        this.mRigidbody.velocity = this.Direction * this.Speed;

        // Destroy shot when out of screen
        if (!this.mRender.isVisible)
            this.Destroy();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check of shot hits the player
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByPlantShot();
            this.Destroy();
        }
    }
}
