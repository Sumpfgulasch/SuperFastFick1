using UnityEngine;

// Attach this script to a GameObject that contains child objects.
// This script draws lines in the Scene view between each immediate child.
public class DrawChildGizmos : MonoBehaviour
{
    // Set to true if you want the last child to connect back to the first child.
    public bool closedLoop = false;

    // The color for the gizmo lines.
    public Color gizmoColor = Color.green;

    // Called by Unity to allow you to draw Gizmos in the Scene view.
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        int childCount = transform.childCount;
        if (childCount < 2) return;

        // Draw lines connecting each child in order.
        for (int i = 0; i < childCount - 1; i++)
        {
            Transform currentChild = transform.GetChild(i);
            Transform nextChild = transform.GetChild(i + 1);
            Gizmos.DrawLine(currentChild.position, nextChild.position);
        }
    
        // Optionally close the loop by drawing a line from the last child back to the first.
        if (closedLoop)
        {
            Gizmos.DrawLine(transform.GetChild(childCount - 1).position, transform.GetChild(0).position);
        }
    }
}