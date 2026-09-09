using UnityEngine;

namespace Game.Scripts.Attributes
{
    public class PreviewSpriteAttribute : PropertyAttribute
    {
        public float Height { get; private set; }
        
        public PreviewSpriteAttribute(float height = 64f)
        {
            Height = height;
        }
    }
}