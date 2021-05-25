using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.CursedNail
{
    public class NailLeftBinding : Binding
    {
        public override Sprite DefaultSprite => EmbeddedAssetLoader.NailLeft_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.NailLeft_Selected;
        public NailLeftBinding() : base("Left Slash"){}

        protected override void OnApplied()
        {
            On.HeroController.CanAttack += Apply_Binding;
        }

        private bool Apply_Binding(On.HeroController.orig_CanAttack orig, HeroController self)
        {
            return Toggle_Rando_Split_Options.GetAttackDirection(self) != Toggle_Rando_Split_Options.Direction.leftward && orig(self);
        }

        protected override void OnRestored()
        {
            On.HeroController.CanAttack -= Apply_Binding;
        }
    }
}