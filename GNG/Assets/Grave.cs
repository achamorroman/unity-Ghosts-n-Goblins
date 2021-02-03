using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    /// <summary>
    /// Audio clip to be played when the grave is hit by a Player Shot
    /// </summary>
    public AudioClip AudioHit;
    /// <summary>
    /// Number of shots neede to show the dragon that throws the Frog Spell
    /// </summary>
    public int NumShotsToShowDragon = 10;

    private AudioSource mAudioSource;

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
    public void HitByPlayerShot()
    {
        // Check if we need to Spawn the dragon
        NumShotsToShowDragon--;
        if(NumShotsToShowDragon == 0)
            GameManager.CurrentLevel.SpawnDragon(this.transform.position);

        // Play hit audio clip
        mAudioSource.clip = this.AudioHit;
        mAudioSource.Play();
    }
}
