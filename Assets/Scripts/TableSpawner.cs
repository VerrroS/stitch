using UnityEngine;
using Meta.XR.MRUtilityKit;

public class TableSpawner : MonoBehaviour
{
    public GameObject chosenTablePrefab;
    public GameObject cubePrefab;
    public Transform cameraRig;
    public MRUKAnchor.SceneLabels tableLabelFilter;

    private GameObject currentChosenTable;
    private GameObject currentCube;
    private Transform currentTableTransform;


    void Start()
    {
        ChooseNearestTable();
    }

    void Update()
    {
        DetectTableInView();
    }

    void ChooseNearestTable()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        //MRUKAnchor closestTable = room.TryGetClosestSurfacePosition(room.transform.position, out Vector3 closestTablePosition, out MRUKAnchor closestTableAnchor, LabelFilter.FromEnum(tableLabelFilter));

    }

    void DetectTableInView()
    {
        Ray ray = new Ray(cameraRig.position, cameraRig.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Table") && hit.transform != currentTableTransform)
            {
                if (currentTableTransform != null)
                {
                    DishilightChosenTable(currentTableTransform);
                }
                HighlightChosenTable(hit.transform);
            }
        }
    }

    void HighlightChosenTable(Transform tableTransform)
    {
        // Destroy previous highlighted objects
        if (currentChosenTable != null)
        {
            Destroy(currentChosenTable);
        }
        if (currentCube != null)
        {
            Destroy(currentCube);
        }

        // Store the current table transform
        currentTableTransform = tableTransform;

        // Spawn and stretch the chosen table prefab
        currentChosenTable = Instantiate(chosenTablePrefab, tableTransform.position, Quaternion.identity);
        currentChosenTable.transform.localScale = new Vector3(tableTransform.localScale.x, 1, tableTransform.localScale.z); // Assuming the table has a uniform scale

        // Spawn the cube prefab 10 cm above the table's anchor
        Vector3 cubePosition = tableTransform.position + Vector3.up * 0.1f;
        currentCube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
    }

    void DishilightChosenTable(Transform oldTableTransform)
    {
        // Logic to de-highlight the old table
        // Destroy the old chosen table and cube prefabs
        if (currentChosenTable != null)
        {
            Destroy(currentChosenTable);
        }
        if (currentCube != null)
        {
            Destroy(currentCube);
        }
    }
}
