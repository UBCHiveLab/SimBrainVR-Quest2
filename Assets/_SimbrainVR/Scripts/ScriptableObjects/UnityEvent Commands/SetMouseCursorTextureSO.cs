using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Mouse Cursor Texture_Command", menuName = "ScriptableObjects/UnityEvent Commands/Set Mouse Cursor Texture")]
public class SetMouseCursorTextureSO : ScriptableObject
{
    [SerializeField] private Texture2D standardTexture = default;
    [SerializeField] private Texture2D textureForLowResolution = default;
    [SerializeField] private Texture2D textureFor4K = default;
    [SerializeField] private float maxWidthForLowRes = 1280f;
    [SerializeField] private CursorMode cursorMode = CursorMode.ForceSoftware;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    private float minWidthFor4K = 3840f;
    public void Activate()
    {
        float currentResWidth = Screen.currentResolution.width;

        Texture2D newCursorTexture = standardTexture;

        //Debug.Log("current res width " + currentResWidth);

        if (currentResWidth < maxWidthForLowRes)
        {
            //use low res texture
            newCursorTexture = textureForLowResolution;

        }
        else if (currentResWidth >= minWidthFor4K)
        {
            //use 4K texture
            newCursorTexture = textureFor4K;
        }

        Debug.Log("changing cursor texture to " + newCursorTexture.name);

        Cursor.SetCursor(newCursorTexture, hotSpot, cursorMode);
    }
}
