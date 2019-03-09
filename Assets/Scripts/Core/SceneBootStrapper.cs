using UnityEngine;
using UnityEngine.UI;

public class SceneBootStrapper : MonoBehaviour {

    public static Canvas WS_Canvas { get; set; }

    void Start(){
        FindReferences();    
    }

    void FindReferences() {
        // WS_Canvas
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).name == "UI") {
                Transform t = transform.GetChild(i);
                for (int j = 0; j < t.childCount; j++) {
                    if (t.GetChild(j).name == "WS_Canvas") {
                        WS_Canvas = t.GetChild(j).GetComponent<Canvas>();
                    }
                }
            }
        }
    }

}
