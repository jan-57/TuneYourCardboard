#if UNITY_EDITOR
//https://www.youtube.com/watch?v=H8j4CbnVTfQ Litan Roy
//https://www.colorhexa.com/ COLOR HELL YEAH
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class HierarchyItemColor
{
    private static byte globalAlpha = 255;
    static HierarchyItemColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarcheWindowItemOnGUI;
    }

    static void HierarcheWindowItemOnGUI(int instanceID, Rect selectRect)
    {
        var gameobject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameobject != null)
        {

            #region Non Color
            if (gameobject.name.StartsWith("/black ", System.StringComparison.Ordinal)) //Black
            {
                EditorGUI.DrawRect(selectRect, Color.black);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/black ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/w ", System.StringComparison.Ordinal)) //white
            {
                EditorGUI.DrawRect(selectRect, Color.white);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/w ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/ow ", System.StringComparison.Ordinal)) //Offwhite
            {
                EditorGUI.DrawRect(selectRect, new Color32(255, 250, 240, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/ow ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/bone ", System.StringComparison.Ordinal)) //Bone
            {
                EditorGUI.DrawRect(selectRect, new Color32(227, 218, 201, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/bone ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/grey ", System.StringComparison.Ordinal)) //Grey
            {
                EditorGUI.DrawRect(selectRect, Color.grey);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/grey ", "").ToUpperInvariant());
            }
            #endregion

            #region Yellowich
            else if (gameobject.name.StartsWith("/gold ", System.StringComparison.Ordinal)) //Gold
            {
                EditorGUI.DrawRect(selectRect, new Color32(255, 215, 0, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/gold ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/dy ", System.StringComparison.Ordinal)) //DarkGreen
            {
                EditorGUI.DrawRect(selectRect, new Color32(184, 134, 11, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/dy ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/y ", System.StringComparison.Ordinal)) //Yellow
            {
                EditorGUI.DrawRect(selectRect, Color.yellow);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/y ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/ly ", System.StringComparison.Ordinal)) //Lightyellow
            {
                EditorGUI.DrawRect(selectRect, new Color32(255, 255, 224, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/ly ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lo ", System.StringComparison.Ordinal)) //LigtOrange
            {
                EditorGUI.DrawRect(selectRect, new Color32(200, 120, 25, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lo ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/o ", System.StringComparison.Ordinal)) //Orange
            {
                EditorGUI.DrawRect(selectRect, new Color32(255, 112, 20, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/o ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lg ", System.StringComparison.Ordinal)) //LightGreen
            {
                EditorGUI.DrawRect(selectRect, new Color32(144, 238, 144, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lg ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lbanana ", System.StringComparison.Ordinal)) //LightBanana
            {
                EditorGUI.DrawRect(selectRect, new Color32(250, 231, 181, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lbanana ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/banana ", System.StringComparison.Ordinal)) //Banana
            {
                EditorGUI.DrawRect(selectRect, new Color32(255, 255, 53, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/banana ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/dbanana ", System.StringComparison.Ordinal)) //DarkBanana
            {
                EditorGUI.DrawRect(selectRect, new Color32(165, 42, 42, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/dbanana ", "").ToUpperInvariant());
            }
            #endregion

            #region Redich
            else if (gameobject.name.StartsWith("/lust ", System.StringComparison.Ordinal)) //Lust
            {
                EditorGUI.DrawRect(selectRect, new Color32(230, 32, 32, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lust ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/maroon ", System.StringComparison.Ordinal)) //Maroon
            {
                EditorGUI.DrawRect(selectRect, new Color32(128, 0, 0, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/maroon ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/dr ", System.StringComparison.Ordinal)) //DarkRed
            {
                EditorGUI.DrawRect(selectRect, new Color32(139, 0, 0, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/dr ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/r ", System.StringComparison.Ordinal)) //Red
            {
                EditorGUI.DrawRect(selectRect, Color.red);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/r ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lr ", System.StringComparison.Ordinal)) //Lightred
            {
                EditorGUI.DrawRect(selectRect, new Color32(240, 128, 128, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lr ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/salmon ", System.StringComparison.Ordinal)) //Salmon
            {
                EditorGUI.DrawRect(selectRect, new Color32(250, 128, 114, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/salmon ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/m ", System.StringComparison.Ordinal)) //Magenta
            {
                EditorGUI.DrawRect(selectRect, Color.magenta);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/m ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lava ", System.StringComparison.Ordinal)) //Lava
            {
                EditorGUI.DrawRect(selectRect, new Color32(207, 16, 32, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lava ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/p ", System.StringComparison.Ordinal)) //Purple
            {
                EditorGUI.DrawRect(selectRect, new Color32(128, 0, 128, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/p ", "").ToUpperInvariant());
            }
            #endregion

            #region Blueich
            else if (gameobject.name.StartsWith("/indigo ", System.StringComparison.Ordinal)) //Indigo
            {
                EditorGUI.DrawRect(selectRect, new Color32(75, 0, 130, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/indigo ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/navy ", System.StringComparison.Ordinal)) //Navy
            {
                EditorGUI.DrawRect(selectRect, new Color32(0, 0, 128, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/navy ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/db ", System.StringComparison.Ordinal)) //Darkblue
            {
                EditorGUI.DrawRect(selectRect, new Color32(0, 0, 139, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/db ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/b ", System.StringComparison.Ordinal)) //Blue
            {
                EditorGUI.DrawRect(selectRect, Color.blue);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/b ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lb ", System.StringComparison.Ordinal)) //Lightblue
            {
                EditorGUI.DrawRect(selectRect, new Color32(173, 216, 230, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lb ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/c ", System.StringComparison.Ordinal)) //Cyan
            {
                EditorGUI.DrawRect(selectRect, Color.cyan);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/c ", "").ToUpperInvariant());
            }
            #endregion

            #region Greenich
            else if (gameobject.name.StartsWith("/dg ", System.StringComparison.Ordinal)) //Darkgreen
            {
                EditorGUI.DrawRect(selectRect, new Color32(0, 100, 0, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/dg ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/g ", System.StringComparison.Ordinal)) //Green
            {
                EditorGUI.DrawRect(selectRect, Color.green);
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/g ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lg ", System.StringComparison.Ordinal)) //LightGreen
            {
                EditorGUI.DrawRect(selectRect, new Color32(144, 238, 144, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lg ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/leaf ", System.StringComparison.Ordinal)) //Leaf
            {
                EditorGUI.DrawRect(selectRect, new Color32(173, 255, 47, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/leaf ", "").ToUpperInvariant());
            }
            else if (gameobject.name.StartsWith("/lime ", System.StringComparison.Ordinal)) //Lime
            {
                EditorGUI.DrawRect(selectRect, new Color32(191, 255, 0, globalAlpha));
                EditorGUI.DropShadowLabel(selectRect, gameobject.name.Replace("/lime ", "").ToUpperInvariant());
            }
            #endregion
        }
    }

}
#endif