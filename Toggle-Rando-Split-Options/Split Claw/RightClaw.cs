using Modding;
using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.Split_Claw
{
    public class RightClaw : Binding
    {
        //For comments, check the LeftClaw class. The same applys except the left and right is flipped
        public override Sprite DefaultSprite => EmbeddedAssetLoader.RightClaw_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.RightClaw_Selected;
        public RightClaw() : base("Right Claw"){}
        
        protected override void OnApplied()
        {
            ModHooks.Instance.HeroUpdateHook += Fix_DoubleJump;
            On.HeroController.CanWallSlide += Apply_Binding_Slide;
            On.HeroController.CanWallJump += Apply_Binding_Jump;
            Toggle_Rando_Split_Options.Is_RightClaw_BindingActive = true;
        }

        private bool Apply_Binding_Slide(On.HeroController.orig_CanWallSlide orig, HeroController self)
        {
            return !self.wallSlidingR && orig(self);
        }
        
        private bool Apply_Binding_Jump(On.HeroController.orig_CanWallJump orig, HeroController self)
        {
            return (!self.cState.facingRight && self.cState.touchingWall) && orig(self);
        }
        private void Fix_DoubleJump()
        {
            var HCS = HeroController.instance.cState;
            var pd = PlayerData.instance;
            
            if (Toggle_Rando_Split_Options.Is_LeftClaw_BindingActive && Toggle_Rando_Split_Options.Is_RightClaw_BindingActive)
            {
                pd.hasWalljump = false;
                return;
            }
            
            if (HCS.facingRight && HCS.touchingWall) pd.hasWalljump = false;
            else pd.hasWalljump = true;
        }
        
        protected override void OnRestored()
        {
            On.HeroController.CanWallSlide -= Apply_Binding_Slide;
            On.HeroController.CanWallJump -= Apply_Binding_Jump;
            ModHooks.Instance.HeroUpdateHook -= Fix_DoubleJump;
            Toggle_Rando_Split_Options.Is_RightClaw_BindingActive = false;
            PlayerData.instance.hasWalljump = true;
        }
    }
}