using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.05f;

    private Material waterMaterial;

    void Start()
    {
        waterMaterial = GetComponent<MeshRenderer>().material;
        if (waterMaterial == null)
        {
            return;
        }
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        waterMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}