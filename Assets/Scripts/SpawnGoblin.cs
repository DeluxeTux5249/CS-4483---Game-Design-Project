using System.Collections;
using UnityEngine;

public class SpawnGoblin : MonoBehaviour
{
    public GameObject goblin;

    IEnumerator Start()
    {
        StartCoroutine(SingleSpawnLoop());
        StartCoroutine(WaveSpawnLoop());
        yield break;
    }

    IEnumerator SingleSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(45f);
            SpawnOneGoblin();
        }
    }

    IEnumerator WaveSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(300f);

            for (int i = 0; i < 5; i++)
            {
                SpawnOneGoblin();
            }
        }
    }

    void SpawnOneGoblin()
    {
        if (goblin == null) return;

        Vector3 pos = transform.position;
        GameObject go = Instantiate(goblin);
        go.transform.position = new Vector3(pos.x, pos.y + transform.localScale.y / 2f, pos.z);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
}