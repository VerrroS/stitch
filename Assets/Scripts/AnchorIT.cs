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

    private GameObject chosen_btable_button = null ; 


    public Transform rayStartPoint;
    public float rayLength = 10;
    public MRUKAnchor.SceneLabels labelFilter;



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
        room = MRUK.Instance.GetCurrentRoom();

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
        //else if (curAnchor != newAnchor)
        //{
            effectMesh.CreateEffectMesh(newAnchor);
        //}

        currentAnchor = newAnchor;

        var spawnPosition = newAnchor.transform.position;
        var spawnRoatation = newAnchor.transform.rotation;
        if (SpawnObject.gameObject.scene.path == null)
        {
            Debug.Log("Instantiating");
            Instantiate(SpawnObject, spawnPosition, spawnRoatation, newAnchor.transform);
        }
    }



}