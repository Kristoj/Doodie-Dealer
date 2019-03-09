using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text moneyAmount;
    public Text interactionMsg;
    private GameObject cardObject;
    private Coroutine cardUpdateCoroutine;

    void Awake() {
        Setup();
    }

    void Setup() {
        try{
            moneyAmount.text = "Money: ";
            interactionMsg.text = ""; 
        }
        catch{
            Debug.LogWarning("Assign UI texts!");
        }
    }

    public void CreateUICard(UICard card) {
        // Destroy existing card if one exists
        if (cardObject != null) {
            DestroyUICard();
        }
        try {
            cardObject = Instantiate(card.cardPrefab, card.transform.position, card.transform.rotation, SceneBootStrapper.WS_Canvas.transform);
            if (cardUpdateCoroutine != null) {
                StopCoroutine(cardUpdateCoroutine);
            }
            cardUpdateCoroutine = StartCoroutine(UpdateCardOrientation());
        }
        catch {
            Debug.LogWarning(card.gameObject.name + " UICard doesn't have card prefab assigned or WS_Canvas was NULL.");
        }
    }

    IEnumerator UpdateCardOrientation() {
        while (cardObject != null) {
            cardObject.transform.LookAt(GameManager.LocalPlayer.Player_Camera.head);
            yield return null;
        }
    }

    public void DestroyUICard() {
        if (cardObject != null)
            Destroy(cardObject);
        // Stop card update coroutine
        if (cardUpdateCoroutine != null) {
            StopCoroutine(cardUpdateCoroutine);
        }
    }
}
