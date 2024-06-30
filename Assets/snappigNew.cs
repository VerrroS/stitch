using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snappingNew : MonoBehaviour
{
    [SerializeField] private GameObject theObjectYouWantToPlace;

    public Transform _rayOrigin;

    public GameObject test;

    public AnchorIT anchorIT;


    // Update is called once per frame
    private void Update()
    {
        if (!anchorIT.tableInitialized)
            return;

        MRUKAnchor tableAnchor = anchorIT.currentAnchor;


        var ray = GetControllerRay();
        var hit = new RaycastHit();
        MRUKAnchor anchorHit = null;
        MRUK.Instance?.GetCurrentRoom()?.Raycast(ray, Mathf.Infinity, out hit, out anchorHit);

        Debug.Log($"{tableAnchor} and {anchorHit}");
        if(tableAnchor != null && anchorHit != null && tableAnchor == anchorHit)
        {
            ShowHitNormal(hit);
            return;
        }

        if (anchorHit != null)
            Debug.Log($"\nAnchor: {anchorHit.name}\nHit point: {hit.point}\nHit normal: {hit.normal}\n");
    }

    private Ray GetControllerRay()
    {
        Vector3 rayOrigin = _rayOrigin.transform.position;
        Quaternion Rotation = _rayOrigin.transform.rotation;
        Vector3 rayDirection = Rotation * Vector3.forward;

        Debug.DrawRay(rayOrigin, rayDirection, Color.yellow);
        return new Ray(rayOrigin, rayDirection);
    }

    private void ShowHitNormal(RaycastHit hit)
    {
        if (theObjectYouWantToPlace != null && hit.point != Vector3.zero && hit.distance != 0)
        {
            theObjectYouWantToPlace.SetActive(true);
            theObjectYouWantToPlace.transform.position =
                hit.point + -theObjectYouWantToPlace.transform.up * theObjectYouWantToPlace.transform.localScale.y;
            theObjectYouWantToPlace.transform.rotation = Quaternion.FromToRotation(-Vector3.up, hit.normal);
        }
        else
        {
            theObjectYouWantToPlace.SetActive(false);
        }
    }
}