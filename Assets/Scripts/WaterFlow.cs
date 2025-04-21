using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public float scrollSpeedX = 0.1f; // Speed of texture scrolling on the X-axis
    public float scrollSpeedY = 0.05f; // Speed of texture scrolling on the Y-axis

    private Material waterMaterial; // The material of the cube

    void Start()
    {
        // Get the material from the cube's Mesh Renderer
        waterMaterial = GetComponent<MeshRenderer>().material;
        if (waterMaterial == null)
        {
            Debug.LogError("No material found on the cube!");
        }
    }

    void Update()
    {
        // Calculate the offset based on time and scroll speed
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // Apply the offset to the material's texture
        waterMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}