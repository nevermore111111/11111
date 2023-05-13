using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CheckFX : MonoBehaviour
{
    //要做的，1将所有文件格式修改为default，2所有图片勾选webg，并且勾选      3修改5*5        4打印max 超过512的图
    [MenuItem("Assets/美术/检查图片")]
    static private void CheckFXTextures()
    {
        //Assets/SharkRes/FX/Tex/Aura_003.png
         Texture2D[] textures = SelectTex();
         ChangeTex(textures);
    }
    static private Texture2D[] SelectTex()
    {
        string folderPath = "Assets/SharkRes/FX/Tex";
        string[] textureGUIDs = AssetDatabase.FindAssets("t:Texture", new[] { folderPath });
        Texture2D[] textures = new Texture2D[textureGUIDs.Length];
        for (int i = 0; i < textureGUIDs.Length; i++)
        {
            string texturePath = AssetDatabase.GUIDToAssetPath(textureGUIDs[i]);
            textures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        }
        return  textures;
    }
    static private void ChangeTex(Texture2D[] texs)
    {
        
        for(int i = 0; i < texs.Length; i++)
        {
            string assetPath = AssetDatabase.GetAssetPath(texs[i]);
            string name =  Path.GetFileNameWithoutExtension(assetPath);
            Debug.Log(name);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if(textureImporter != null)
            {
                TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings("WebGL");
                settings.format = TextureImporterFormat.ASTC_5x5;//
                settings.overridden = true;//
                settings.compressionQuality = 50;//
                if(name.ToLower().StartsWith("sl"))
                {
                    settings.maxTextureSize = 128;
                }
                if (name.Length == 4||(name.Length > 6 && name[5]=='0')||name.Contains("fishicon")) 
                {
                    settings.maxTextureSize = 256;
                }
                else
                {
                    settings.maxTextureSize = 512;
                }
                settings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
                settings.name = "WebGL";
                textureImporter.SetPlatformTextureSettings(settings);
                textureImporter.textureType = TextureImporterType.Default;
                if (name.ToLower().StartsWith("sl")|| name.Contains("fishicon"))
                {
                    textureImporter.alphaIsTransparency = true;
                }
                else
                {
                    textureImporter.alphaIsTransparency = false;
                }
            }
            textureImporter.SaveAndReimport();
        }
    }
    [MenuItem("Assets/美术/修改图片(支持鱼的皮肤，技能图标，鱼的贴图)")]
    static private void Fun01()
    {
        Texture2D[] texs = new Texture2D[Selection.objects.Length];
        for(int i = 0; i < texs.Length; i++)
        {
            texs[i] = Selection.objects[i] as Texture2D;
        }
        ChangeTex(texs);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
