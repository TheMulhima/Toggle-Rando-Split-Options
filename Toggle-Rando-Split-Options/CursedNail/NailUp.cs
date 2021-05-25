using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.CursedNail
{
    public class NailUpBinding : Binding
    {
        public override Sprite DefaultSprite => EmbeddedAssetLoader.NailUp_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.NailUp_Selected;

        public NailUpBinding() : base("Up Slash")
        {
        }

        protected override void OnApplied()
        {
            On.HeroController.CanAttack += Apply_Binding;
        }

        private bool Apply_Binding(On.HeroController.orig_CanAttack orig, HeroController self)
        {
            return Toggle_Rando_Split_Options.GetAttackDirection(self) != Toggle_Rando_Split_Options.Direction.upward &&
                   orig(self);
        }

        protected override void OnRestored()
        {
            On.HeroController.CanAttack -= Apply_Binding;
        }
    }
}