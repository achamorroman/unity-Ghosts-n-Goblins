using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : GameElement
{
    /// <summary>
    /// Shots period, in seconds
    /// </summary>
    public float ShotsPeriodSecs = 4f;

    [Header("Audio Clips")]
    public AudioClip AudioDeath;
    public AudioClip AudioShot;

    /// <summary>
    /// Prefab to instantiate when a new PlantShot should be thrown
    /// </summary>
    public GameObject PlantShotPrefab;

    private float mTimeToNextShot;

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        mTimeToNextShot = ShotsPeriodSecs;
        base.Start();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {       
        // Plants only active when visible
        if(mRender.isVisible)
        {
            // Check if it's time to throw a new shot
            mTimeToNextShot -= Time.deltaTime;
            if(mTimeToNextShot <= 0)
            {
                SpawnPlantShot();
                mTimeToNextShot = ShotsPeriodSecs;
            }

        }

        base.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    private void SpawnPlantShot()
    {
        GameObject newObj = GameObject.Instantiate(PlantShotPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        PlantShot shot = newObj.GetComponent<PlantShot>();
     
        // Calculate shot direction towards the Player
        shot.Direction = (GameManager.Player.transform.position + new Vector3(0, 1, 0) - this.transform.position).normalized;

        AudioSource.PlayClipAtPoint(AudioShot, this.transform.position, 0.5f);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if plant hits the player
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.HitByPlant();
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

        AudioSource.PlayClipAtPoint(AudioDeath, this.transform.position, 0.5f);
    }
}
