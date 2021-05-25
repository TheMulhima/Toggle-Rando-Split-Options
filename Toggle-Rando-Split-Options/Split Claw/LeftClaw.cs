using Modding;
using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options.Split_Claw
{
    public class LeftClaw : Binding
    {
        public override Sprite DefaultSprite => EmbeddedAssetLoader.LeftClaw_Default;

        public override Sprite SelectedSprite => EmbeddedAssetLoader.LeftClaw_Selected;
        public LeftClaw() : base("Left Claw"){}

        protected override void OnApplied()
        {
            ModHooks.Instance.HeroUpdateHook += Fix_DoubleJump;
            On.HeroController.CanWallSlide += Apply_Binding_Slide;
            On.HeroController.CanWallJump += Apply_Binding_Jump;

            // This bool allows the other claw to check whether its active or not
            Toggle_Rando_Split_Options.Is_LeftClaw_BindingActive = true;
        }
        private bool Apply_Binding_Slide(On.HeroController.orig_CanWallSlide orig, HeroController self)
        {
            //this prevents wall sliding on left wall
            return !self.wallSlidingL && orig(self);
        }
        private bool Apply_Binding_Jump(On.HeroController.orig_CanWallJump orig, HeroController self)
        {
            /*This is needed because even with wall sliding disabled using the other hook, you can still
             *hug the wall and recharge your jump.
             * The reason that you check for facing right is because wall sliding is disabled so the player 
             * touching the wall wont make them turn around.
             */
            return (self.cState.facingRight && self.cState.touchingWall) && orig(self);
        }
        
        private void Fix_DoubleJump()
        {
            //This is needed because like jump, you can recharge double jump by hugging wall
            var HCS = HeroController.instance.cState;
            var pd = PlayerData.instance;
            
            //if both claw binding active, just remove claw because its easier
            if (Toggle_Rando_Split_Options.Is_LeftClaw_BindingActive && Toggle_Rando_Split_Options.Is_RightClaw_BindingActive)
            {
                pd.hasWalljump = false;
                return;
            }
            
            /*with a wall on left without wall slide, you need to face left to hug the wall.
             * If that happens to be the case, remove wall jump altogether.
             * This would probably make CanWallJump hook obsolete but I dont want to remove it
             * and break something
             */ 
            if (!HCS.facingRight && HCS.touchingWall) pd.hasWalljump = false;
            else pd.hasWalljump = true;
        }

        protected override void OnRestored()
        {
            On.HeroController.CanWallSlide -= Apply_Binding_Slide;
            On.HeroController.CanWallJump -= Apply_Binding_Jump;
            ModHooks.Instance.HeroUpdateHook -= Fix_DoubleJump;
            Toggle_Rando_Split_Options.Is_RightClaw_BindingActive = false;
            
            /* This makes sure that walljump is given back when either:
             * 1) They use both left and right claw
             * 2) They disabled this biniding while hugging a wall (dont ask why)
             */
            PlayerData.instance.hasWalljump = true;
        }
    }
}