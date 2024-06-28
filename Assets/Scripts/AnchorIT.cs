using Meta.WitAi;
using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static FindSpawnPositions;
using static Meta.XR.MRUtilityKit.EffectMesh;
using static OVRPlugin;

public class AnchorIT : MonoBehaviour
{
    public Transform playerHead;
    public EffectMesh effectMesh;
    public GameObject chooseTableButton;



    public Transform rayStartPoint;
    public float rayLength = 10;
    public MRUKAnchor.SceneLabels labelFilter;
    private GameObject tShirtinstance = null;



    EffectMeshObject efMesh= null;
    MRUKAnchor currentAnchor = null;
    MRUKRoom room = null;

    private bool tableInitialized = false;


    public GameObject SpawnObject;
    // Update is called once per frame

    public void Start()
    {
        room = MRUK.Instance.GetCurrentRoom();
    }
    void Update()
    {
        if (tableInitialized)
        {
            return;
        }
        if (room == null)
        {
            room = MRUK.Instance.GetCurrentRoom();
            Debug.Log("searching room");
            return;
        }

        Vector3 origin = playerHead.position;
        Vector3 direction = playerHead.forward;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
    
        bool hasHit = room.Raycast(ray, rayLength, LabelFilter.FromEnum(labelFilter), out RaycastHit hit, out MRUKAnchor newAnchor);
        if (hasHit)
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            string label = newAnchor.AnchorLabels[0];


            HighlightCurrentTable(newAnchor, currentAnchor);
        }
    }

    private void HighlightCurrentTable(MRUKAnchor newAnchor, MRUKAnchor curAnchor)
    {
        if (curAnchor != null && curAnchor != newAnchor) // Removes current anchor
        {
            effectMesh.DestroyMesh(curAnchor);

            Debug.Log("Destroying current anchor");
        }

        if (currentAnchor != newAnchor)
        {
            if (tShirtinstance != null)
            {
                Destroy(tShirtinstance); // Destroy the previous instance if it exists
            }

            Debug.Log("Instantiating");
            effectMesh.CreateEffectMesh(newAnchor);

            var spawnPosition = newAnchor.transform.position;
            var spawnRotation = newAnchor.transform.rotation * Quaternion.Euler(-90, 0, 0);

            tShirtinstance = Instantiate(SpawnObject, spawnPosition, spawnRotation, newAnchor.transform);
            currentAnchor = newAnchor;
        }
    }



}