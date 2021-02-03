using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : GameElement
{
    /// <summary>
    /// Audio clip to play when this crow dies
    /// </summary>
    public AudioClip AudioDeath;
    /// <summary>
    /// Distance Activation Threshold, in m
    /// </summary>
    public float ActivationThreshold = 30;
    /// <summary>
    /// Trus if the crow is already flying. False otherwise
    /// </summary>
    private bool IsFlying;

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Crows should be destroyed when outside of the screen only if they are in flying state. Initially, when they are
        // standing on a grave, they should not be destroyed (they can be far away in the level).
        this.DestroyWhenNotVisible = false;
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Behavior should be different if the crow is already flying or not
        if (IsFlying)
        {
            mAnimator.Play("CrowFly");
            
            // Calculate speed that is constant horizontally, and that has a Sin waveform vertically
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, Mathf.Sin(Time.time * 2f) * 1.6f);

            // Once the crow is flying, it should be destroyed if gets out of the screen
            this.DestroyWhenNotVisible = true;
        }
        else
        {
            mAnimator.Play("CrowStand");
            
            // Speed should be zero when standing on a grave
            mRigidBody.velocity = Vector2.zero;
            
            // Make the crow look at the player when it's standing on a grave
            mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;
            
            // Crows shouldn't be destroyed when out of the screen while they are standing on the grave
            this.DestroyWhenNotVisible = false;

            // Activate crow when player gets close enough
            IsFlying = Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform)) < ActivationThreshold;
        }

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxSplash(this.transform.position);

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 0.5f);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if crow hits the player
        Player ply = collision.collider.GetComponent<Player>();
        if (ply != null)
        {
            ply.HitByCrow();
            this.Destroy();
        }
    }
}
