using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diablo : GameElement
{
    /// <summary>
    /// Vertical velocity, in m/s
    /// </summary>
    public float SpeedY = 8;
    /// <summary>
    /// State of the diablo
    /// </summary>
    public eDiabloState State = eDiabloState.StandStill;
    /// <summary>
    /// Period between state changes, in seconds
    /// </summary>
    public int StateChangePeriod = 2;
    /// <summary>
    /// Distance Activation Threshold, in m
    /// </summary>
    public float ActivationThreshold = 10;
    /// <summary>
    /// True if the Diablo has already been activated
    /// </summary>
    public bool Activated;
    /// <summary>
    /// Direction of movement, in 2D
    /// </summary>
    private Vector2 mDirection = Vector2.right;
    /// <summary>
    /// Time the Diablo has been in the current state
    /// </summary>
    private float TimeInState = 0;
    /// <summary>
    /// Time the diablo has been moving in the current direction
    /// </summary>
    private float TimeInCurDirection = 0;
    /// <summary>
    /// Initial Position in Y
    /// </summary>
    private float mInitialPosY;

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        mInitialPosY = this.transform.position.y;
        base.Start();
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateState()
    {
        TimeInState += Time.deltaTime;

        // If player not close enough yet, always in standstill
        if (!Activated)
        {
            this.State = eDiabloState.StandStill;
            return;
        }

        // If enough time in current state, choose a new state randomly
        switch (this.State)
        {
            case eDiabloState.StandStill:
                if (TimeInState > StateChangePeriod)
                {
                    this.State = GameManager.FlipCoin()? eDiabloState.Flying: eDiabloState.Walking;
                    TimeInState = 0;
                }
                break;
            case eDiabloState.Flying:
                if(TimeInState > StateChangePeriod && this.transform.position.y < mInitialPosY + 0.5f)
                {
                    this.State = GameManager.FlipCoin()? eDiabloState.Walking: eDiabloState.StandStill;
                    TimeInState = 0;
                }
                break;
            case eDiabloState.Walking:
                if (TimeInState > StateChangePeriod)
                {
                    this.State = GameManager.FlipCoin()? eDiabloState.Flying: eDiabloState.StandStill;
                    TimeInState = 0;
                }
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateAnimator()
    {
        // Play different animations based on State
        switch(this.State)
        {
            case eDiabloState.StandStill:
                mAnimator.Play("DiabloStand");
                break;
            case eDiabloState.Flying:
                mAnimator.Play("DiabloFly");
                break;
            case eDiabloState.Walking:
                mAnimator.Play("DiabloWalk");
                break;
        }
    }   
    /// <summary>
    /// 
    /// </summary>
    private void UpdateVelocity()
    {
        // Update velocity depending on State
        switch (this.State)
        {
            case eDiabloState.StandStill:
                mRigidBody.velocity = Vector2.zero;
                this.transform.position = new Vector3(this.transform.position.x, mInitialPosY, this.transform.position.z);
                break;
            case eDiabloState.Flying:
                mRigidBody.velocity = mDirection * SpeedY;
                break;
            case eDiabloState.Walking:
                mRigidBody.velocity = mDirection * SpeedX;
                this.transform.position = new Vector3(this.transform.position.x, mInitialPosY, this.transform.position.z);
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateDirection()
    {
        // Keep track of time in current direction
        TimeInCurDirection += Time.deltaTime;

        // Store the direction that looks to player just as a commodity, used later
        Vector2 dirToPlayer = mLookDir.LookLeft ? Vector2.left : Vector2.right;

        // Update direction of movement, based on State
        switch (this.State)
        {
            case eDiabloState.StandStill:
                mDirection = Vector2.zero;
                break;
            case eDiabloState.Flying:
                // Always go up at the beginning of state cycle
                if (TimeInState < 1)
                    mDirection = Vector2.up;
                else if (TimeInState < 3)
                {
                    if (TimeInCurDirection >= 1)
                    {
                        // In the middle, choose randomly every second                    
                        mDirection = GameManager.FlipCoin() ? dirToPlayer: Vector2.zero;
                        TimeInCurDirection = 0;
                    }
                }
                else
                {
                    // Always go down at the end of state cycle
                    mDirection = Vector2.down;
                }
                break;
            case eDiabloState.Walking:
                if (TimeInCurDirection >= 1)
                {
                    // Choose randomly every second                    
                    mDirection = GameManager.FlipCoin() ? dirToPlayer : Vector2.zero;
                    TimeInCurDirection = 0;
                }
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Check if Diablo should be activated
        if (!Activated && Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform)) < ActivationThreshold)
            Activated = true;

        UpdateState();
        UpdateAnimator();
        UpdateDirection();
        UpdateVelocity();
     

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hits the player
        Player p = collision.collider.GetComponent<Player>();
        if(p != null)
            p.HitByDiablo();
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
