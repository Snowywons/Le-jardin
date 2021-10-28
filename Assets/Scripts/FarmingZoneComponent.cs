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
        }
    }

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
}
