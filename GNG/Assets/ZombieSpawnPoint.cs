using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    private AudioSource mAudioSource;
    public GameObject PrefabZombie;
    public float SpawnPeriodSecs = 10;
    private float mTimeToNextSpawn = 0;
    public float ActivationThresholdM = 30;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        mAudioSource = this.GetComponent<AudioSource>();
    }
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Randomly choose an initial spawn period time
        mTimeToNextSpawn = Random.Range(1f, 4f);
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        float distToPlayer = Mathf.Abs(GameManager.DistanceToPlayerInX(this.transform));
        if (distToPlayer < ActivationThresholdM)
        {
            mTimeToNextSpawn -= Time.deltaTime;
            if (mTimeToNextSpawn <= 0)
            {
                SpawnZombie(distToPlayer < 8);

                // Randomly choose next spawn period
                mTimeToNextSpawn = SpawnPeriodSecs + Random.Range(-5f, 1f);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SpawnZombie(bool pPlaySound)
    {
        GameObject newObj = GameObject.Instantiate(this.PrefabZombie, this.transform.position, Quaternion.identity);

        // Randomly choose pickup type, with a 15% chance of having a price at all
        if (Random.Range(0, 100) <= 15)
            newObj.GetComponent<Zombie>().PickupType = (ePickupType)Random.Range(0, 6);

        // Make the Zombie look to the player
        LookDirection dir = newObj.GetComponent<LookDirection>();
        dir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;

        if (pPlaySound)
            mAudioSource.Play();
    }
}
