using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;

namespace AdjustableBandits
{
	public class MCMSettings : AttributeGlobalSettings<MCMSettings>
	{
		public override string Id => "AdjustableBandits";

		public override string DisplayName => "Adjustable Bandits";

		public override string FolderName => "AdjustableBandits";

		public override string FormatType => "json";


		// --- BANDIT POPULATION

		[SettingPropertyFloatingInteger(
			"Bandit Party Size Multiplier", 
			0.01f, 
			10.0f, 
			"0%", 
			RequireRestart = false,
			HintText = "Adjusts the size of all bandit parties. Only newly spawned parties are affected. High values may decrease performance and may have negative influence on game experience. [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup("Bandit Population")]
		public float BanditMultiplier { get; set; } = 1f;


		[SettingPropertyInteger(
			"Maximum Number of Looter Parties",
			50,
			600,
			"0 Parties",
			RequireRestart = false,
			HintText = "[Native: 150]",
			Order = 1)]
		[SettingPropertyGroup("Bandit Population")]
		public int NumberOfMaximumLooterParties { get; set; } = 150;

		[SettingPropertyInteger(
			"Maximum Parties around Hideout",
			1,
			32,
			"0 Parties",
			RequireRestart = false,
			HintText = "[Native: 8]",
			Order = 2)]
		[SettingPropertyGroup("Bandit Population")]
		public int NumberOfMaximumBanditPartiesAroundEachHideout { get; set; } = 8;


		// --- HIDEOUTS

		[SettingPropertyInteger(
			"Initial Hideouts per Faction",
			1,
			15,
			"0 Hideouts",
			RequireRestart = false,
			HintText = "[Native: 3]",
			Order = 0)]
		[SettingPropertyGroup("Hideouts")]
		public int NumberOfInitialHideoutsAtEachBanditFaction { get; set; } = 3;

		[SettingPropertyInteger(
			"Maximum Hideouts per Faction",
			1,
			25,
			"0 Hideouts",
			RequireRestart = false,
			HintText = "[Native: 10]",
			Order = 1)]
		[SettingPropertyGroup("Hideouts")]
		public int NumberOfMaximumHideoutsAtEachBanditFaction { get; set; } = 10;

		[SettingPropertyInteger(
			"Minimum Parties to Infest Hideout",
			1,
			10,
			"0 Parties",
			RequireRestart = false,
			HintText = "[Native: 2]",
			Order = 2)]
		[SettingPropertyGroup("Hideouts")]
		public int NumberOfMinimumBanditPartiesInAHideoutToInfestIt { get; set; } = 2;

		[SettingPropertyInteger(
			"Maximum Parties in Hideout",
			1,
			10,
			"0 Parties",
			RequireRestart = false,
			HintText = "[Native: 4]",
			Order = 3)]
		[SettingPropertyGroup("Hideouts")]
		public int NumberOfMaximumBanditPartiesInEachHideout { get; set; } = 4;

		[SettingPropertyFloatingInteger(
			"Maximum Troop Count in First Fight - Factor",
			0f,
			10f,
			"0.0",
			RequireRestart = false,
			HintText = "Actual count depends on number of troops in hideout and player progress (lowest maximum is 12 with player progress at 0). Set to 0 for unlimited. [Native: 1.0]",
			Order = 4)]
		[SettingPropertyGroup("Hideouts")]
		public float NumberOfMaximumTroopCountForFirstFightInHideoutFactor { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Maximum Troop Count in Boss Fight - Factor",
			0f,
			10f,
			"0.0",
			RequireRestart = false,
			HintText = "Actual count depends on number of troops in hideout and player progress (lowest maximum is 6 with player progress at 0). Set to 0 for unlimited. [Native: 1.0]",
			Order = 5)]
		[SettingPropertyGroup("Hideouts")]
		public float NumberOfMaximumTroopCountForBossFightInHideoutFactor { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Spawn Percentage in First Fight",
			0f,
			0.99f,
			"0%",
			RequireRestart = false,
			HintText = "Percentage of troops to be spawned in the initial fight before the boss battle. [Native: 75%]",
			Order = 6)]
		[SettingPropertyGroup("Hideouts")]
		public float SpawnPercentageForFirstFightInHideoutMission { get; set; } = 0.75f;


		[SettingPropertyInteger(
			"Minimum Bandits Troops in Hideout Mission",
			1,
			100,
			"0 Troops",
			RequireRestart = false,
			HintText = "[Native: 10]",
			Order = 7)]
		[SettingPropertyGroup("Hideouts")]
		public int NumberOfMinimumBanditTroopsInHideoutMission { get; set; } = 10;

		[SettingPropertyInteger(
			"Maximum Player Troops in Hideout Mission",
			1,
			100,
			"0 Troops",
			RequireRestart = false,
			HintText = "[Native: 10]",
			Order = 8)]
		[SettingPropertyGroup("Hideouts")]
		public int PlayerMaximumTroopCountForHideoutMission { get; set; } = 10;
	}
}
