using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LookDirection))]
[RequireComponent(typeof(SpriteRenderer))]
public class GameElement : MonoBehaviour
{
    /// <summary>
    /// Number of PlayerShot hits to make this game element die
    /// </summary>
    public int NumberOfHitsToDie = 1;
    /// <summary>
    /// Destroy automatically when out of screen
    /// </summary>
    public bool DestroyWhenNotVisible = false;
    /// <summary>
    /// Makes this game element constantly look at the player
    /// </summary>
    public bool AlwaysLookToPlayer = false;
    /// <summary>
    /// Horizontal speed, in m/s
    /// </summary>
    public float SpeedX = 5f;
    /// <summary>
    /// Score given to the player when this game element dies
    /// </summary>
    public int ScoreWhenDied = 100;

    [HideInInspector]
    public Rigidbody2D mRigidBody;
    protected SpriteRenderer mRender;
    protected Animator mAnimator;
    protected LookDirection mLookDir;

    public bool IsVisible
    {
        get { return this.mRender.isVisible; }
    }
    public bool HasBeenVisible;

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake()
    {
        mRigidBody = this.GetComponent<Rigidbody2D>();
        mRender = this.GetComponent<SpriteRenderer>();
        mAnimator = this.GetComponent<Animator>();
        mLookDir = this.GetComponent<LookDirection>();
    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Update()
    {
        // Always look at player
        if (AlwaysLookToPlayer)
            mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;

        if (IsVisible)
            HasBeenVisible = true;

        // If out of screen, detroy it
        if (DestroyWhenNotVisible && !IsVisible && HasBeenVisible)
            this.Destroy();
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void HitByPlayerShot()
    {
        // Check if the element should die
        NumberOfHitsToDie--;
        if (NumberOfHitsToDie <= 0)
        {
            this.Destroy();
            GameManager.Player.Score += ScoreWhenDied;
        }
    }
}
