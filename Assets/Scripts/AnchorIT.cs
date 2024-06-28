using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnchorIT : MonoBehaviour
{
    public Transform playerHead;
    public EffectMesh effectMesh;


    public Transform rayStartPoint;
    public float rayLength = 10;
    public MRUKAnchor.SceneLabels labelFilter;

    public TextMeshProUGUI debugText;


    // Update is called once per frame
    void Update()
    {
        Vector3 origin = playerHead.position;
        Vector3 direction = playerHead.forward;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hasHit = room.Raycast(ray, rayLength, LabelFilter.FromEnum(labelFilter), out RaycastHit hit, out MRUKAnchor anchor);

        if (hasHit)
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            string label = anchor.AnchorLabels[0];

            debugText.transform.position = hitPoint;
            debugText.transform.rotation = Quaternion.LookRotation(hitNormal);

            debugText.text = label;

            effectMesh.CreateEffectMesh(anchor);

        }

    }
}