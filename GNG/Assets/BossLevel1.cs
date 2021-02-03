using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel1 : GameElement
{
    /// <summary>
    /// Vertical force applied to rigidbody when Jumping, in N
    /// </summary>
    public float JumpForceNewtons = 1500f;
    /// <summary>
    /// State of Boxx
    /// </summary>
    public eBossLevel1State State = eBossLevel1State.StandStill;

    [Header("Audio Clips")]
    public AudioClip AudioDeath;
    public AudioClip AudioHit;

    private float TimeInState = 0f;
    private float mInitialPosY = 0;


    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        this.mInitialPosY = this.transform.position.y;
        base.Start();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Walk()
    {
        this.State = eBossLevel1State.Walk;
        TimeInState = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    private void Jump()
    {
        this.State = eBossLevel1State.Jump;
        mRigidBody.AddForce(new Vector2(0f, JumpForceNewtons));
        TimeInState = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    private void Stop()
    {
        this.State = eBossLevel1State.StandStill;
        TimeInState = 0;
    }

    #region Update
    /// <summary>
    /// 
    /// </summary>
    private void UpdateState()
    {
        this.TimeInState += Time.deltaTime;

        // Make state change randomly every 1 seconds
        if (TimeInState > 1)
        {
            switch (this.State)
            {
                case eBossLevel1State.StandStill:
                    if (GameManager.FlipCoin())
                        Walk();
                    else Jump();
                    break;
                case eBossLevel1State.Walk:
                    if (GameManager.FlipCoin())
                        Stop();
                    else Jump();
                    break;
                case eBossLevel1State.Jump:
                    if (this.transform.position.y < mInitialPosY + 0.1)
                        Stop();
                    break;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateAnimator()
    {
        // Update animation according to State
        switch (this.State)
        {
            case eBossLevel1State.StandStill:
                mAnimator.Play("BossStandStill");
                break;
            case eBossLevel1State.Walk:
                mAnimator.Play("BossWalk");
                break;
            case eBossLevel1State.Jump:
                mAnimator.Play("BossJump");
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateVelocity()
    {
        // Update velocity according to State. When walking, it should move horizontally only. 
        switch (this.State)
        {
            case eBossLevel1State.Walk:
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0f);
                break;
            case eBossLevel1State.Jump:
                this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, this.mRigidBody.velocity.y);
                break;
            case eBossLevel1State.StandStill:
                this.mRigidBody.velocity = Vector2.zero;
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Do not update until visible
        if (!this.mRender.isVisible)
            return;

        UpdateState();
        UpdateAnimator();
        UpdateVelocity();

        base.Update();
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hits the player
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByBoss();

            this.HitByPlayerShot();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public override void HitByPlayerShot()
    {
        base.HitByPlayerShot();

        AudioSource.PlayClipAtPoint(AudioHit, this.transform.position, 1f);
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
        GameManager.CurrentLevel.SpawnFxDeathFire(this.transform.position);
        GameManager.CurrentLevel.StopBackgroundMusic();        
        
        // Play Victory Sound
        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 0.75f);

        GameManager.CurrentLevelFinished();
    }
}
