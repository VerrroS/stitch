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
    public GameObject GameBoxInfoGuide;



    public Transform rayStartPoint;
    public float rayLength = 10;
    public MRUKAnchor.SceneLabels labelFilter;
    private GameObject tShirtinstance = null;
    public UiCollider uiCollider;


    EffectMeshObject efMesh= null;
    public MRUKAnchor currentAnchor = null;
    MRUKRoom room = null;

    public bool tableInitialized = false;


    private UiCollider curUiCol;

    public GameObject tableUi;
    public GameObject tableWorkspace;
    // Update is called once per frame

    public void Start()
    {
        



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
                curUiCol.placeWorkspace.RemoveListener(OnPlaceWorkspace);
                Destroy(tShirtinstance); // Destroy the previous instance if it exists
            }

            Debug.Log("Instantiating");
            effectMesh.CreateEffectMesh(newAnchor);
            GameBoxInfoGuide.GetComponent<GuideBoxController>().ShowMessage("Take out the pattern pieces from the shirt and lay them on your desk.");


            var spawnPosition = newAnchor.transform.position;
            var spawnRotation = newAnchor.transform.rotation * Quaternion.Euler(-90, 0, 0);
            
            tShirtinstance = Instantiate(tableUi, spawnPosition, spawnRotation, newAnchor.transform);
            curUiCol = tShirtinstance.GetComponentInChildren<UiCollider>();
            curUiCol.placeWorkspace.AddListener(OnPlaceWorkspace);
            Debug.Log("uiCol found?" + curUiCol);

            currentAnchor = newAnchor;
        }
    }

    private void OnPlaceWorkspace()
    {
        Debug.Log("Placing workspace");

        curUiCol.placeWorkspace.RemoveListener(OnPlaceWorkspace);

        Vector3 offset = currentAnchor.transform.right * -1.3f + currentAnchor.transform.forward * 0.3f; // Move left and forward

        var spawnPosition = currentAnchor.transform.position + offset;
        var spawnRotation = currentAnchor.transform.rotation;
        Destroy(tShirtinstance);
        tShirtinstance = Instantiate(tableWorkspace, spawnPosition, spawnRotation * Quaternion.Euler(0, 90, 90), currentAnchor.transform);




        GameObject go = currentAnchor.gameObject.transform.GetChild(0).gameObject;
        Debug.Log("Child found?" + go.name);
        go.GetComponent<MeshRenderer>().enabled = false;
        var col = go.AddComponent<BoxCollider>();
        col.isTrigger = true;
        go.AddComponent<Snapping>();

        //effectMesh.DestroyMesh(currentAnchor);
        tableInitialized = true;
    }
}