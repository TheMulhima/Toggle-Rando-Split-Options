using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.Split_Cloak
{
    public class DownDash : Binding
    {
        public override Sprite DefaultSprite => EmbeddedAssetLoader.DownDash_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.DownDash_Selected;
        public DownDash() : base("Down Dash"){}

        protected override void OnApplied()
        {
            On.HeroController.CanDash += Apply_Binding;
        }

        private bool Apply_Binding(On.HeroController.orig_CanDash orig, HeroController self)
        {
            return Toggle_Rando_Split_Options.GetDashDirection(self) != Toggle_Rando_Split_Options.Direction.downward && orig(self);
        }

        protected override void OnRestored()
        {
            On.HeroController.CanDash -= Apply_Binding;
        }
    }
}