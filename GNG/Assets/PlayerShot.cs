using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : GameElement
{
    public eWeapon ShotType = eWeapon.Spear;
    
    [Header("Sprites")]
    public Sprite Dagger;
    public Sprite Spear;
    public Sprite Torch;
    public Sprite Axe;
    public Sprite Shield;


    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        this.DestroyWhenNotVisible = true;
        base.Awake();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Change properties of shot based on shot type
        switch (this.ShotType)
        {
            case eWeapon.Axe:
                this.mRender.sprite = this.Axe;
                this.mRigidBody.gravityScale = 16;
                
                // This type of shots use physics and gravity, so add a force to literally throw it away
                mRigidBody.AddForce(new Vector2((mLookDir.LookLeft ? -1 : 1) * 1000, 1000));
                break;
            case eWeapon.Dagger:
                this.mRender.sprite = this.Dagger;
                this.SpeedX = 40;
                this.mRigidBody.gravityScale = 0;
                break;
            case eWeapon.Shield:
                this.mRender.sprite = this.Shield;
                this.SpeedX = 25;
                this.mRigidBody.gravityScale = 0;
                break;
            case eWeapon.Spear:
                this.mRender.sprite = this.Spear;
                this.SpeedX = 25;
                this.mRigidBody.gravityScale = 0;
                break;
            case eWeapon.Torch:
                this.mRender.sprite = this.Torch;
                this.mRigidBody.gravityScale = 16;

                // This type of shots use physics and gravity, so add a force to literally throw it away
                mRigidBody.AddForce(new Vector2((mLookDir.LookLeft ? -1 : 1) * 1000, 1000));
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        // Update rigid body velociy for those type of shots without physics (torch and axe are controlled by the physics engine)
        if (this.ShotType == eWeapon.Dagger || this.ShotType == eWeapon.Shield || this.ShotType == eWeapon.Spear)
            this.mRigidBody.velocity = new Vector2((mLookDir.LookLeft ? -1 : 1) * SpeedX, 0f);

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hit any game element
        GameElement element = collision.collider.GetComponent<GameElement>();
        if (element != null)
            element.HitByPlayerShot();
        else
        {
            // Check if hit a grave
            Grave grv = collision.collider.GetComponent<Grave>();
            if (grv != null)
                grv.HitByPlayerShot();
        }

        // Check if hit a treasure box
        TreasureBox treasure = collision.collider.GetComponent<TreasureBox>();
        if (treasure != null)
            treasure.Destroy();

        // Always destroy the shot when hits something
        this.Destroy();
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Destroy()
    {
        GameManager.Player.DestroyShot(this);
        GameManager.CurrentLevel.SpawnFxVanish(this.transform.position + new Vector3(mLookDir.LookLeft? -1 : 1, 0, 0));

        // Intentionally avoiding to call the base class, as we override this behavior.
        // base.Destroy();        
    }
}
