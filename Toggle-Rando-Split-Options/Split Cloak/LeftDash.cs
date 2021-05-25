using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.Split_Cloak
{
    public class LeftDash : Binding
    {
        public override Sprite DefaultSprite => EmbeddedAssetLoader.LeftDash_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.LeftDash_Selected;
        public LeftDash() : base("Left Dash"){}

        protected override void OnApplied()
        {
            On.HeroController.CanDash += Apply_Binding;
        }

        private bool Apply_Binding(On.HeroController.orig_CanDash orig, HeroController self)
        {
            return Toggle_Rando_Split_Options.GetDashDirection(self) != Toggle_Rando_Split_Options.Direction.leftward && orig(self);
        }

        protected override void OnRestored()
        {
            On.HeroController.CanDash -= Apply_Binding;
        }
        
    }
}