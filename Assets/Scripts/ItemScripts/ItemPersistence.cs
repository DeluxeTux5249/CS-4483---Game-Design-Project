using UnityEngine;

// yes I copied this whole ass class with no subclassing, and no I don't care
// if you wanted best practices... we should've started this earlier lmao
public class ItemPersistance : MonoBehaviour
{
    [Header("Use the format 'scene_number'")]
    [SerializeField] private string itemID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            // any triggered code if unnamed
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey(), 0) == 1)
        {
            // If already collected, just remove them immediately
            gameObject.SetActive(false);
        }
    }

    [ContextMenu("Set Collected")]
    public void MarkAsCollected()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            Debug.Log("This item has no ID, and isn't saved");
            return;
        }

        PlayerPrefs.SetInt(PlayerPrefsKey(), 1);
        PlayerPrefs.Save();
        Debug.Log($"{itemID} has been saved in playerprefs");
    }

    [ContextMenu("Set Uncollected")]
    public void ResetState()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsKey());
        Debug.Log($"{itemID} state reset.");
    }


    private string PlayerPrefsKey()
    {
        return $"Item_{itemID}_IsCollected";
    }
}