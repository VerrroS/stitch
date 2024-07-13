using System.Collections.Generic;
using UnityEngine;

public class CubeArrangement : MonoBehaviour
{
    public List<GameObject> cubes = new List<GameObject>(); // List of cubes to be arranged
    public int maxIterations = 10; // Maximum number of iterations
    public BoxCollider mainCube; // Main cube to arrange the cubes
    public float bufferZone = 0.1f; // Buffer zone to ensure cubes are within bounds and spaced apart

    void Start()
    {
        if (mainCube == null)
        {
            Debug.LogError("Main cube is not assigned.");
            return;
        }

        HideCubesFromList();
        ArrangeCubes(mainCube);
    }

    public void HideCubesFromList()
    {
        foreach (GameObject cube in cubes)
        {
            cube.SetActive(false);
        }
    }

    public void ShowCubesFromList()
    {
        foreach (GameObject cube in cubes)
        {
            cube.SetActive(true);
        }
    }

    void ArrangeCubes(BoxCollider mainCube)
    {
        // Size of the main cube
        Vector3 mainCubeSize = mainCube.bounds.size;
        Vector3 mainCubeMin = mainCube.bounds.min + new Vector3(bufferZone, bufferZone, bufferZone);
        Vector3 mainCubeMax = mainCube.bounds.max - new Vector3(bufferZone, bufferZone, bufferZone);

        // Start position for placing the cubes
        Vector3 startPos = mainCubeMin;
        startPos.y = mainCube.bounds.max.y + bufferZone; // Ensure cubes are placed on top of the main cube

        if (!ArrangeCubesOnSurface(startPos, mainCubeMin, mainCubeMax))
        {
            Debug.LogWarning("Failed to arrange cubes within the main cube. Using the last arrangement.");
        }

        ShowCubesFromList();
    }

    bool ArrangeCubesOnSurface(Vector3 startPos, Vector3 mainCubeMin, Vector3 mainCubeMax)
    {
        List<Bounds> placedCubeBounds = new List<Bounds>();

        float currentX = startPos.x;
        float currentZ = startPos.z;

        foreach (GameObject cube in cubes)
        {
            Vector3 cubeSize = cube.GetComponent<Collider>().bounds.size;
            Bounds cubeBounds = new Bounds(new Vector3(currentX + cubeSize.x / 2, startPos.y - cubeSize.y / 2, currentZ + cubeSize.z / 2), cubeSize);

            // Check if the cube can fit in the remaining space with buffer zone and if it is within the bounds of the main cube
            if (currentX + cubeSize.x + bufferZone > mainCubeMax.x ||
                currentZ + cubeSize.z + bufferZone > mainCubeMax.z ||
                IsOverlapping(placedCubeBounds, cubeBounds))
            {
                // If it doesn't fit, reset currentX and move to the next row
                currentX = startPos.x;
                currentZ += cubeSize.z + bufferZone;

                // Check if the new row position is still within bounds
                if (currentZ + cubeSize.z + bufferZone > mainCubeMax.z)
                {
                    return false;
                }

                // Update bounds for the new row
                cubeBounds = new Bounds(new Vector3(currentX + cubeSize.x / 2, startPos.y - cubeSize.y / 2, currentZ + cubeSize.z / 2), cubeSize);
            }

            // Place the cube
            cube.transform.position = cubeBounds.center;
            cube.transform.rotation = mainCube.transform.rotation;
            placedCubeBounds.Add(cubeBounds);

            // Update the current position for the next cube, including buffer zone
            currentX += cubeSize.x + bufferZone;
        }

        return true;
    }

    bool IsOverlapping(List<Bounds> placedCubeBounds, Bounds newCubeBounds)
    {
        foreach (var bounds in placedCubeBounds)
        {
            if (bounds.Intersects(newCubeBounds))
            {
                return true;
            }
        }
        return false;
    }
}
