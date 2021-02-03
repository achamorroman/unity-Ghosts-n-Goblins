using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : GameElement
{
    public ePickupType PickupType = ePickupType.None;
    public eZombieState State = eZombieState.Appearing;
    public float MaxTimeLiving = 15f;
    public AudioClip AudioZombieHit;
    public SpriteRenderer PickupIcon;

    private CapsuleCollider2D mCapsuleCollider;
    private float mTimeLiving;
    private float mGravityScaleOriginalValue;

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {        
        this.mCapsuleCollider = this.GetComponent<CapsuleCollider2D>();
        this.DestroyWhenNotVisible = true;
        base.Awake();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        this.mGravityScaleOriginalValue = this.mRigidBody.gravityScale;
        
        // Make the zombie harmless while appearing
        this.mCapsuleCollider.enabled = false;

        this.mTimeLiving = 0;
        
        // Initialize the Pickup Icon if this zombie has a price hidden
        this.PickupIcon.enabled = this.PickupType != ePickupType.None;

        base.Start();
    }
   
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        mTimeLiving += Time.deltaTime;

        // Check if the zombie should get buried again in the ground
        if(mTimeLiving > MaxTimeLiving)
            this.State = eZombieState.Disappearing;

        // Update animator and physics properties according to State
        switch (this.State)
        {
            case eZombieState.Appearing:
                this.mAnimator.Play("ZombieStart");
                this.mRigidBody.velocity = Vector2.zero;
                this.mRigidBody.gravityScale = 0;
                break;
            case eZombieState.Walking:
                this.mAnimator.Play("ZombieWalk");
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, this.mRigidBody.velocity.y);
                this.mRigidBody.gravityScale = mGravityScaleOriginalValue;
                break;
            case eZombieState.Disappearing:
                this.mAnimator.Play("ZombieEnd");
                this.mRigidBody.velocity = Vector2.zero;
                this.mRigidBody.gravityScale = 0;
                break;
        }

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If collisions with graves are enabled (disabled by default), Zombies will turn back when hitting a grave
        Grave grv = collision.collider.GetComponent<Grave>();
        if(grv != null)
            mLookDir.LookLeft = !mLookDir.LookLeft;

        // Check if collided with the player
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByZombie();
            this.Destroy();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void StartAnimationFinished()
    {
        // When the start animation ends, zombie needs to start walking and stop being harmless
        this.State = eZombieState.Walking;
        this.mCapsuleCollider.enabled = true;
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();

        // Generate Fx visual when killed
        GameManager.CurrentLevel.SpawnFxSplash(this.transform.position);
        
        // Play killed audio using PlayClipAtPoint instead of an AudioSource (because we are destroying the Game Object and therefore the AudioSource would stop playing)
        AudioSource.PlayClipAtPoint(AudioZombieHit, this.transform.position, 0.5f);

        // If the zombie has a pickup hidden, spawn treasure box
        if (this.PickupType != ePickupType.None)
            GameManager.CurrentLevel.SpawnTreasureBox(this.PickupType, this.transform.position);
    }

}
