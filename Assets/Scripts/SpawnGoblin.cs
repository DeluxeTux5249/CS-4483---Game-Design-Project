using System.Collections;
using UnityEngine;

public class SpawnGoblin : MonoBehaviour
{
    public GameObject goblin_builing;
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

            for (int i = 0; i < 2; i++)
            {
                SpawnOneGoblin();
            }
            for (int i = 0; i < 3; i++)
            {
                SpawnGoblinToAttackBuilding();
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

    void SpawnGoblinToAttackBuilding()
    {
        if (goblin_builing == null) return;
        Vector3 pos = transform.position;
        GameObject go = Instantiate(goblin_builing);
        go.transform.position = new Vector3(pos.x, pos.y + transform.localScale.y / 2f, pos.z);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
}