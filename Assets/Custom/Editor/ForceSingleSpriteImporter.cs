using UnityEditor;

namespace Custom.Editor
{
    /// <summary>
    /// Auto-configures imported textures as Single sprites so UI sprites behave consistently
    /// across the team. By default it only touches textures whose import data has not been set
    /// yet (<see cref="TextureImporter.importSettingsMissing"/>), so existing art is never
    /// unexpectedly re-imported. Flip the <c>ForceAllTextures</c> const below to true to force
    /// every imported texture to Single sprite, ignoring prior import data.
    /// </summary>
    public class ForceSingleSpriteImporter : AssetPostprocessor
    {
        // Flip to true to force ALL imported textures to Single sprite, ignoring prior import data.
        private const bool ForceAllTextures = false;

        private void OnPreprocessTexture()
        {
            TextureImporter importer = (TextureImporter)assetImporter;

            if (!ForceAllTextures && !importer.importSettingsMissing)
                return;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
        }
    }
}