#pragma warning disable 0649
using UnityEngine;

public class DookerDefecation : MonoBehaviour {

    [Header("Defecation Stats")]
    [SerializeField] private float defecationMinThreshold = 30f;
    [SerializeField] private float doodieSpawnRate = 5f;
    [SerializeField] private float stomachWasteReduction = 30f;

    [Header("Doodie Spawning")]
    [SerializeField] private Vector3 doodieSpawnPosition = new Vector3(0, .5f, -.25f);
    [SerializeField] private float doodieSpawnForce = 5f;

    // Classes
    private DookerStats stats;

    void Awake() {
        SetupDependencies();
    }

    void Update() {
        NeedToDefecate++;
    }

    public void Defecate() {
        Item clone = Instantiate(Database.Instance.GetItem(ItemName.Doodie_Normal),                                     // Spawn doodie
                        transform.position + doodieSpawnPosition, Quaternion.identity);     
        clone.GetComponent<Rigidbody>().AddForce(-transform.forward * doodieSpawnForce);                                // Add force to the rigidbody
        stats.StomachWaste -= stomachWasteReduction;
        NeedToDefecate = 0;
    }

    void SetupDependencies() {
        stats = GetComponent<DookerStats>();
    }


    [SerializeField] private float _needToDefecate;
    private float NeedToDefecate {
        get {
            return _needToDefecate;
        }
        set {
            if (value == 0)
                _needToDefecate = 0;
            _needToDefecate += doodieSpawnRate * Time.deltaTime;
            _needToDefecate = Mathf.Clamp(_needToDefecate, 0, 100);
        }
    }

    public bool CanDefecate {
        get {
            // False if we haven't waited long enough to defecate again
            if (NeedToDefecate < 100) {
                return false;
            }
            // False if we haven't enough stomach waste
            else if (stats.StomachWaste <= defecationMinThreshold) {
                return false;
            }
            return true;    // Otherwise return true
        }
    }
}
