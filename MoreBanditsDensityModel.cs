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

namespace MoreBandits
{
	internal class MoreBanditsDensityModel : DefaultBanditDensityModel
	{
		public override int NumberOfMaximumLooterParties =>
			Main.Settings.NumberOfMaximumLooterParties;

		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt =>
			Main.Settings.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;

		public override int NumberOfMaximumBanditPartiesInEachHideout =>
			Main.Settings.NumberOfMaximumBanditPartiesInEachHideout;

		public override int NumberOfMaximumBanditPartiesAroundEachHideout =>
			Main.Settings.NumberOfMaximumBanditPartiesAroundEachHideout;

		public override int NumberOfMaximumHideoutsAtEachBanditFaction =>
			Main.Settings.NumberOfMaximumHideoutsAtEachBanditFaction;

		public override int NumberOfInitialHideoutsAtEachBanditFaction =>
			Main.Settings.NumberOfInitialHideoutsAtEachBanditFaction;

		public override int NumberOfMinimumBanditTroopsInHideoutMission => 
			Main.Settings.NumberOfMinimumBanditTroopsInHideoutMission;

		//public override int NumberOfMaximumTroopCountForFirstFightInHideout => 
		//	MathF.Floor(6f * (2f + Campaign.Current.PlayerProgress));
		//public override int NumberOfMaximumTroopCountForBossFightInHideout => 
		//	MathF.Floor(1f + 5f * (1f + Campaign.Current.PlayerProgress));
		//public override float SpawnPercentageForFirstFightInHideoutMission => 
		//	0.75f;

		public override int GetPlayerMaximumTroopCountForHideoutMission(MobileParty party)
		{
			float troopCount = Main.Settings.PlayerMaximumTroopCountForHideoutMission;
			if (party.HasPerk(DefaultPerks.Tactics.SmallUnitTactics))
				troopCount += DefaultPerks.Tactics.SmallUnitTactics.PrimaryBonus;
			return MathF.Round(troopCount);
		}
	}
}
