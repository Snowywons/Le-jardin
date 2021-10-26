using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingZoneComponent : MonoBehaviour
{
    [SerializeField] List<GameObject> zones;
    [SerializeField] float desiredHeight;

    private void Start()
    {
        for (int i = 0; i < GameSystem.Instance.farmableZoneCount; i++)
        {
            if (zones != null)
            {
                StartCoroutine(SetDesiredHeight(zones[i]));

                foreach (Transform child in zones[i].transform)
                {
                    if (null == child) continue;

                    ParticleSystem ps = child.gameObject.GetComponent<ParticleSystem>();

                    if (ps != null)
                        ps.gameObject.SetActive(true);
                }
            }
            //SetDesiredHeight(zones[i]);
        }
    }

    //public void Claim(int id)
    //{
    //    if (zones != null)
    //    {
    //        GameObject zone = zones[id];
    //        SetDesiredHeight(zone);
    //        SetZoneTiles(zone);
    //    }
    //}

    private IEnumerator SetDesiredHeight(GameObject zone)
    {
        float time = 0;
        Vector3 oldPos = zone.transform.localPosition;
        Vector3 newPos = oldPos;
        newPos.y = desiredHeight;

        while (time < 1f)
        {
            zone.transform.localPosition = Vector3.Lerp(oldPos, newPos, time);
            time += Time.deltaTime * 2.5f;
            yield return null;
        }

        zone.transform.localPosition = newPos;
    }

    //private void SetDesiredHeight(GameObject zone)
    //{
    //    Vector3 newPos = zone.transform.localPosition;
    //    newPos.y = desiredHeight;
    //    zone.transform.localPosition = newPos;
    //}

    // Récursivité pour accéder à tous les enfants d'une zone
    //private void SetZoneTiles(GameObject obj)
    //{
    //    if (null == obj) return;

    //    foreach (Transform child in obj.transform)
    //    {
    //        if (null == child) continue;

    //        TileComponent tile = child.gameObject.GetComponent<TileComponent>();

    //        if (tile != null)
    //            tile.isFarmable = true;

    //        SetZoneTiles(child.gameObject);
    //    }
    //}
}
