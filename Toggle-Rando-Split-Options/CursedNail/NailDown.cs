using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.CursedNail
{
    public class NailDownBinding : Binding
    {
        public NailDownBinding() : base("Pogo")
        {
        }

        public override Sprite DefaultSprite => EmbeddedAssetLoader.NailDown_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.NailDown_Selected;


        protected override void OnApplied()
        {
            On.HeroController.CanAttack += Apply_Binding;
        }

        private bool Apply_Binding(On.HeroController.orig_CanAttack orig, HeroController self)
        {
            return Toggle_Rando_Split_Options.GetAttackDirection(self) !=
                Toggle_Rando_Split_Options.Direction.downward && orig(self);
        }

        protected override void OnRestored()
        {
            On.HeroController.CanAttack -= Apply_Binding;
        }
    }
}