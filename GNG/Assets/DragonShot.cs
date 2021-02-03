using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShot : GameElement
{
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Velocity is always constant
        this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0f);

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hits the player 
        Player player = collision.collider.GetComponent<Player>();
        if(player != null)
        {
            player.HitByDragonShot();
            this.Destroy();
        }
    }
}
