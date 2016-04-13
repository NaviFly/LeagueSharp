using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace _xcsoft__SpellData_Viewer
{
    class Program
    {
        static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        static Menu Menu;
        static Render.Text Text;
        const string Font = "monospace";
        const string NewLine = "\n";
        static Spell Q, W, E, R;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R);

            Menu = new Menu("[xcsoft] SpellData Viewer", "xcsoft_spelldataviewer", true);
            Menu.AddToMainMenu();

            Menu.AddItem(new MenuItem("switch", "Switch")).SetValue<Boolean>(true);
            Menu.AddItem(new MenuItem("x", "X")).SetValue<Slider>(new Slider(250 ,0 , Drawing.Width));
            Menu.AddItem(new MenuItem("y", "Y")).SetValue<Slider>(new Slider(0, -2000, Drawing.Height));
            Menu.AddItem(new MenuItem("size", "Size")).SetValue<Slider>(new Slider(16, 10, 20));

            Menu.Item("x").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text.X = eventArgs.GetNewValue<Slider>().Value;
                };

            Menu.Item("y").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text.Y = eventArgs.GetNewValue<Slider>().Value;
                };

            Menu.Item("size").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text = new Render.Text(Menu.Item("x").GetValue<Slider>().Value, Menu.Item("y").GetValue<Slider>().Value, "", Menu.Item("size").GetValue<Slider>().Value, SharpDX.Color.White, Font);
                };

            Text = new Render.Text(Menu.Item("x").GetValue<Slider>().Value, Menu.Item("y").GetValue<Slider>().Value, "", Menu.Item("size").GetValue<Slider>().Value, SharpDX.Color.White, Font);

            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (!Menu.Item("switch").GetValue<Boolean>())
                return;

            Text.text =
                "AffectsStatusFlags: " + "Q: " + Q.Instance.SData.AffectsStatusFlags + ", W: " + W.Instance.SData.AffectsStatusFlags + ", E: " + E.Instance.SData.AffectsStatusFlags + ", R: " + R.Instance.SData.AffectsStatusFlags + NewLine +
                "AffectsTypeFlags: " + "Q: " + Q.Instance.SData.AffectsTypeFlags + ", W: " + W.Instance.SData.AffectsTypeFlags + ", E: " + E.Instance.SData.AffectsTypeFlags + ", R: " + R.Instance.SData.AffectsTypeFlags + NewLine +
                "AfterEffectName: " + "Q: " + Q.Instance.SData.AfterEffectName + ", W: " + W.Instance.SData.AfterEffectName + ", E: " + E.Instance.SData.AfterEffectName + ", R: " + R.Instance.SData.AfterEffectName + NewLine +
                "AlternateName: " + "Q: " + Q.Instance.SData.AlternateName + ", W: " + W.Instance.SData.AlternateName + ", E: " + E.Instance.SData.AlternateName + ", R: " + R.Instance.SData.AlternateName + NewLine +
                "AlwaysSnapFacing: " + "Q: " + Q.Instance.SData.AlwaysSnapFacing + ", W: " + W.Instance.SData.AlwaysSnapFacing + ", E: " + E.Instance.SData.AlwaysSnapFacing + ", R: " + R.Instance.SData.AlwaysSnapFacing + NewLine +
                "AmmoCountHiddenInUI: " + "Q: " + Q.Instance.SData.AmmoCountHiddenInUI + ", W: " + W.Instance.SData.AmmoCountHiddenInUI + ", E: " + E.Instance.SData.AmmoCountHiddenInUI + ", R: " + R.Instance.SData.AmmoCountHiddenInUI + NewLine +
                "AmmoNotAffectedByCDR: " + "Q: " + Q.Instance.SData.AmmoNotAffectedByCDR + ", W: " + W.Instance.SData.AmmoNotAffectedByCDR + ", E: " + E.Instance.SData.AmmoNotAffectedByCDR + ", R: " + R.Instance.SData.AmmoNotAffectedByCDR + NewLine +
                "AmmoRechargeTime: " + "Q: " + Q.Instance.SData.AmmoRechargeTime + ", W: " + W.Instance.SData.AmmoRechargeTime + ", E: " + E.Instance.SData.AmmoRechargeTime + ", R: " + R.Instance.SData.AmmoRechargeTime + NewLine +
                "AmmoUsed: " + "Q: " + Q.Instance.SData.AmmoUsed + ", W: " + W.Instance.SData.AmmoUsed + ", E: " + E.Instance.SData.AmmoUsed + ", R: " + R.Instance.SData.AmmoUsed + NewLine +
                "AnimationLeadOutName: " + "Q: " + Q.Instance.SData.AnimationLeadOutName + ", W: " + W.Instance.SData.AnimationLeadOutName + ", E: " + E.Instance.SData.AnimationLeadOutName + ", R: " + R.Instance.SData.AnimationLeadOutName + NewLine +
                "AnimationLoopName: " + "Q: " + Q.Instance.SData.AnimationLoopName + ", W: " + W.Instance.SData.AnimationLoopName + ", E: " + E.Instance.SData.AnimationLoopName + ", R: " + R.Instance.SData.AnimationLoopName + NewLine +
                "AnimationName: " + "Q: " + Q.Instance.SData.AnimationName + ", W: " + W.Instance.SData.AnimationName + ", E: " + E.Instance.SData.AnimationName + ", R: " + R.Instance.SData.AnimationName + NewLine +
                "AnimationWinddownName: " + "Q: " + Q.Instance.SData.AnimationWinddownName + ", W: " + W.Instance.SData.AnimationWinddownName + ", E: " + E.Instance.SData.AnimationWinddownName + ", R: " + R.Instance.SData.AnimationWinddownName + NewLine +
                "ApplyAttackDamage: " + "Q: " + Q.Instance.SData.ApplyAttackDamage + ", W: " + W.Instance.SData.ApplyAttackDamage + ", E: " + E.Instance.SData.ApplyAttackDamage + ", R: " + R.Instance.SData.ApplyAttackDamage + NewLine +
                "ApplyAttackEffect: " + "Q: " + Q.Instance.SData.ApplyAttackEffect + ", W: " + W.Instance.SData.ApplyAttackEffect + ", E: " + E.Instance.SData.ApplyAttackEffect + ", R: " + R.Instance.SData.ApplyAttackEffect + NewLine +
                "ApplyMaterialOnHitSound: " + "Q: " + Q.Instance.SData.ApplyMaterialOnHitSound + ", W: " + W.Instance.SData.ApplyMaterialOnHitSound + ", E: " + E.Instance.SData.ApplyMaterialOnHitSound + ", R: " + R.Instance.SData.ApplyMaterialOnHitSound + NewLine +
                "BelongsToAvatar: " + "Q: " + Q.Instance.SData.BelongsToAvatar + ", W: " + W.Instance.SData.BelongsToAvatar + ", E: " + E.Instance.SData.BelongsToAvatar + ", R: " + R.Instance.SData.BelongsToAvatar + NewLine +
                "BounceRadius: " + "Q: " + Q.Instance.SData.BounceRadius + ", W: " + W.Instance.SData.BounceRadius + ", E: " + E.Instance.SData.BounceRadius + ", R: " + R.Instance.SData.BounceRadius + NewLine +
                "CanCastWhileDisabled: " + "Q: " + Q.Instance.SData.CanCastWhileDisabled + ", W: " + W.Instance.SData.CanCastWhileDisabled + ", E: " + E.Instance.SData.CanCastWhileDisabled + ", R: " + R.Instance.SData.CanCastWhileDisabled + NewLine +
                "CancelChargeOnRecastTime: " + "Q: " + Q.Instance.SData.CancelChargeOnRecastTime + ", W: " + W.Instance.SData.CancelChargeOnRecastTime + ", E: " + E.Instance.SData.CancelChargeOnRecastTime + ", R: " + R.Instance.SData.CancelChargeOnRecastTime + NewLine +
                "CanMoveWhileChanneling: " + "Q: " + Q.Instance.SData.CanMoveWhileChanneling + ", W: " + W.Instance.SData.CanMoveWhileChanneling + ", E: " + E.Instance.SData.CanMoveWhileChanneling + ", R: " + R.Instance.SData.CanMoveWhileChanneling + NewLine +
                "CannotBeSuppressed: " + "Q: " + Q.Instance.SData.CannotBeSuppressed + ", W: " + W.Instance.SData.CannotBeSuppressed + ", E: " + E.Instance.SData.CannotBeSuppressed + ", R: " + R.Instance.SData.CannotBeSuppressed + NewLine +
                "CanOnlyCastWhileDead: " + "Q: " + Q.Instance.SData.CanOnlyCastWhileDead + ", W: " + W.Instance.SData.CanOnlyCastWhileDead + ", E: " + E.Instance.SData.CanOnlyCastWhileDead + ", R: " + R.Instance.SData.CanOnlyCastWhileDead + NewLine +
                "CanOnlyCastWhileDisabled: " + "Q: " + Q.Instance.SData.CanOnlyCastWhileDisabled + ", W: " + W.Instance.SData.CanOnlyCastWhileDisabled + ", E: " + E.Instance.SData.CanOnlyCastWhileDisabled + ", R: " + R.Instance.SData.CanOnlyCastWhileDisabled + NewLine +
                "CantCancelWhileChanneling: " + "Q: " + Q.Instance.SData.CantCancelWhileChanneling + ", W: " + W.Instance.SData.CantCancelWhileChanneling + ", E: " + E.Instance.SData.CantCancelWhileChanneling + ", R: " + R.Instance.SData.CantCancelWhileChanneling + NewLine +
                "CantCancelWhileWindingUp: " + "Q: " + Q.Instance.SData.CantCancelWhileWindingUp + ", W: " + W.Instance.SData.CantCancelWhileWindingUp + ", E: " + E.Instance.SData.CantCancelWhileWindingUp + ", R: " + R.Instance.SData.CantCancelWhileWindingUp + NewLine +
                "CantCastWhileRooted: " + "Q: " + Q.Instance.SData.CantCastWhileRooted + ", W: " + W.Instance.SData.CantCastWhileRooted + ", E: " + E.Instance.SData.CantCastWhileRooted + ", R: " + R.Instance.SData.CantCastWhileRooted + NewLine +
                "CastConeAngle: " + "Q: " + Q.Instance.SData.CastConeAngle + ", W: " + W.Instance.SData.CastConeAngle + ", E: " + E.Instance.SData.CastConeAngle + ", R: " + R.Instance.SData.CastConeAngle + NewLine +
                "CastConeDistance: " + "Q: " + Q.Instance.SData.CastConeDistance + ", W: " + W.Instance.SData.CastConeDistance + ", E: " + E.Instance.SData.CastConeDistance + ", R: " + R.Instance.SData.CastConeDistance + NewLine +
                "CastFrame: " + "Q: " + Q.Instance.SData.CastFrame + ", W: " + W.Instance.SData.CastFrame + ", E: " + E.Instance.SData.CastFrame + ", R: " + R.Instance.SData.CastFrame + NewLine +
                "CastRadius: " + "Q: " + Q.Instance.SData.CastRadius + ", W: " + W.Instance.SData.CastRadius + ", E: " + E.Instance.SData.CastRadius + ", R: " + R.Instance.SData.CastRadius + NewLine +
                "CastRadiusSecondary: " + "Q: " + Q.Instance.SData.CastRadiusSecondary + ", W: " + W.Instance.SData.CastRadiusSecondary + ", E: " + E.Instance.SData.CastRadiusSecondary + ", R: " + R.Instance.SData.CastRadiusSecondary + NewLine +
                "CastRadiusTexture: " + "Q: " + Q.Instance.SData.CastRadiusTexture + ", W: " + W.Instance.SData.CastRadiusTexture + ", E: " + E.Instance.SData.CastRadiusTexture + ", R: " + R.Instance.SData.CastRadiusTexture + NewLine +
                "CastRange: " + "Q: " + Q.Instance.SData.CastRange + ", W: " + W.Instance.SData.CastRange + ", E: " + E.Instance.SData.CastRange + ", R: " + R.Instance.SData.CastRange + NewLine +
                "CastRangeDisplayOverride: " + "Q: " + Q.Instance.SData.CastRangeDisplayOverride + ", W: " + W.Instance.SData.CastRangeDisplayOverride + ", E: " + E.Instance.SData.CastRangeDisplayOverride + ", R: " + R.Instance.SData.CastRangeDisplayOverride + NewLine +
                "CastRangeGrowthDuration: " + "Q: " + Q.Instance.SData.CastRangeGrowthDuration + ", W: " + W.Instance.SData.CastRangeGrowthDuration + ", E: " + E.Instance.SData.CastRangeGrowthDuration + ", R: " + R.Instance.SData.CastRangeGrowthDuration + NewLine +
                "CastRangeGrowthMax: " + "Q: " + Q.Instance.SData.CastRangeGrowthMax + ", W: " + W.Instance.SData.CastRangeGrowthMax + ", E: " + E.Instance.SData.CastRangeGrowthMax + ", R: " + R.Instance.SData.CastRangeGrowthMax + NewLine +
                "CastRangeTextureOverrideName: " + "Q: " + Q.Instance.SData.CastRangeTextureOverrideName + ", W: " + W.Instance.SData.CastRangeTextureOverrideName + ", E: " + E.Instance.SData.CastRangeTextureOverrideName + ", R: " + R.Instance.SData.CastRangeTextureOverrideName + NewLine +
                "CastRangeUseBoundingBoxes: " + "Q: " + Q.Instance.SData.CastRangeUseBoundingBoxes + ", W: " + W.Instance.SData.CastRangeUseBoundingBoxes + ", E: " + E.Instance.SData.CastRangeUseBoundingBoxes + ", R: " + R.Instance.SData.CastRangeUseBoundingBoxes + NewLine +
                "CastTargetAdditionalUnitsRadius: " + "Q: " + Q.Instance.SData.CastTargetAdditionalUnitsRadius + ", W: " + W.Instance.SData.CastTargetAdditionalUnitsRadius + ", E: " + E.Instance.SData.CastTargetAdditionalUnitsRadius + ", R: " + R.Instance.SData.CastTargetAdditionalUnitsRadius + NewLine +
                "CastType: " + "Q: " + Q.Instance.SData.CastType + ", W: " + W.Instance.SData.CastType + ", E: " + E.Instance.SData.CastType + ", R: " + R.Instance.SData.CastType + NewLine +
                "ChannelDuration: " + "Q: " + Q.Instance.SData.ChannelDuration + ", W: " + W.Instance.SData.ChannelDuration + ", E: " + E.Instance.SData.ChannelDuration + ", R: " + R.Instance.SData.ChannelDuration + NewLine +
                "ChargeUpdateInterval: " + "Q: " + Q.Instance.SData.ChargeUpdateInterval + ", W: " + W.Instance.SData.ChargeUpdateInterval + ", E: " + E.Instance.SData.ChargeUpdateInterval + ", R: " + R.Instance.SData.ChargeUpdateInterval + NewLine +
                "CircleMissileAngularVelocity: " + "Q: " + Q.Instance.SData.CircleMissileAngularVelocity + ", W: " + W.Instance.SData.CircleMissileAngularVelocity + ", E: " + E.Instance.SData.CircleMissileAngularVelocity + ", R: " + R.Instance.SData.CircleMissileAngularVelocity + NewLine +
                "CircleMissileRadialVelocity: " + "Q: " + Q.Instance.SData.CircleMissileRadialVelocity + ", W: " + W.Instance.SData.CircleMissileRadialVelocity + ", E: " + E.Instance.SData.CircleMissileRadialVelocity + ", R: " + R.Instance.SData.CircleMissileRadialVelocity + NewLine +
                "ClientOnlyMissileTargetBoneName: " + "Q: " + Q.Instance.SData.ClientOnlyMissileTargetBoneName + ", W: " + W.Instance.SData.ClientOnlyMissileTargetBoneName + ", E: " + E.Instance.SData.ClientOnlyMissileTargetBoneName + ", R: " + R.Instance.SData.ClientOnlyMissileTargetBoneName + NewLine +
                "Coefficient: " + "Q: " + Q.Instance.SData.Coefficient + ", W: " + W.Instance.SData.Coefficient + ", E: " + E.Instance.SData.Coefficient + ", R: " + R.Instance.SData.Coefficient + NewLine +
                "Coefficient2: " + "Q: " + Q.Instance.SData.Coefficient2 + ", W: " + W.Instance.SData.Coefficient2 + ", E: " + E.Instance.SData.Coefficient2 + ", R: " + R.Instance.SData.Coefficient2 + NewLine +
                "ConsideredAsAutoAttack: " + "Q: " + Q.Instance.SData.ConsideredAsAutoAttack + ", W: " + W.Instance.SData.ConsideredAsAutoAttack + ", E: " + E.Instance.SData.ConsideredAsAutoAttack + ", R: " + R.Instance.SData.ConsideredAsAutoAttack + NewLine +
                "Cooldown: " + "Q: " + Q.Instance.SData.Cooldown + ", W: " + W.Instance.SData.Cooldown + ", E: " + E.Instance.SData.Cooldown + ", R: " + R.Instance.SData.Cooldown + NewLine +
                "CursorChangesInGrass: " + "Q: " + Q.Instance.SData.CursorChangesInGrass + ", W: " + W.Instance.SData.CursorChangesInGrass + ", E: " + E.Instance.SData.CursorChangesInGrass + ", R: " + R.Instance.SData.CursorChangesInGrass + NewLine +
                "CursorChangesInTerrain: " + "Q: " + Q.Instance.SData.CursorChangesInTerrain + ", W: " + W.Instance.SData.CursorChangesInTerrain + ", E: " + E.Instance.SData.CursorChangesInTerrain + ", R: " + R.Instance.SData.CursorChangesInTerrain + NewLine +
                "DeathRecapPriority: " + "Q: " + Q.Instance.SData.DeathRecapPriority + ", W: " + W.Instance.SData.DeathRecapPriority + ", E: " + E.Instance.SData.DeathRecapPriority + ", R: " + R.Instance.SData.DeathRecapPriority + NewLine +
                "DelayCastOffsetPercent: " + "Q: " + Q.Instance.SData.DelayCastOffsetPercent + ", W: " + W.Instance.SData.DelayCastOffsetPercent + ", E: " + E.Instance.SData.DelayCastOffsetPercent + ", R: " + R.Instance.SData.DelayCastOffsetPercent + NewLine +
                "DelayTotalTimePercent: " + "Q: " + Q.Instance.SData.DelayTotalTimePercent + ", W: " + W.Instance.SData.DelayTotalTimePercent + ", E: " + E.Instance.SData.DelayTotalTimePercent + ", R: " + R.Instance.SData.DelayTotalTimePercent + NewLine +
                "Description: " + "Q: " + Q.Instance.SData.Description + ", W: " + W.Instance.SData.Description + ", E: " + E.Instance.SData.Description + ", R: " + R.Instance.SData.Description + NewLine +
                "DisableCastBar: " + "Q: " + Q.Instance.SData.DisableCastBar + ", W: " + W.Instance.SData.DisableCastBar + ", E: " + E.Instance.SData.DisableCastBar + ", R: " + R.Instance.SData.DisableCastBar + NewLine +
                "DisplayName: " + "Q: " + Q.Instance.SData.DisplayName + ", W: " + W.Instance.SData.DisplayName + ", E: " + E.Instance.SData.DisplayName + ", R: " + R.Instance.SData.DisplayName + NewLine +
                "DoesntBreakChannels: " + "Q: " + Q.Instance.SData.DoesntBreakChannels + ", W: " + W.Instance.SData.DoesntBreakChannels + ", E: " + E.Instance.SData.DoesntBreakChannels + ", R: " + R.Instance.SData.DoesntBreakChannels + NewLine +
                "DoNotNeedToFaceTarget: " + "Q: " + Q.Instance.SData.DoNotNeedToFaceTarget + ", W: " + W.Instance.SData.DoNotNeedToFaceTarget + ", E: " + E.Instance.SData.DoNotNeedToFaceTarget + ", R: " + R.Instance.SData.DoNotNeedToFaceTarget + NewLine +
                //"DrawSecondaryLineIndicator: " + "Q: " + Q.Instance.SData.DrawSecondaryLineIndicator + ", W: " + W.Instance.SData.DrawSecondaryLineIndicator + ", E: " + E.Instance.SData.DrawSecondaryLineIndicator + ", R: " + R.Instance.SData.DrawSecondaryLineIndicator + NewLine +
                "DynamicExtended: " + "Q: " + Q.Instance.SData.DynamicExtended + ", W: " + W.Instance.SData.DynamicExtended + ", E: " + E.Instance.SData.DynamicExtended + ", R: " + R.Instance.SData.DynamicExtended + NewLine +
                "DynamicTooltip: " + "Q: " + Q.Instance.SData.DynamicTooltip + ", W: " + W.Instance.SData.DynamicTooltip + ", E: " + E.Instance.SData.DynamicTooltip + ", R: " + R.Instance.SData.DynamicTooltip + NewLine +
                "ExcludedUnitTags: " + "Q: " + Q.Instance.SData.ExcludedUnitTags + ", W: " + W.Instance.SData.ExcludedUnitTags + ", E: " + E.Instance.SData.ExcludedUnitTags + ", R: " + R.Instance.SData.ExcludedUnitTags + NewLine +
                "Flags: " + "Q: " + Q.Instance.SData.Flags + ", W: " + W.Instance.SData.Flags + ", E: " + E.Instance.SData.Flags + ", R: " + R.Instance.SData.Flags + NewLine +
                "HaveAfterEffect: " + "Q: " + Q.Instance.SData.HaveAfterEffect + ", W: " + W.Instance.SData.HaveAfterEffect + ", E: " + E.Instance.SData.HaveAfterEffect + ", R: " + R.Instance.SData.HaveAfterEffect + NewLine +
                "HaveHitBone: " + "Q: " + Q.Instance.SData.HaveHitBone + ", W: " + W.Instance.SData.HaveHitBone + ", E: " + E.Instance.SData.HaveHitBone + ", R: " + R.Instance.SData.HaveHitBone + NewLine +
                "HaveHitEffect: " + "Q: " + Q.Instance.SData.HaveHitEffect + ", W: " + W.Instance.SData.HaveHitEffect + ", E: " + E.Instance.SData.HaveHitEffect + ", R: " + R.Instance.SData.HaveHitEffect + NewLine +
                "HavePointEffect: " + "Q: " + Q.Instance.SData.HavePointEffect + ", W: " + W.Instance.SData.HavePointEffect + ", E: " + E.Instance.SData.HavePointEffect + ", R: " + R.Instance.SData.HavePointEffect + NewLine +
                "HideRangeIndicatorWhenCasting: " + "Q: " + Q.Instance.SData.HideRangeIndicatorWhenCasting + ", W: " + W.Instance.SData.HideRangeIndicatorWhenCasting + ", E: " + E.Instance.SData.HideRangeIndicatorWhenCasting + ", R: " + R.Instance.SData.HideRangeIndicatorWhenCasting + NewLine +
                "HitBoneName: " + "Q: " + Q.Instance.SData.HitBoneName + ", W: " + W.Instance.SData.HitBoneName + ", E: " + E.Instance.SData.HitBoneName + ", R: " + R.Instance.SData.HitBoneName + NewLine +
                "HitEffectName: " + "Q: " + Q.Instance.SData.HitEffectName + ", W: " + W.Instance.SData.HitEffectName + ", E: " + E.Instance.SData.HitEffectName + ", R: " + R.Instance.SData.HitEffectName + NewLine +
                "HitEffectOrientType: " + "Q: " + Q.Instance.SData.HitEffectOrientType + ", W: " + W.Instance.SData.HitEffectOrientType + ", E: " + E.Instance.SData.HitEffectOrientType + ", R: " + R.Instance.SData.HitEffectOrientType + NewLine +
                "HitEffectPlayerName: " + "Q: " + Q.Instance.SData.HitEffectPlayerName + ", W: " + W.Instance.SData.HitEffectPlayerName + ", E: " + E.Instance.SData.HitEffectPlayerName + ", R: " + R.Instance.SData.HitEffectPlayerName + NewLine +
                "IgnoreAnimContinueUntilCastFrame: " + "Q: " + Q.Instance.SData.IgnoreAnimContinueUntilCastFrame + ", W: " + W.Instance.SData.IgnoreAnimContinueUntilCastFrame + ", E: " + E.Instance.SData.IgnoreAnimContinueUntilCastFrame + ", R: " + R.Instance.SData.IgnoreAnimContinueUntilCastFrame + NewLine +
                "IgnoreRangeCheck: " + "Q: " + Q.Instance.SData.IgnoreRangeCheck + ", W: " + W.Instance.SData.IgnoreRangeCheck + ", E: " + E.Instance.SData.IgnoreRangeCheck + ", R: " + R.Instance.SData.IgnoreRangeCheck + NewLine +
                "InventoryIcon: " + "Q: " + Q.Instance.SData.InventoryIcon + ", W: " + W.Instance.SData.InventoryIcon + ", E: " + E.Instance.SData.InventoryIcon + ", R: " + R.Instance.SData.InventoryIcon + NewLine +
                "IsDisabledWhileDead: " + "Q: " + Q.Instance.SData.IsDisabledWhileDead + ", W: " + W.Instance.SData.IsDisabledWhileDead + ", E: " + E.Instance.SData.IsDisabledWhileDead + ", R: " + R.Instance.SData.IsDisabledWhileDead + NewLine +
                "IsToggleSpell: " + "Q: " + Q.Instance.SData.IsToggleSpell + ", W: " + W.Instance.SData.IsToggleSpell + ", E: " + E.Instance.SData.IsToggleSpell + ", R: " + R.Instance.SData.IsToggleSpell + NewLine +
                "KeywordWhenAcquired: " + "Q: " + Q.Instance.SData.KeywordWhenAcquired + ", W: " + W.Instance.SData.KeywordWhenAcquired + ", E: " + E.Instance.SData.KeywordWhenAcquired + ", R: " + R.Instance.SData.KeywordWhenAcquired + NewLine +
                "LineDragLength: " + "Q: " + Q.Instance.SData.LineDragLength + ", W: " + W.Instance.SData.LineDragLength + ", E: " + E.Instance.SData.LineDragLength + ", R: " + R.Instance.SData.LineDragLength + NewLine +
                "LineMissileBounces: " + "Q: " + Q.Instance.SData.LineMissileBounces + ", W: " + W.Instance.SData.LineMissileBounces + ", E: " + E.Instance.SData.LineMissileBounces + ", R: " + R.Instance.SData.LineMissileBounces + NewLine +
                "LineMissileDelayDestroyAtEndSeconds: " + "Q: " + Q.Instance.SData.LineMissileDelayDestroyAtEndSeconds + ", W: " + W.Instance.SData.LineMissileDelayDestroyAtEndSeconds + ", E: " + E.Instance.SData.LineMissileDelayDestroyAtEndSeconds + ", R: " + R.Instance.SData.LineMissileDelayDestroyAtEndSeconds + NewLine +
                "LineMissileEndsAtTargetPoint: " + "Q: " + Q.Instance.SData.LineMissileEndsAtTargetPoint + ", W: " + W.Instance.SData.LineMissileEndsAtTargetPoint + ", E: " + E.Instance.SData.LineMissileEndsAtTargetPoint + ", R: " + R.Instance.SData.LineMissileEndsAtTargetPoint + NewLine +
                "LineMissileTargetHeightAugment: " + "Q: " + Q.Instance.SData.LineMissileTargetHeightAugment + ", W: " + W.Instance.SData.LineMissileTargetHeightAugment + ", E: " + E.Instance.SData.LineMissileTargetHeightAugment + ", R: " + R.Instance.SData.LineMissileTargetHeightAugment + NewLine +
                "LineMissileTimePulseBetweenCollisionSpellHits: " + "Q: " + Q.Instance.SData.LineMissileTimePulseBetweenCollisionSpellHits + ", W: " + W.Instance.SData.LineMissileTimePulseBetweenCollisionSpellHits + ", E: " + E.Instance.SData.LineMissileTimePulseBetweenCollisionSpellHits + ", R: " + R.Instance.SData.LineMissileTimePulseBetweenCollisionSpellHits + NewLine +
                "LineMissileTrackUnits: " + "Q: " + Q.Instance.SData.LineMissileTrackUnits + ", W: " + W.Instance.SData.LineMissileTrackUnits + ", E: " + E.Instance.SData.LineMissileTrackUnits + ", R: " + R.Instance.SData.LineMissileTrackUnits + NewLine +
                "LineMissileTrackUnitsAndContinues: " + "Q: " + Q.Instance.SData.LineMissileTrackUnitsAndContinues + ", W: " + W.Instance.SData.LineMissileTrackUnitsAndContinues + ", E: " + E.Instance.SData.LineMissileTrackUnitsAndContinues + ", R: " + R.Instance.SData.LineMissileTrackUnitsAndContinues + NewLine +
                "LineMissileUsesAccelerationForBounce: " + "Q: " + Q.Instance.SData.LineMissileUsesAccelerationForBounce + ", W: " + W.Instance.SData.LineMissileUsesAccelerationForBounce + ", E: " + E.Instance.SData.LineMissileUsesAccelerationForBounce + ", R: " + R.Instance.SData.LineMissileUsesAccelerationForBounce + NewLine +
                "LineTargetingBaseTextureOverrideName: " + "Q: " + Q.Instance.SData.LineTargetingBaseTextureOverrideName + ", W: " + W.Instance.SData.LineTargetingBaseTextureOverrideName + ", E: " + E.Instance.SData.LineTargetingBaseTextureOverrideName + ", R: " + R.Instance.SData.LineTargetingBaseTextureOverrideName + NewLine +
                "LineTargetingTargetTextureOverrideName: " + "Q: " + Q.Instance.SData.LineTargetingTargetTextureOverrideName + ", W: " + W.Instance.SData.LineTargetingTargetTextureOverrideName + ", E: " + E.Instance.SData.LineTargetingTargetTextureOverrideName + ", R: " + R.Instance.SData.LineTargetingTargetTextureOverrideName + NewLine +
                "LineWidth: " + "Q: " + Q.Instance.SData.LineWidth + ", W: " + W.Instance.SData.LineWidth + ", E: " + E.Instance.SData.LineWidth + ", R: " + R.Instance.SData.LineWidth + NewLine +
                "LocationTargettingWidth1: " + "Q: " + Q.Instance.SData.LocationTargettingWidth1 + ", W: " + W.Instance.SData.LocationTargettingWidth1 + ", E: " + E.Instance.SData.LocationTargettingWidth1 + ", R: " + R.Instance.SData.LocationTargettingWidth1 + NewLine +
                "LockConeToPlayer: " + "Q: " + Q.Instance.SData.LockConeToPlayer + ", W: " + W.Instance.SData.LockConeToPlayer + ", E: " + E.Instance.SData.LockConeToPlayer + ", R: " + R.Instance.SData.LockConeToPlayer + NewLine +
                "LookAtPolicy: " + "Q: " + Q.Instance.SData.LookAtPolicy + ", W: " + W.Instance.SData.LookAtPolicy + ", E: " + E.Instance.SData.LookAtPolicy + ", R: " + R.Instance.SData.LookAtPolicy + NewLine +
                "LuaOnMissileUpdateDistanceInterval: " + "Q: " + Q.Instance.SData.LuaOnMissileUpdateDistanceInterval + ", W: " + W.Instance.SData.LuaOnMissileUpdateDistanceInterval + ", E: " + E.Instance.SData.LuaOnMissileUpdateDistanceInterval + ", R: " + R.Instance.SData.LuaOnMissileUpdateDistanceInterval + NewLine +
                "MaxAmmo: " + "Q: " + Q.Instance.SData.MaxAmmo + ", W: " + W.Instance.SData.MaxAmmo + ", E: " + E.Instance.SData.MaxAmmo + ", R: " + R.Instance.SData.MaxAmmo + NewLine +
                "MaxGrowthLineBaseTextureName: " + "Q: " + Q.Instance.SData.MaxGrowthLineBaseTextureName + ", W: " + W.Instance.SData.MaxGrowthLineBaseTextureName + ", E: " + E.Instance.SData.MaxGrowthLineBaseTextureName + ", R: " + R.Instance.SData.MaxGrowthLineBaseTextureName + NewLine +
                "MaxGrowthLineTargetTextureName: " + "Q: " + Q.Instance.SData.MaxGrowthLineTargetTextureName + ", W: " + W.Instance.SData.MaxGrowthLineTargetTextureName + ", E: " + E.Instance.SData.MaxGrowthLineTargetTextureName + ", R: " + R.Instance.SData.MaxGrowthLineTargetTextureName + NewLine +
                "MaxGrowthRangeTextureName: " + "Q: " + Q.Instance.SData.MaxGrowthRangeTextureName + ", W: " + W.Instance.SData.MaxGrowthRangeTextureName + ", E: " + E.Instance.SData.MaxGrowthRangeTextureName + ", R: " + R.Instance.SData.MaxGrowthRangeTextureName + NewLine +
                "MaxHighlightTargets: " + "Q: " + Q.Instance.SData.MaxHighlightTargets + ", W: " + W.Instance.SData.MaxHighlightTargets + ", E: " + E.Instance.SData.MaxHighlightTargets + ", R: " + R.Instance.SData.MaxHighlightTargets + NewLine +
                "MinimapIcon: " + "Q: " + Q.Instance.SData.MinimapIcon + ", W: " + W.Instance.SData.MinimapIcon + ", E: " + E.Instance.SData.MinimapIcon + ", R: " + R.Instance.SData.MinimapIcon + NewLine +
                "MinimapIconDisplayFlag: " + "Q: " + Q.Instance.SData.MinimapIconDisplayFlag + ", W: " + W.Instance.SData.MinimapIconDisplayFlag + ", E: " + E.Instance.SData.MinimapIconDisplayFlag + ", R: " + R.Instance.SData.MinimapIconDisplayFlag + NewLine +
                "MinimapIconRotation: " + "Q: " + Q.Instance.SData.MinimapIconRotation + ", W: " + W.Instance.SData.MinimapIconRotation + ", E: " + E.Instance.SData.MinimapIconRotation + ", R: " + R.Instance.SData.MinimapIconRotation + NewLine +
                "MissileAccel: " + "Q: " + Q.Instance.SData.MissileAccel + ", W: " + W.Instance.SData.MissileAccel + ", E: " + E.Instance.SData.MissileAccel + ", R: " + R.Instance.SData.MissileAccel + NewLine +
                "MissileBlockTriggersOnDestroy: " + "Q: " + Q.Instance.SData.MissileBlockTriggersOnDestroy + ", W: " + W.Instance.SData.MissileBlockTriggersOnDestroy + ", E: " + E.Instance.SData.MissileBlockTriggersOnDestroy + ", R: " + R.Instance.SData.MissileBlockTriggersOnDestroy + NewLine +
                "MissileBoneName: " + "Q: " + Q.Instance.SData.MissileBoneName + ", W: " + W.Instance.SData.MissileBoneName + ", E: " + E.Instance.SData.MissileBoneName + ", R: " + R.Instance.SData.MissileBoneName + NewLine +
                "MissileEffect: " + "Q: " + Q.Instance.SData.MissileEffect + ", W: " + W.Instance.SData.MissileEffect + ", E: " + E.Instance.SData.MissileEffect + ", R: " + R.Instance.SData.MissileEffect + NewLine +
                "MissileEffectEnemy: " + "Q: " + Q.Instance.SData.MissileEffectEnemy + ", W: " + W.Instance.SData.MissileEffectEnemy + ", E: " + E.Instance.SData.MissileEffectEnemy + ", R: " + R.Instance.SData.MissileEffectEnemy + NewLine +
                "MissileEffectPlayer: " + "Q: " + Q.Instance.SData.MissileEffectPlayer + ", W: " + W.Instance.SData.MissileEffectPlayer + ", E: " + E.Instance.SData.MissileEffectPlayer + ", R: " + R.Instance.SData.MissileEffectPlayer + NewLine +
                "MissileFixedTravelTime: " + "Q: " + Q.Instance.SData.MissileFixedTravelTime + ", W: " + W.Instance.SData.MissileFixedTravelTime + ", E: " + E.Instance.SData.MissileFixedTravelTime + ", R: " + R.Instance.SData.MissileFixedTravelTime + NewLine +
                "MissileFollowsTerrainHeight: " + "Q: " + Q.Instance.SData.MissileFollowsTerrainHeight + ", W: " + W.Instance.SData.MissileFollowsTerrainHeight + ", E: " + E.Instance.SData.MissileFollowsTerrainHeight + ", R: " + R.Instance.SData.MissileFollowsTerrainHeight + NewLine +
                "MissileGravity: " + "Q: " + Q.Instance.SData.MissileGravity + ", W: " + W.Instance.SData.MissileGravity + ", E: " + E.Instance.SData.MissileGravity + ", R: " + R.Instance.SData.MissileGravity + NewLine +
                "MissileLifetime: " + "Q: " + Q.Instance.SData.MissileLifetime + ", W: " + W.Instance.SData.MissileLifetime + ", E: " + E.Instance.SData.MissileLifetime + ", R: " + R.Instance.SData.MissileLifetime + NewLine +
                "MissileMaxSpeed: " + "Q: " + Q.Instance.SData.MissileMaxSpeed + ", W: " + W.Instance.SData.MissileMaxSpeed + ", E: " + E.Instance.SData.MissileMaxSpeed + ", R: " + R.Instance.SData.MissileMaxSpeed + NewLine +
                "MissileMinSpeed: " + "Q: " + Q.Instance.SData.MissileMinSpeed + ", W: " + W.Instance.SData.MissileMinSpeed + ", E: " + E.Instance.SData.MissileMinSpeed + ", R: " + R.Instance.SData.MissileMinSpeed + NewLine +
                //"MissileMinTravelTime: " + "Q: " + Q.Instance.SData.MissileMinTravelTime + ", W: " + W.Instance.SData.MissileMinTravelTime + ", E: " + E.Instance.SData.MissileMinTravelTime + ", R: " + R.Instance.SData.MissileMinTravelTime + NewLine +
                "MissilePerceptionBubbleRadius: " + "Q: " + Q.Instance.SData.MissilePerceptionBubbleRadius + ", W: " + W.Instance.SData.MissilePerceptionBubbleRadius + ", E: " + E.Instance.SData.MissilePerceptionBubbleRadius + ", R: " + R.Instance.SData.MissilePerceptionBubbleRadius + NewLine +
                "MissilePerceptionBubbleRevealsStealth: " + "Q: " + Q.Instance.SData.MissilePerceptionBubbleRevealsStealth + ", W: " + W.Instance.SData.MissilePerceptionBubbleRevealsStealth + ", E: " + E.Instance.SData.MissilePerceptionBubbleRevealsStealth + ", R: " + R.Instance.SData.MissilePerceptionBubbleRevealsStealth + NewLine +
                "MissileSpeed: " + "Q: " + Q.Instance.SData.MissileSpeed + ", W: " + W.Instance.SData.MissileSpeed + ", E: " + E.Instance.SData.MissileSpeed + ", R: " + R.Instance.SData.MissileSpeed + NewLine +
                "MissileTargetHeightAugment: " + "Q: " + Q.Instance.SData.MissileTargetHeightAugment + ", W: " + W.Instance.SData.MissileTargetHeightAugment + ", E: " + E.Instance.SData.MissileTargetHeightAugment + ", R: " + R.Instance.SData.MissileTargetHeightAugment + NewLine +
                "MissileUnblockable: " + "Q: " + Q.Instance.SData.MissileUnblockable + ", W: " + W.Instance.SData.MissileUnblockable + ", E: " + E.Instance.SData.MissileUnblockable + ", R: " + R.Instance.SData.MissileUnblockable + NewLine +
                "NoWinddownIfCancelled: " + "Q: " + Q.Instance.SData.NoWinddownIfCancelled + ", W: " + W.Instance.SData.NoWinddownIfCancelled + ", E: " + E.Instance.SData.NoWinddownIfCancelled + ", R: " + R.Instance.SData.NoWinddownIfCancelled + NewLine +
                "OrientRadiusTextureFromPlayer: " + "Q: " + Q.Instance.SData.OrientRadiusTextureFromPlayer + ", W: " + W.Instance.SData.OrientRadiusTextureFromPlayer + ", E: " + E.Instance.SData.OrientRadiusTextureFromPlayer + ", R: " + R.Instance.SData.OrientRadiusTextureFromPlayer + NewLine +
                "OrientRangeIndicatorToCursor: " + "Q: " + Q.Instance.SData.OrientRangeIndicatorToCursor + ", W: " + W.Instance.SData.OrientRangeIndicatorToCursor + ", E: " + E.Instance.SData.OrientRangeIndicatorToCursor + ", R: " + R.Instance.SData.OrientRangeIndicatorToCursor + NewLine +
                "OrientRangeIndicatorToFacing: " + "Q: " + Q.Instance.SData.OrientRangeIndicatorToFacing + ", W: " + W.Instance.SData.OrientRangeIndicatorToFacing + ", E: " + E.Instance.SData.OrientRangeIndicatorToFacing + ", R: " + R.Instance.SData.OrientRangeIndicatorToFacing + NewLine +
                "OverrideCastTime: " + "Q: " + Q.Instance.SData.OverrideCastTime + ", W: " + W.Instance.SData.OverrideCastTime + ", E: " + E.Instance.SData.OverrideCastTime + ", R: " + R.Instance.SData.OverrideCastTime + NewLine +
                "ParticleStartOffset: " + "Q: " + Q.Instance.SData.ParticleStartOffset + ", W: " + W.Instance.SData.ParticleStartOffset + ", E: " + E.Instance.SData.ParticleStartOffset + ", R: " + R.Instance.SData.ParticleStartOffset + NewLine +
                "PointEffectName: " + "Q: " + Q.Instance.SData.PointEffectName + ", W: " + W.Instance.SData.PointEffectName + ", E: " + E.Instance.SData.PointEffectName + ", R: " + R.Instance.SData.PointEffectName + NewLine +
                "RequiredUnitTags: " + "Q: " + Q.Instance.SData.RequiredUnitTags + ", W: " + W.Instance.SData.RequiredUnitTags + ", E: " + E.Instance.SData.RequiredUnitTags + ", R: " + R.Instance.SData.RequiredUnitTags + NewLine +
                "SelectionPreference: " + "Q: " + Q.Instance.SData.SelectionPreference + ", W: " + W.Instance.SData.SelectionPreference + ", E: " + E.Instance.SData.SelectionPreference + ", R: " + R.Instance.SData.SelectionPreference + NewLine +
                "Sound_VOEventCategory: " + "Q: " + Q.Instance.SData.Sound_VOEventCategory + ", W: " + W.Instance.SData.Sound_VOEventCategory + ", E: " + E.Instance.SData.Sound_VOEventCategory + ", R: " + R.Instance.SData.Sound_VOEventCategory + NewLine +
                "SpellCastTime: " + "Q: " + Q.Instance.SData.SpellCastTime + ", W: " + W.Instance.SData.SpellCastTime + ", E: " + E.Instance.SData.SpellCastTime + ", R: " + R.Instance.SData.SpellCastTime + NewLine +
                "SpellRevealsChampion: " + "Q: " + Q.Instance.SData.SpellRevealsChampion + ", W: " + W.Instance.SData.SpellRevealsChampion + ", E: " + E.Instance.SData.SpellRevealsChampion + ", R: " + R.Instance.SData.SpellRevealsChampion + NewLine +
                "SpellTotalTime: " + "Q: " + Q.Instance.SData.SpellTotalTime + ", W: " + W.Instance.SData.SpellTotalTime + ", E: " + E.Instance.SData.SpellTotalTime + ", R: " + R.Instance.SData.SpellTotalTime + NewLine +
                "StartCooldown: " + "Q: " + Q.Instance.SData.StartCooldown + ", W: " + W.Instance.SData.StartCooldown + ", E: " + E.Instance.SData.StartCooldown + ", R: " + R.Instance.SData.StartCooldown + NewLine +
                "TargeterConstrainedToRange: " + "Q: " + Q.Instance.SData.TargeterConstrainedToRange + ", W: " + W.Instance.SData.TargeterConstrainedToRange + ", E: " + E.Instance.SData.TargeterConstrainedToRange + ", R: " + R.Instance.SData.TargeterConstrainedToRange + NewLine +
                "TargettingType: " + "Q: " + Q.Instance.SData.TargettingType + ", W: " + W.Instance.SData.TargettingType + ", E: " + E.Instance.SData.TargettingType + ", R: " + R.Instance.SData.TargettingType + NewLine +
                "UpdateRotationWhenCasting: " + "Q: " + Q.Instance.SData.UpdateRotationWhenCasting + ", W: " + W.Instance.SData.UpdateRotationWhenCasting + ", E: " + E.Instance.SData.UpdateRotationWhenCasting + ", R: " + R.Instance.SData.UpdateRotationWhenCasting + NewLine +
                "UseAnimatorFramerate: " + "Q: " + Q.Instance.SData.UseAnimatorFramerate + ", W: " + W.Instance.SData.UseAnimatorFramerate + ", E: " + E.Instance.SData.UseAnimatorFramerate + ", R: " + R.Instance.SData.UseAnimatorFramerate + NewLine +
                "UseAutoattackCastTime: " + "Q: " + Q.Instance.SData.UseAutoattackCastTime + ", W: " + W.Instance.SData.UseAutoattackCastTime + ", E: " + E.Instance.SData.UseAutoattackCastTime + ", R: " + R.Instance.SData.UseAutoattackCastTime + NewLine +
                "UseChargeChanneling: " + "Q: " + Q.Instance.SData.UseChargeChanneling + ", W: " + W.Instance.SData.UseChargeChanneling + ", E: " + E.Instance.SData.UseChargeChanneling + ", R: " + R.Instance.SData.UseChargeChanneling + NewLine +
                "UseChargeTargeting: " + "Q: " + Q.Instance.SData.UseChargeTargeting + ", W: " + W.Instance.SData.UseChargeTargeting + ", E: " + E.Instance.SData.UseChargeTargeting + ", R: " + R.Instance.SData.UseChargeTargeting + NewLine +
                "UseGlobalLineIndicator: " + "Q: " + Q.Instance.SData.UseGlobalLineIndicator + ", W: " + W.Instance.SData.UseGlobalLineIndicator + ", E: " + E.Instance.SData.UseGlobalLineIndicator + ", R: " + R.Instance.SData.UseGlobalLineIndicator + NewLine +
                "UseMinimapTargeting: " + "Q: " + Q.Instance.SData.UseMinimapTargeting + ", W: " + W.Instance.SData.UseMinimapTargeting + ", E: " + E.Instance.SData.UseMinimapTargeting + ", R: " + R.Instance.SData.UseMinimapTargeting + NewLine +
                "";

            Text.OnEndScene();
        }
    }
}