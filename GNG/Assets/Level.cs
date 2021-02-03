using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    /// <summary>
    /// Allowed time to complete the level, in seconds
    /// </summary>
    public float TimeRemaining = 120;

    [Header("Spawn Prefabs")]
    public GameObject PrefabDeathFire;
    public GameObject PrefabVanish;
    public GameObject PrefabSplash;
    public GameObject PrefabDragon;
    public GameObject PrefabDragonShot;
    public GameObject PrefabFlyingGhostShot;
    public GameObject PrefabPickup;
    public GameObject PrefabTreasureBox;

    [HideInInspector]
    public List<Ladder> Ladders = new List<Ladder>();
    [HideInInspector]
    public List<MovingPlatform> MovingPlatforms = new List<MovingPlatform>();

    private AudioSource mAudioSource;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        // Update the global reference of GameManager to point to this level
        GameManager.CurrentLevel = this;

        mAudioSource = this.GetComponent<AudioSource>();

        // Clean the collections. WARNING: This clear should be done BEFORE the ladders/platforms start registering themselves in this collection
        Ladders.Clear();
        MovingPlatforms.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        // Update time remaining and kill player if it reaches zero
        this.TimeRemaining -= Time.deltaTime;
        if (TimeRemaining < 0)
            GameManager.Player.DieArthurDie();
    }
    /// <summary>
    /// 
    /// </summary>
    public void ResetLevel()
    {
        this.StopBackgroundMusic();
        mAudioSource.time = 0;
        this.PlayBackgroundMusic();
    }
    /// <summary>
    /// 
    /// </summary>
    public void StopBackgroundMusic()
    {
        mAudioSource.Stop();
    }
    /// <summary>
    /// 
    /// </summary>
    public void PlayBackgroundMusic()
    {
        mAudioSource.Play();
    }
    /// <summary>
    /// Returns the remaining time as a mm::ss formatted string 
    /// </summary>
    /// <returns></returns>
    public string GetRemainingTimeFormatted()
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(TimeRemaining);
        return time.ToString(@"mm\:ss");
    }

    #region Dynamically Spawn Stuff
    /// <summary>
    /// 
    /// </summary>
    public void SpawnFxDeathFire(Vector3 pPosition)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabDeathFire, pPosition, Quaternion.identity);
    }
    /// <summary>
    /// 
    /// </summary>
    public void SpawnFxVanish(Vector3 pPosition)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabVanish, pPosition, Quaternion.identity);
    }
    /// <summary>
    /// 
    /// </summary>
    public void SpawnFxSplash(Vector3 pPosition)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabSplash, pPosition, Quaternion.identity);
    }
    /// <summary>
    /// 
    /// </summary>
    public void SpawnDragon(Vector3 pPosition)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabDragon, pPosition, Quaternion.identity);
    }
    /// <summary>
    /// 
    /// </summary>
    public void SpawnDragonShot(Vector3 pPosition, bool pLookLeft)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabDragonShot, pPosition, Quaternion.identity);
        newGO.GetComponent<LookDirection>().LookLeft = pLookLeft;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPosition"></param>
    public void SpawnFlyingGhostShot(Vector3 pPosition)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabFlyingGhostShot, pPosition, Quaternion.identity);
        newGO.GetComponent<LookDirection>().LookLeft = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pType"></param>
    public void SpawnPickUp(ePickupType pType, Vector3 pPos)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabPickup, pPos, Quaternion.identity);
        newGO.GetComponent<Pickup>().Type = pType;
        newGO.transform.position = pPos;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="pPos"></param>
    public void SpawnTreasureBox(ePickupType pType, Vector3 pPos)
    {
        GameObject newGO = GameObject.Instantiate(this.PrefabTreasureBox, pPos, Quaternion.identity);
        newGO.GetComponent<TreasureBox>().Type = pType;
        newGO.transform.position = pPos;
    }
    #endregion

}
