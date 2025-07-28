using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloudsManager : MonoBehaviour
{
    public List<StormCloudsBehaviour> stormClouds;
    public bool isStormCloud;

    public int count;

    public void SpawnStormClouds(float destination, float duration)
    {
        count = stormClouds.Count;
        float delayFactor = 0.3f;
        float offsetFactor = 0.15f;

        transform.DOMoveY(destination, duration);

        //stormClouds[0].gameObject.SetActive(true);

        for (int cloud = 0; cloud <= stormClouds.Count-1; cloud++)
        {
            stormClouds[cloud].gameObject.SetActive(true);
            stormClouds[cloud].transform.DOMoveY(destination - (destination * (1+ offsetFactor*cloud)-destination), duration * (1 + delayFactor * cloud));
            offsetFactor -= offsetFactor/5f;
            //delayFactor += delayFactor;*/
        }
    }

    public void DespawnStormClouds(float duration)
    {

        for (int cloud = stormClouds.Count - 1; cloud >= 0 ; cloud--)
        {
            stormClouds[cloud].notEnding = false;
            stormClouds[cloud].StartCoroutine(stormClouds[cloud].DespawnMovement(duration));
            duration += 0.2f;
        }

        StartCoroutine(DestroyStormClouds(duration));
    }

    public IEnumerator DestroyStormClouds(float duration)
    {
        float time = 0;
        while(time <= duration)
        {
            time += Time.deltaTime;
        }
        Destroy(this);

        yield return null;
    }


}
