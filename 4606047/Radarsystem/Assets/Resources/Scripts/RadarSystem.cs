using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarSystem : MonoBehaviour
{
    [Header("Player & UI References")]
    // The player’s transform; the radar's "up" is defined by the player's forward direction.
    public Transform player;
    // The UI element (a RectTransform) that represents the radar background.
    public RectTransform radarUI;

    [Header("Radar Settings")]
    // Maximum distance (in world units) that will be displayed.
    public float maxRadarDistance = 1000f;
    // The radius (in pixels) of the radar UI.
    public float radarRadius = 200f;

    [Header("Radar Icon")]
    // Prefab for the radar icon – must have an Image and a RadarUIItem component.
    public GameObject radarIconPrefab;

    // List of radar UI items (each representing a contact).
    private List<RadarUIItem> radarItems = new List<RadarUIItem>();

    void Start()
    {
        // Find all game objects in the scene with the RadarContact script.
        RadarContact[] contacts = GameObject.FindObjectsOfType<RadarContact>();
        foreach (RadarContact contact in contacts)
        {
            // Create an icon for each contact and parent it to the radar UI.
            GameObject icon = Instantiate(radarIconPrefab, radarUI, false);

            // Get (or add) the helper component that links to the world contact.
            RadarUIItem item = icon.GetComponent<RadarUIItem>();
            if (item == null)
            {
                item = icon.AddComponent<RadarUIItem>();
            }
            item.contactTransform = contact.transform;

            // If the RadarContact has a custom icon, apply it to the Image.
            Image img = icon.GetComponent<Image>();
            if (img != null && contact.radarIcon != null)
            {
                img.sprite = contact.radarIcon;
            }

            radarItems.Add(item);
        }
    }

    void Update()
    {
        // Update each radar icon's position every frame.
        foreach (RadarUIItem item in radarItems)
        {
            if (item.contactTransform == null)
                continue;

            // Calculate the world-space offset from the player (ignoring vertical).
            Vector3 offset = item.contactTransform.position - player.position;
            Vector2 offset2D = new Vector2(offset.x, offset.z);

            // Clamp the distance so that icons do not move off the radar.
            float distance = offset2D.magnitude;
            if (distance > maxRadarDistance)
            {
                offset2D = offset2D.normalized * maxRadarDistance;
                distance = maxRadarDistance;
            }

            // Determine the angle relative to the player’s forward direction.
            Vector3 playerForward = player.forward;
            Vector2 playerForward2D = new Vector2(playerForward.x, playerForward.z).normalized;
            float angle = Vector2.SignedAngle(playerForward2D, offset2D);
            float rad = angle * Mathf.Deg2Rad;

            // Map the distance to the radar's radius.
            float normalizedDistance = distance / maxRadarDistance;
            float iconDistance = normalizedDistance * radarRadius;

            // Calculate the position on the radar: direct up (0°) corresponds to the player's forward.
            Vector2 iconPos = new Vector2(
                iconDistance * Mathf.Sin(rad),
                iconDistance * Mathf.Cos(rad)
            );

            // Update the UI icon's anchored position.
            item.GetComponent<RectTransform>().anchoredPosition = iconPos;
        }
    }
}
