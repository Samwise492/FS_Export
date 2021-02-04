using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemBase))] //link to the "Item Base"
public class ItemDatabaseEditor : Editor
{
    private ItemBase itemBase;

    private void Awake()
    {
        itemBase = (ItemBase)target; //variable that keeps our "Item Base"
        
    }
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal(); //how to place buttons (we chose horizontal grid)

        if (GUILayout.Button("New Item")) //button name
        {
            itemBase.CreateItem();
        }

        if (GUILayout.Button("Remove"))
        {
            itemBase.RemoveItem();
        }

        if (GUILayout.Button("<="))
        {
            itemBase.PrevItem();
        }

        if (GUILayout.Button("=>"))
        {
            itemBase.NextItem();
        }

        GUILayout.EndHorizontal(); //the end of list which will be shown in horizontal styles

        base.OnInspectorGUI(); //we use base interface for showing buttons in Unity editor
    }
}
