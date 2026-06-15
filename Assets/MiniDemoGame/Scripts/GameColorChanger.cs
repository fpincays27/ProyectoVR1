using UnityEngine;

public class CameraColorChanger : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField]
    private Color[] colors;

    private int currentIndex;

    public void ChangeColor()
    {
        if (colors.Length == 0) return;

        currentIndex++;

        if (currentIndex >= colors.Length)
            currentIndex = 0;

        mainCamera.backgroundColor = colors[currentIndex];
    }
}