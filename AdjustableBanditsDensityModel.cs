using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace AdjustableBandits
{
	internal class AdjustableBanditsDensityModel : DefaultBanditDensityModel
	{
		public override int NumberOfMaximumLooterParties =>
			AdjustableBandits.Settings.NumberOfMaximumLooterParties;

		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt =>
			AdjustableBandits.Settings.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;

		public override int NumberOfMaximumBanditPartiesInEachHideout =>
			AdjustableBandits.Settings.NumberOfMaximumBanditPartiesInEachHideout;

		public override int NumberOfMaximumBanditPartiesAroundEachHideout =>
			AdjustableBandits.Settings.NumberOfMaximumBanditPartiesAroundEachHideout;

		public override int NumberOfMaximumHideoutsAtEachBanditFaction =>
			AdjustableBandits.Settings.NumberOfMaximumHideoutsAtEachBanditFaction;

		public override int NumberOfInitialHideoutsAtEachBanditFaction =>
			AdjustableBandits.Settings.NumberOfInitialHideoutsAtEachBanditFaction;

		public override int NumberOfMinimumBanditTroopsInHideoutMission => 
			AdjustableBandits.Settings.NumberOfMinimumBanditTroopsInHideoutMission;

		public override int NumberOfMaximumTroopCountForFirstFightInHideout =>  // MathF.Floor(6f * (2f + Campaign.Current.PlayerProgress))
			AdjustableBandits.Settings.NumberOfMaximumTroopCountForFirstFightInHideoutFactor <= 0f ? 65536 :
			(int)(base.NumberOfMaximumTroopCountForFirstFightInHideout * AdjustableBandits.Settings.NumberOfMaximumTroopCountForFirstFightInHideoutFactor);
		public override int NumberOfMaximumTroopCountForBossFightInHideout =>   // MathF.Floor(1f + 5f * (1f + Campaign.Current.PlayerProgress))
			AdjustableBandits.Settings.NumberOfMaximumTroopCountForBossFightInHideoutFactor <= 0f ? 65536 :
			(int)(base.NumberOfMaximumTroopCountForBossFightInHideout * AdjustableBandits.Settings.NumberOfMaximumTroopCountForBossFightInHideoutFactor);
		public override float SpawnPercentageForFirstFightInHideoutMission =>
			AdjustableBandits.Settings.SpawnPercentageForFirstFightInHideoutMission;

		public override int GetPlayerMaximumTroopCountForHideoutMission(MobileParty party)
		{
			float troopCount = AdjustableBandits.Settings.PlayerMaximumTroopCountForHideoutMission;
			if (party.HasPerk(DefaultPerks.Tactics.SmallUnitTactics))
				troopCount += DefaultPerks.Tactics.SmallUnitTactics.PrimaryBonus;
			return MathF.Round(troopCount);
		}
	}
}
