using UnityEngine;

public class EnemyPersistance : MonoBehaviour
{
    [Header("Use the format 'scene_number'")] 
    [SerializeField] private string enemyID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (string.IsNullOrEmpty(enemyID))
        {
            // could add default code to make all enemies persistent... not worth
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey(), 0) == 1)
        {
            // If already dead, just remove them immediately
            gameObject.SetActive(false);
        }
    }

    [ContextMenu("Set As Dead")]
    public void MarkAsDead()
    {
        if (string.IsNullOrEmpty(enemyID))
        {
            Debug.Log("This enemy has no ID, and is thus impersistent");
            return;
        }

        PlayerPrefs.SetInt(PlayerPrefsKey(), 1);
        PlayerPrefs.Save();
        Debug.Log($"{enemyID} has been marked dead in playerprefs");
    }

    [ContextMenu("Reset Death State")]
    public void ResetState()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsKey());
        Debug.Log($"{enemyID} state reset.");
    }


    private string PlayerPrefsKey()
    {
        return $"Enemy_{enemyID}_IsDead";
    }
}
