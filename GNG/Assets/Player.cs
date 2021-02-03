using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : GameElement
{
    private AudioSource mAudioSource;
    private CircleCollider2D mCollider;
    private BoxCollider2D mGroundCheckCollider;
    private List<PlayerShot> mShots = new List<PlayerShot>();
    private ePlayerState mStateBeforeShooting = ePlayerState.Idle;
    private Vector2 mInitialPosition;
    private float mGravityScaleOriginalValue;

    [Header("Player Status")]
    public int Lives = 3;
    public int Score = 0;
    public float GodModeTimeCounter = 0f;
    public ePlayerState State = ePlayerState.Idle;
    public ePlayerArmorState ArmorState = ePlayerArmorState.FullArmor;
    public eWeapon CurrentWeapon = eWeapon.Spear;
    
    [Header("Dynamics")]
    public float JumpForceNewtons = 1200f;
    public float PlayerSpeedLadder = 10f;

    [Header("Environment Info")]
    public MovingPlatform InMovingPlatform;
    public Ladder InLadder;
    public Collider2D LadderClimbDownCollider;
    public bool Grounded = false;
    public bool ReadyToClimbUp;
    public bool ReadyToClimbDown;

    [Header("Audio Clips")]
    public AudioClip AudioJump;
    public AudioClip AudioThrow;
    public AudioClip AudioHit;
    public AudioClip AudioDeath;
    public AudioClip AudioArmorPickup;
    public AudioClip AudioTreasurePickup;
    public AudioClip AudioWeaponPickup;

    [Header("Spawn Prefabs")]
    public GameObject PrefabShot;


    public bool IsShooting
    {
        get { return this.State == ePlayerState.Shoot || this.State == ePlayerState.ShootCrouch; }
    }
    public bool IsClimbing
    {
        get { return this.State == ePlayerState.Climb; }
    }
    public bool IsNaked
    {
        get { return this.ArmorState == ePlayerArmorState.Naked; }
    }
    public bool IsFrog
    {
        get { return this.ArmorState == ePlayerArmorState.Frog; }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        GameManager.Player = this;

        base.Awake();
        mGroundCheckCollider = this.GetComponent<BoxCollider2D>();
        mCollider = this.GetComponent<CircleCollider2D>();
        mAudioSource = this.GetComponent<AudioSource>();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        this.Lives = 3;
        this.Score = 0;
        mGravityScaleOriginalValue = this.mRigidBody.gravityScale;
        mInitialPosition = this.transform.position;
        base.Start();
    }

    #region Player Actions
    /// <summary>
    /// 
    /// </summary>
    private void ClimbUp()
    {
        this.State = ePlayerState.Climb;
        mRigidBody.velocity = new Vector2(0, PlayerSpeedLadder);
    }
    /// <summary>
    /// 
    /// </summary>
    private void ClimbDown()
    {
        this.State = ePlayerState.Climb;
        mRigidBody.velocity = new Vector2(0, -PlayerSpeedLadder);
    }
    /// <summary>
    /// 
    /// </summary>
    private void StopClimb()
    {
        this.mRigidBody.velocity = Vector2.zero;
    }
    /// <summary>
    /// 
    /// </summary>
    private void MoveRight()
    {
        mRigidBody.velocity = new Vector2(SpeedX, mRigidBody.velocity.y);
        mLookDir.LookLeft = false;
        this.State = ePlayerState.Run;
    }
    /// <summary>
    /// 
    /// </summary>
    private void MoveLeft()
    {
        mRigidBody.velocity = new Vector2(-SpeedX, mRigidBody.velocity.y);
        mLookDir.LookLeft = true;
        this.State = ePlayerState.Run;
    }
    /// <summary>
    /// 
    /// </summary>
    private void StickToMovingPlatform()
    {
        this.mRigidBody.velocity = new Vector2(this.InMovingPlatform.mRigidBody.velocity.x, this.mRigidBody.velocity.y);
    }
    /// <summary>
    /// 
    /// </summary>
    private void Jump()
    {
        mRigidBody.AddForce(new Vector2(0f, JumpForceNewtons));

        mAudioSource.clip = this.AudioJump;
        mAudioSource.Play();
    }   
    /// <summary>
    /// 
    /// </summary>
    private void JumpBackwards()
    {
        float forceX = JumpForceNewtons / 2f;
        mRigidBody.AddForce(new Vector2(mLookDir.LookLeft? forceX : -forceX, JumpForceNewtons));
    }
    /// <summary>
    /// 
    /// </summary>
    private void Shoot(bool pCrouch)
    {
        if (mShots.Count < 3)
        {
            mStateBeforeShooting = this.State;
            SpawnShot();

            if(pCrouch)
                this.State = ePlayerState.ShootCrouch;
            else this.State = ePlayerState.Shoot;


            mAudioSource.clip = this.AudioThrow;
            mAudioSource.Play();
        }
    }
    /// <summary>
    /// Makes the player crouch. pLookLeft is an optional parameter, when supplied, changes the look direction
    /// </summary>
    private void Crouch(bool? pLookLeft = null)
    {
        this.State = ePlayerState.Crouch;

        // Allow the player to change direction when crouched
        if (pLookLeft.HasValue)
            mLookDir.LookLeft = pLookLeft.Value;
    }
    /// <summary>
    /// 
    /// </summary>
    private void Stop()
    {
        mRigidBody.velocity = new Vector2(0, mRigidBody.velocity.y);
        this.State = ePlayerState.Idle;
    }
    #endregion

    #region Hit Methods
    /// <summary>
    /// 
    /// </summary>
    public void HitByDiablo()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByPlant()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByPlantShot()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByZombie()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByFlyingGhost()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByBoss()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByFlyKnight()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByCrow()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByDragonShot()
    {
        this.ArmorState = ePlayerArmorState.Frog;
        mAudioSource.clip = this.AudioHit;
        mAudioSource.Play();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HitByFlyingGhostShot()
    {
        Hit();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Hit()
    {
        // No harm if we're in god mode
        if (GodModeTimeCounter > 0)
            return;

        switch (this.ArmorState)
        {
            case ePlayerArmorState.FullArmor:
                this.ArmorState = ePlayerArmorState.Naked;
                this.GodModeTimeCounter = 3;
                mAudioSource.clip = this.AudioHit;
                mAudioSource.Play();
                this.JumpBackwards();
                break;
            case ePlayerArmorState.Frog:
                this.ArmorState = ePlayerArmorState.Naked;
                this.GodModeTimeCounter = 3;
                mAudioSource.clip = this.AudioHit;
                mAudioSource.Play();
                this.JumpBackwards();
                break;
            case ePlayerArmorState.Naked:
                this.DieArthurDie();
                break;
        }
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void DieArthurDie()
    {
        if (this.Lives > 1)
        {            
            this.Lives--;

            // Play audio just once
            mAudioSource.PlayOneShot(this.AudioDeath, 0.75f);
            this.State = ePlayerState.Die;
            return;
        }
        else GameOver();
    }
    /// <summary>
    /// 
    /// </summary>
    private void GameOver()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public void Pickup(Pickup pPickup)
    {
        switch (pPickup.Type)
        {
            case ePickupType.Armor:
                this.ArmorState = ePlayerArmorState.FullArmor;
                this.mAudioSource.clip = this.AudioTreasurePickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Axe:
                this.CurrentWeapon = eWeapon.Axe;
                this.mAudioSource.clip = this.AudioWeaponPickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Cross:
                this.Score += 200;
                this.mAudioSource.clip = this.AudioTreasurePickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Dagger:
                this.CurrentWeapon = eWeapon.Dagger;
                this.mAudioSource.clip = this.AudioWeaponPickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Torch:
                this.CurrentWeapon = eWeapon.Torch;
                this.mAudioSource.clip = this.AudioWeaponPickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Shield:
                this.CurrentWeapon = eWeapon.Shield;
                this.mAudioSource.clip = this.AudioWeaponPickup;
                this.mAudioSource.Play();
                break;
            case ePickupType.Spear:
                this.CurrentWeapon = eWeapon.Spear;
                this.mAudioSource.clip = this.AudioWeaponPickup;
                this.mAudioSource.Play();
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShootAnimationFinished()
    {
        this.State = this.mStateBeforeShooting;
    }
    /// <summary>
    /// 
    /// </summary>
    public void DeathAnimationFinished()
    {
        // When player dies, we need to give some time to play the death animation. When the animation finishes, this method is called so we can reset the level and player
        GameManager.ResetLevel();
    }
    /// <summary>
    /// This method is called when arthur dies, so it resets its state and position to the initial value
    /// </summary>
    public void ResetPlayer()
    {
        this.transform.position = mInitialPosition;
        this.ArmorState = ePlayerArmorState.FullArmor;
        this.State = ePlayerState.Idle;
    }

    #region Update Methods
    /// <summary>
    /// 
    /// </summary>
    private void UpdateInput()
    {
        // WARNING: Order of these actions is 100% relevant. Shoot, and then jump, should preceed to everything else        
        // Some of this actions should only be allowed when Grounded (like jump or move), while others (like shooting) are 
        // always allowed.
        if (IsShooting || this.State == ePlayerState.Die)
            return;

        // SPECIAL CASE: When already in climbing state and not pressing any key, velocity should be zero           
        if (IsClimbing && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            StopClimb();

        if (Input.GetKeyDown(KeyCode.Z) && Grounded)
            Jump();
        else if (Input.GetKeyDown(KeyCode.X) && !IsFrog)
            Shoot(Input.GetKey(KeyCode.DownArrow));
        else if (Input.GetKey(KeyCode.DownArrow) && !IsClimbing && !ReadyToClimbDown)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                Crouch(true);
            else if (Input.GetKey(KeyCode.RightArrow))
                Crouch(false);
            else Crouch();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Grounded)
            MoveRight();
        else if (Input.GetKey(KeyCode.LeftArrow) && Grounded)
            MoveLeft();
        else if (ReadyToClimbUp && Input.GetKey(KeyCode.UpArrow))
            ClimbUp();
        else if (ReadyToClimbDown && Input.GetKey(KeyCode.DownArrow))
            ClimbDown();
        else
        {
            // This is what happens when no input is received from the player
            if (Grounded)
                Stop();
            if (InMovingPlatform != null)
                StickToMovingPlatform();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateAnimator()
    {
        // Reset AnimatorSpeed to its default value
        mAnimator.speed = 1;

        // If dying, no other logic is allowed
        if (this.State == ePlayerState.Die)
        {
            mAnimator.Play("Death");
            return;
        }

        // Make the Jump Animation preceed any other anymation, excep the Shoot animation
        // The second condition (!IsShooting) allows this, to play the shoot animation even if we are in the middle of a Jump
        if (!Grounded && !IsShooting && !IsClimbing)
        {
            if (IsFrog)
                mAnimator.Play("FrogJump");
            else if (Mathf.Abs(mRigidBody.velocity.x) < 1)
                mAnimator.Play(IsNaked ? "JumpIdleNaked" : "JumpIdle");
            else mAnimator.Play(IsNaked ? "JumpRunningNaked" : "JumpRunning");

            return;
        }

        // Update Animator
        switch (this.State)
        {
            case ePlayerState.Idle:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Idle");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("FrogIdle");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("IdleNaked");
                        break;
                }
                break;
            case ePlayerState.Run:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Run");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("RunNaked");
                        break;
                }
                break;
            case ePlayerState.Climb:
                mAnimator.Play(IsNaked ? "ClimbNaked" : "Climb");

                // Stop animation if player is not moving up or down
                if (Mathf.Abs(mRigidBody.velocity.y) < 0.01f)
                    mAnimator.speed = 0;
                break;
            case ePlayerState.Crouch:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Crouch");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("CrouchNaked");
                        break;
                }

                break;
            case ePlayerState.Die:
                mAnimator.Play("Death");
                break;
            case ePlayerState.LosingArmour:
                mAnimator.Play("ArmourLost");
                break;
            case ePlayerState.Shoot:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("Shoot");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("ShootNaked");
                        break;
                }
                break;
            case ePlayerState.ShootCrouch:
                switch (this.ArmorState)
                {
                    case ePlayerArmorState.FullArmor:
                        mAnimator.Play("ShootCrouch");
                        break;
                    case ePlayerArmorState.Frog:
                        mAnimator.Play("Frog");
                        break;
                    case ePlayerArmorState.Naked:
                        mAnimator.Play("ShootCrouchNaked");
                        break;
                }
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateGraphics()
    {
        if(GodModeTimeCounter > 0)
        {
            GodModeTimeCounter -= Time.deltaTime;
            GodModeTimeCounter = Mathf.Max(0f, this.GodModeTimeCounter);

            // Make the player blink while it's in GodMode
            mRender.enabled = ((int)(GodModeTimeCounter * 10f)) % 2 == 0;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdatePhysics()
    {
        if(LadderClimbDownCollider)
            Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, false);

        switch (this.State)
        {
            case ePlayerState.Climb:
                this.mRigidBody.gravityScale = 0;
                
                if (LadderClimbDownCollider)
                    Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, true);
                break;
            default:
                this.mRigidBody.gravityScale = mGravityScaleOriginalValue;
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateGroundCheck()
    {
        Vector2 groundBoxWorldPosition = (Vector2)this.transform.position + this.mGroundCheckCollider.offset;
        Collider2D[] collisions = Physics2D.OverlapBoxAll(groundBoxWorldPosition, this.mGroundCheckCollider.size, 0f);
        Grounded = false;
        foreach(var coll in collisions)
        {
            if (coll.tag == "Player" || coll.tag == "Ladders")
                continue;

            Grounded = true;
            break;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        SearchLadders();
        SearchMovingPlatforms();

        UpdateGroundCheck();
        UpdateInput();
        UpdateAnimator();
        UpdateGraphics();
        UpdatePhysics();

        base.Update();
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void SearchMovingPlatforms()
    {
        InMovingPlatform = null;
        foreach(MovingPlatform mp in GameManager.CurrentLevel.MovingPlatforms)
        {
            if(mp.TriggerDetector.bounds.Contains(this.mRigidBody.position))
            {
                InMovingPlatform = mp;
                break;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SearchLadders()
    {
        if (LadderClimbDownCollider != null)
        {
            Physics2D.IgnoreCollision(mCollider, LadderClimbDownCollider, false);
            LadderClimbDownCollider = null;
        }
        InLadder = null;
        ReadyToClimbUp = false;
        ReadyToClimbDown = false;

        foreach (Ladder ladder in GameManager.CurrentLevel.Ladders)
        {
            if (ladder.PrincipalTrigger.bounds.Contains(GameManager.Player.transform.position))
            {
                InLadder = ladder;
                ReadyToClimbUp = true;
                ReadyToClimbDown = true;
            }
            else if (ladder.UpTrigger.bounds.Contains(GameManager.Player.transform.position))
                ReadyToClimbDown = true;              
            else if (ladder.DownTrigger.bounds.Contains(GameManager.Player.transform.position))
                ReadyToClimbUp = true;

            // If we are on top of a ladder, search if it has a Platform Effector 2D associated, so it's deactivated if the user decides
            // to climb down
            if (ReadyToClimbDown)
            {
                var colliders = ladder.ClimbDownEffector.gameObject.GetComponents<Collider2D>().Where(collider => collider.usedByEffector == true);
                LadderClimbDownCollider = colliders.FirstOrDefault();
            }

            // Break the loop if we already found a ladder 
            if (ReadyToClimbDown || ReadyToClimbUp)
                break;
        }
    }

    #region Shots
    /// <summary>
    /// 
    /// </summary>
    private void SpawnShot()
    {
        float offsetX = mLookDir.LookLeft ? -0.5f : 0.5f;
        float offsetY = (this.State == ePlayerState.Crouch) ? 0.75f : 1.6f;
        Vector3 offset = new Vector3(offsetX, offsetY, 0);
        GameObject newObj = GameObject.Instantiate(this.PrefabShot, this.transform.position + offset, Quaternion.identity);
        LookDirection dir = newObj.GetComponent<LookDirection>();
        dir.LookLeft = this.mLookDir.LookLeft;
        PlayerShot pShot = newObj.GetComponent<PlayerShot>();
        pShot.ShotType = this.CurrentWeapon;
        mShots.Add(pShot);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pShot"></param>
    public void DestroyShot(PlayerShot pShot)
    {
        GameObject.Destroy(pShot.gameObject);
        
        if (mShots.Contains(pShot))
            mShots.Remove(pShot);
    }
    #endregion
}
