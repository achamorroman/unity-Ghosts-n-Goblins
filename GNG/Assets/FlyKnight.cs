using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyKnight : GameElement
{
    private float ActivationTime = float.MinValue;

    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Always look left
        this.mLookDir.LookLeft = true;

        // Element's velocity is always constant, but should only move when visible
        if (mRender.isVisible)
        {
            if (ActivationTime < 0)
                ActivationTime = Time.time;
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, Mathf.Sin((Time.time - ActivationTime) * 2.5f) * 9f);
        }
        else this.mRigidBody.velocity = Vector2.zero;

        base.Update();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hits the player
        Player ply = collision.collider.GetComponent<Player>();
        if (ply != null)
        {
            ply.HitByFlyKnight();
            this.Destroy();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
    }

}
