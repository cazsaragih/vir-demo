using System.Collections.Generic;
using UnityEditor;

public class AssetImporter : AssetPostprocessor
{
    public enum Target
    {
        Standalone,
        Android,
        iPhone
    }

    private List<Target> buildTargets = new List<Target> { Target.Standalone, Target.Android, Target.iPhone };

    private TextureImporterFormat standaloneTextureFormat = TextureImporterFormat.DXT1Crunched;
    private TextureImporterFormat androidTextureFormat = TextureImporterFormat.ETC2_RGBA8Crunched;
    private TextureImporterFormat iOSTextureFormat = TextureImporterFormat.ETC2_RGBA8Crunched;

    void OnPreprocessTexture()
    {
        // Only proceed core sprites (non-plugin).
        if (!assetPath.StartsWith("Assets/VirTest/Sprites"))
            return;

        TextureImporter textureImporter = (TextureImporter)assetImporter;


        // Don't apply the settings if want to manually override the settings on inspector.
        if (textureImporter.importSettingsMissing)
        {
            foreach (var target in buildTargets)
            {
                TextureImporterPlatformSettings importerPlatformSettings = new TextureImporterPlatformSettings();
                importerPlatformSettings.name = target.ToString();

                switch (target)
                {
                    case Target.Standalone:
                        importerPlatformSettings.format = standaloneTextureFormat;
                        break;
                    case Target.Android:
                        importerPlatformSettings.format = androidTextureFormat;
                        break;
                    case Target.iPhone:
                        importerPlatformSettings.format = iOSTextureFormat;
                        break;
                }

                importerPlatformSettings.overridden = true;

                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.SetPlatformTextureSettings(importerPlatformSettings);
            }
        }
    }
}
