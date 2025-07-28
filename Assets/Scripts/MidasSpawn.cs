using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MidasSpawn : MonoBehaviour
{
    private List<Transform> _midasCoinsSpawnList;

    public List<Transform> MidasCoinsSpawnList { get { return _midasCoinsSpawnList; } }

    // Start is called before the first frame update
    void Start()
    {
        _midasCoinsSpawnList = new List<Transform>(GetComponentsInChildren<Transform>().ToList<Transform>());
    }

    public void SpawnMidasCoins(Coin coin, GameObject parent)
    {
        StartCoroutine(GoldSpawning(coin,parent));
        //StopAllCoroutines();
    }

    private IEnumerator GoldSpawning(Coin coin, GameObject parent)
    {
        yield return new WaitForSeconds(0.3f);

        _midasCoinsSpawnList.ForEach(spawnPoint =>
        {
            var gold =Instantiate(coin, spawnPoint.position, Quaternion.identity, parent.transform);
            gold.transform.parent = null;
        });
    }
}
