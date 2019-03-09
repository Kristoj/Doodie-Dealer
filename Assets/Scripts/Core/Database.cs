#pragma warning disable 0649
using UnityEngine;

public enum ItemName {
    Error_Item              = 0,
    Doodie_Normal           = 1,
    Doodie_Legendary        = 999
}

public class Database : MonoBehaviour {

    [SerializeField] private Item[] items;
    [SerializeField] private AudioClip[] sounds;

    // Singleton self reference
    private static Database pm_database;
    public static Database Instance {
        get {
            if (pm_database == null)
                pm_database = FindObjectOfType<Database>();
            return pm_database;
        }
        set {
            pm_database = value;
        }
    }

    void Awake() {
        // TODO We need to call init functions somewhere else
        // It isn't Databases job to do this...
        SoundSystem.SetupSoundSystem();
        gameObject.AddComponent<SceneBootStrapper>();
    }

    public Item GetItem(ItemName itemName) {
        return items[(((int)itemName))];
    }

    #region Audio
    public void AssignAudioFiles(ref object[] objs) {
        sounds = new AudioClip[objs.Length];
        for (int i = 0; i < objs.Length; i++) {
            sounds[i] = (objs[i] as AudioClip);
        }
        Debug.Log("Succesfully Assigned " + sounds.Length + " Sound clips.");
    }

    public AudioClip GetSoundClip(string soundName) {
        AudioClip sound = null;
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == soundName) {
                sound = sounds[i];
                break;
            }
        }

        // Display error message if we couldn't find the sound clip
        if (sound == null) {
            // Scan audio files
            GetComponent<AutomaticAssignerScript>().AssignAudioFiles();
            for (int i = 0; i < sounds.Length; i++) {
                if (sounds[i].name == soundName) {
                    sound = sounds[i];
                    break;
                }
            }

            if (sound != null)
                Debug.LogWarning("Vili pls... Audio was located after scanning.\n Please scan audio files in the editor when you leave Play Mode.");
        }

        return sound;
    }
    #endregion

}
