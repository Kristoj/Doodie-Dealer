using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundSystem {

    private static GameObject sourceRoot;
    private static int playCount = 0;

    public static void SetupSoundSystem() {
        sourceRoot = new GameObject("Sound Source Root");
        sourceRoot.transform.SetParent(Database.Instance.transform);
    }

    public static void OnAwake() {
        SetupSoundSystem();
    }

    /// <summary>
    /// Plays sound in 2D space.
    /// </summary>
    /// <param name="soundName"></param>
    public static void PlaySound2D(string soundName) {
        PlaySoundClip(soundName, 1, Vector3.zero, null, true);
    }

    /// <summary>
    /// Plays sound in 2D space with given volume.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    public static void PlaySound2D(string soundName, float volume) {
        PlaySoundClip(soundName, volume, Vector3.zero, null, true);
    }

    /// <summary>
    /// Plays sound in 3D space with given volume and position.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    public static void PlaySound(string soundName, float volume, Vector3 soundPosition) {
        PlaySoundClip(soundName, volume, soundPosition);
    }

    /// <summary>
    /// Plays sound in 3D space with given volume and position. Also parents the sound object to the given parent object.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    public static void PlaySound(string soundName, float volume, Vector3 soundPosition, Transform parentObject) {
        PlaySoundClip(soundName, volume, soundPosition, parentObject);
    }

    // Actually play the user given sound.
    static void PlaySoundClip(string soundName, float volume = 1, Vector3 soundPosition = default, Transform parentObject = null, bool is2DSound = false) {

        // NULL Reference ?
        AudioClip soundToPlay = Database.Instance.GetSoundClip(soundName);
        if (soundToPlay == null) {
            Debug.LogWarning("Vili pls... Could not find sound called: " + soundName);
            return;
        }

        // Create new gameobject that holds a Audio Source component.
        GameObject sourceObject = new GameObject("Sound Source" + playCount);
        sourceObject.transform.SetParent(sourceRoot.transform);
        AudioSource soundSource = sourceObject.AddComponent<AudioSource>();

        // Variables TODO

        // Change Sound Source variables
        soundSource.volume = volume;
        soundSource.clip = soundToPlay;

        // Play the sound
        soundSource.Play();

        // Increment play count
        playCount++;

        // Start the destroy coroutine from any monobehaviour
        Database.Instance.StartCoroutine(DestroySoundObject(sourceObject, soundToPlay.length));
    }

    // Coroutine for destroying the sound object
    static IEnumerator DestroySoundObject(GameObject objectToDestroy, float waitTime) {

        // Wait for x amount of seconds
        while (waitTime + 1 > 0) {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        // Destroy sound object
        Object.Destroy(objectToDestroy);
    }
}
