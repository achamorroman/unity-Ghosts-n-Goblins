using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGhost : GameElement
{
    /// <summary>
    /// Audio clip to play when this element dies
    /// </summary>
    public AudioClip AudioDeath;

    private float TimeInState = 0;
    private bool Moving = true;
    private bool ReadyToShoot = true;
      
    /// <summary>
    /// 
    /// </summary>
    private void UpdateAnimator()
    {        
        if(!Moving)
            mAnimator.Play("FlyingGhostStandStill");
        else
        {
            if(ReadyToShoot)
                mAnimator.Play("FlyingGhostMovingWithArrow");
            else mAnimator.Play("FlyingGhostMovingNoArrow");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateVelocity()
    {
        if (Moving)
            mRigidBody.velocity = new Vector2(SpeedX, 0f);
        else
            mRigidBody.velocity = Vector2.zero;
    }
    /// <summary>
    /// 
    /// </summary>
    private void SpawnShot()
    {
        GameManager.CurrentLevel.SpawnFlyingGhostShot(this.transform.position);
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Always Look left
        mLookDir.LookLeft = true;

        if (!mRender.isVisible)
            return;

        // Decide is we should keep moving
        TimeInState += Time.deltaTime;
        if (TimeInState > 2)
        {
            if (Moving && ReadyToShoot)
                Moving = false;
            else 
            {
                Moving = true;
                if (ReadyToShoot)
                {
                    this.SpawnShot();
                    ReadyToShoot = false;
                }
            }

            TimeInState = 0;
        }

        // Update rest of elements
        UpdateVelocity();
        UpdateAnimator();

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
            ply.HitByFlyingGhost();
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

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 1f);
    }


}
