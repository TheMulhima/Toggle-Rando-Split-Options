using System;
using System.Reflection;
using UnityEngine;

namespace Toggle_Rando_Split_Options
{
    internal static class EmbeddedAssetLoader
    {
        public static readonly Sprite NailUp_Default, NailDown_Default, NailLeft_Default, NailRight_Default;
        public static readonly Sprite DownDash_Default,LeftDash_Default, RightDash_Default;
        public static readonly Sprite LeftClaw_Default, RightClaw_Default;
        
        public static readonly Sprite NailUp_Selected, NailDown_Selected, NailLeft_Selected, NailRight_Selected;
        public static readonly Sprite DownDash_Selected,LeftDash_Selected, RightDash_Selected;
        public static readonly Sprite LeftClaw_Selected, RightClaw_Selected;

        private static Vector2 Half => new Vector2(0.5f, 0.5f);

        static EmbeddedAssetLoader()
        {
            NailUp_Default = CreateSprite("Bindings_Default.Nail.NailUp_Default.png");
            NailDown_Default = CreateSprite("Bindings_Default.Nail.NailDown_Default.png"); 
            NailLeft_Default = CreateSprite("Bindings_Default.Nail.NailLeft_Default.png");
            NailRight_Default = CreateSprite("Bindings_Default.Nail.NailRight_Default.png");

            DownDash_Default = CreateSprite("Bindings_Default.Dash.DownDash_Default.png");
            LeftDash_Default = CreateSprite("Bindings_Default.Dash.LeftDash_Default.png");
            RightDash_Default = CreateSprite("Bindings_Default.Dash.RightDash_Default.png");
  
            LeftClaw_Default = CreateSprite("Bindings_Default.Claw.LeftClaw_Default.png");
            RightClaw_Default = CreateSprite("Bindings_Default.Claw.RightClaw_Default.png");

            
            NailUp_Selected = CreateSprite("Bindings_Selected.Nail.NailUp_Selected.png");
            NailDown_Selected = CreateSprite("Bindings_Selected.Nail.NailDown_Selected.png"); 
            NailLeft_Selected = CreateSprite("Bindings_Selected.Nail.NailLeft_Selected.png");
            NailRight_Selected = CreateSprite("Bindings_Selected.Nail.NailRight_Selected.png");
            
            DownDash_Selected = CreateSprite("Bindings_Selected.Dash.DownDash_Selected.png");
            LeftDash_Selected = CreateSprite("Bindings_Selected.Dash.LeftDash_Selected.png");
            RightDash_Selected = CreateSprite("Bindings_Selected.Dash.RightDash_Selected.png");
            
            LeftClaw_Selected = CreateSprite("Bindings_Selected.Claw.LeftClaw_Selected.png");
            RightClaw_Selected = CreateSprite("Bindings_Selected.Claw.RightClaw_Selected.png");
            
        }
        private static Sprite CreateSprite(string assetPath)
        {
            try
            {
                var asm = Assembly.GetCallingAssembly();
                using (var stream = asm.GetManifestResourceStream($"Toggle-Rando-Split-Options.Assets.{assetPath}"))
                {
                    
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);

                    Texture2D texture2D = new Texture2D(2, 2);
                    bool success = texture2D.LoadImage(data);
                    if (!success)
                        throw new Exception("ImageConversion.LoadImage() failed.");

                    return Sprite.Create(texture2D, GetRectForTexture(texture2D), Half, 64);
                }
            }
            catch (Exception ex)
            {
                Modding.Logger.Log("Couldn't load embedded resource: " + ex.Message);
                throw;
            }
        }

        private static Rect GetRectForTexture(Texture2D texture)
        {
            return new(0f, 0f, texture.width, texture.height);
        }
    }
}