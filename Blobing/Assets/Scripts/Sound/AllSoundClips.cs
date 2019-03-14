using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundReferences", menuName = "SoundReferences", order = 1)]
public class AllSoundClips : ScriptableObject {
	[Header("Music")]
	public AudioClip gameMusic;

    [Header("Player")]
    public AudioClip iceSkating;
    public AudioClip playerDeath;

    [Header("Game")]
    public AudioClip gainPoint;
    public AudioClip startGame;
    public AudioClip gameOver;

    [Header("Ice Cutting")]
    public AudioClip plouf;
    public AudioClip iceCracking;
    public AudioClip trailLengthUp;
    public AudioClip penguinCry;

	
}
