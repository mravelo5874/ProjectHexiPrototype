using UnityEngine;
using UnityEditor;

public class TextureArrayWizard : ScriptableWizard
{
    public Texture2D[] textures;

    [MenuItem("Assets/Create/Texture Array")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<TextureArrayWizard>("Create Texture Array", "Create" );
    }

    void OnWizardCreate()
    {
        // return if texture array is 0
        if (textures.Length == 0)
        {
            return;
        }

        string path = EditorUtility.SaveFilePanelInProject("Save Texture Array", "Texture Array", "asset", "Save Texture Array");
        if (path.Length == 0)
        {
            return;
        }
        
        Texture2D t = textures[0];
        Texture2DArray texture_array = new Texture2DArray(t.width, t.height, textures.Length, t.format, t.mipmapCount > 1);
        texture_array.anisoLevel = t.anisoLevel;
        texture_array.filterMode = t.filterMode;
        texture_array.wrapMode = t.wrapMode;

        for (int i = 0; i < textures.Length; i++)
        {
            for (int m = 0; m < t.mipmapCount; m++)
            {
                Graphics.CopyTexture(textures[i], 0, m, texture_array, i, m);
            }
        }

        AssetDatabase.CreateAsset(texture_array, path);
    }
}
