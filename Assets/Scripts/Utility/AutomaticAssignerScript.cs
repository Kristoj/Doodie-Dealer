using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutomaticAssignerScript : MonoBehaviour {
    
    public void AssignAudioFiles() {
        object[] objs = Resources.LoadAll("Audio", typeof(AudioClip));
        Database.Instance.AssignAudioFiles(ref objs);
    }

}
