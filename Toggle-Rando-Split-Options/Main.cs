using Modding;
using Toggle_Rando_Split_Options.CursedNail;
using Toggle_Rando_Split_Options.Split_Claw;
using Toggle_Rando_Split_Options.Split_Cloak;
using ToggleableBindings;
using UnityEngine;

namespace Toggle_Rando_Split_Options
{
    public class Toggle_Rando_Split_Options : Mod, ITogglableMod
    {
        public Toggle_Rando_Split_Options() : base("Toggle_Rando_Split_Options Bindings") {}

        public override string GetVersion() => "1.0.0";

        //This is needed because for some reason left claw binding doesnt work when right is active
        public static bool Is_RightClaw_BindingActive, Is_LeftClaw_BindingActive;

        private bool HasClaw, HasDash;

        public override void Initialize()
        {
            if (!BindingManager.IsBindingRegistered<NailDownBinding>())
                BindingManager.RegisterBinding<NailDownBinding>();

            if (!BindingManager.IsBindingRegistered<NailLeftBinding>())
                BindingManager.RegisterBinding<NailLeftBinding>();

            if (!BindingManager.IsBindingRegistered<NailRightBinding>())
                BindingManager.RegisterBinding<NailRightBinding>();

            if (!BindingManager.IsBindingRegistered<NailUpBinding>())
                BindingManager.RegisterBinding<NailUpBinding>();
            
            
            if (!BindingManager.IsBindingRegistered<LeftClaw>())
                BindingManager.RegisterBinding<LeftClaw>();

            if (!BindingManager.IsBindingRegistered<RightClaw>())
                BindingManager.RegisterBinding<RightClaw>();

            if (!BindingManager.IsBindingRegistered<LeftDash>())
                BindingManager.RegisterBinding<LeftDash>();

            if (!BindingManager.IsBindingRegistered<RightDAsh.RightDash>())
                BindingManager.RegisterBinding<RightDAsh.RightDash>();
            
            if (!BindingManager.IsBindingRegistered<DownDash>())
                BindingManager.RegisterBinding<DownDash>();

            ModHooks.Instance.SavegameLoadHook += CheckForSkills;
        }

        private void CheckForSkills(int id)
        {
            var pd = PlayerData.instance;
            HasClaw = pd.hasWalljump;
            HasDash = pd.hasDash;

            /*
             * This makes sure people dont exploit and get claw by doing and undoing binding
             * So this makes it work only on save 2
             * Additionally if the player already has left and right claw binding active
             * HasClaw will be true because from the previous quitout, "hasWallJump" is true
            */
            if (HasClaw)
            {
                if (!BindingManager.IsBindingRegistered<LeftClaw>())
                    BindingManager.RegisterBinding<LeftClaw>();

                if (!BindingManager.IsBindingRegistered<RightClaw>())
                    BindingManager.RegisterBinding<RightClaw>();
            }
            else
            {
                BindingManager.DeregisterBinding<LeftClaw>();
                BindingManager.DeregisterBinding<RightClaw>();
            }


            if (HasDash)
            {
                if (!BindingManager.IsBindingRegistered<LeftDash>())
                    BindingManager.RegisterBinding<LeftDash>();

                if (!BindingManager.IsBindingRegistered<RightDAsh.RightDash>())
                    BindingManager.RegisterBinding<RightDAsh.RightDash>();
            
                if (!BindingManager.IsBindingRegistered<DownDash>())
                    BindingManager.RegisterBinding<DownDash>();
            }
            else 
            {
                BindingManager.DeregisterBinding<LeftDash>();
                BindingManager.DeregisterBinding<RightDAsh.RightDash>();
                BindingManager.DeregisterBinding<DownDash>();
            }
            
        }


        public void Unload()
        {
            BindingManager.DeregisterBinding<NailDownBinding>();
            BindingManager.DeregisterBinding<NailLeftBinding>();
            BindingManager.DeregisterBinding<NailRightBinding>();
            BindingManager.DeregisterBinding<NailUpBinding>();
            BindingManager.DeregisterBinding<LeftClaw>();
            BindingManager.DeregisterBinding<RightClaw>();
            BindingManager.DeregisterBinding<LeftDash>();
            BindingManager.DeregisterBinding<RightDAsh.RightDash>();
            BindingManager.DeregisterBinding<DownDash>();
        }


        #region Rando Code that checks for direction
        public enum Direction
        {
            upward,
            leftward,
            rightward,
            downward
        }
        public static Direction GetAttackDirection(HeroController hc)
        {
            if (hc.wallSlidingL)
            {
                return Direction.rightward;
            }
            else if (hc.wallSlidingR)
            {
                return Direction.leftward;
            }

            if (hc.vertical_input > Mathf.Epsilon)
            {
                return Direction.upward;
            }
            else if (hc.vertical_input < -Mathf.Epsilon)
            {
                if (hc.hero_state != GlobalEnums.ActorStates.idle && hc.hero_state != GlobalEnums.ActorStates.running)
                {
                    return Direction.downward;
                }
                else
                {
                    return hc.cState.facingRight ? Direction.rightward : Direction.leftward;
                }
            }
            else
            {
                return hc.cState.facingRight ? Direction.rightward : Direction.leftward;
            }
        }
        
        public static Direction GetDashDirection(HeroController hc)
        {
            InputHandler input = ReflectionHelper.GetAttr<HeroController, InputHandler>(hc, "inputHandler");
            if (!hc.cState.onGround && input.inputActions.down.IsPressed && hc.playerData.GetBool("equippedCharm_31")
                && !(input.inputActions.left.IsPressed || input.inputActions.right.IsPressed))
            {
                return Direction.downward;
            }
            if (hc.wallSlidingL) return Direction.rightward;
            else if (hc.wallSlidingR) return Direction.leftward;
            else if (input.inputActions.right.IsPressed) return Direction.rightward;
            else if (input.inputActions.left.IsPressed) return Direction.leftward;
            else if (hc.cState.facingRight) return Direction.rightward;
            else return Direction.leftward;
        }
        

        #endregion
    }
}