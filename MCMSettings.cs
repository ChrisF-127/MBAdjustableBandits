﻿using MCM.Abstractions.Attributes;
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

		#region GENERAL
		//private const string GeneralGroupName = "{=adjban_group_GeneralSettings}General Settings";
		#endregion

		#region BANDIT POPULATION
		private const string BanditPopulationGroupName = "{=adjban_group_BanditPopulation}Bandit Population";

		[SettingPropertyFloatingInteger(
			"{=adjban_name_BanditPartySizeMultiplier}Bandit Party Size Multiplier", 
			0.01f, 
			100.0f, 
			"0.00", 
			RequireRestart = false,
			HintText = "{=adjban_hint_BanditPartySizeMultiplier}Adjusts the size of all bandit parties. Only newly spawned parties are affected. High values may decrease performance and may have negative influence on game experience. [Default: 1.00]",
			Order = 0)]
		[SettingPropertyGroup(
			BanditPopulationGroupName, 
			GroupOrder = 1)]
		public float BanditMultiplier { get; set; } = 1f;

		[SettingPropertyInteger(
			"{=adjban_name_BanditPartySizeLimit}Bandit Party Size Limit (affects Movement Speed)",
			20,
			1000,
			"0",
			RequireRestart = false,
			HintText =
			"{=adjban_hint_BanditPartySizeLimit}While it does not make the bandit party's troops desert, being over the limit slows down the party. A higher limit lessens or removes the malus, increasing movement speed. (The party slows down if it has more troops than the size limit.) [Default: 20]",
			Order = 1)]
		[SettingPropertyGroup(
			BanditPopulationGroupName,
			GroupOrder = 1)]
		public int BanditPartySizeLimit { get; set; } = 20;


		[SettingPropertyInteger(
			"{=adjban_name_MaxNumLooterParties}Maximum Number of Looter Parties",
			50,
			600,
			"{=adjban_format_Parties}0 Parties",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxNumLooterParties}Maximum number of looter parties on the entire world map. [Default: 150]",
			Order = 2)]
		[SettingPropertyGroup(
			BanditPopulationGroupName,
			GroupOrder = 1)]
		public int NumberOfMaximumLooterParties { get; set; } = 150;

		[SettingPropertyInteger(
			"{=adjban_name_MaxPartiesAroundHideout}Maximum Parties around Hideout",
			1,
			32,
			"{=adjban_format_Parties}0 Parties",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxPartiesAroundHideout}Maximum number of faction-parties around hideouts. [Default: 8]",
			Order = 3)]
		[SettingPropertyGroup(
			BanditPopulationGroupName,
			GroupOrder = 1)]
		public int NumberOfMaximumBanditPartiesAroundEachHideout { get; set; } = 8;
		#endregion

		#region HIDEOUTS
		private const string HideoutsGroupName = "{=adjban_group_Hideouts}Hideouts";

		[SettingPropertyInteger(
			"{=adjban_name_InitialHideoutsPerFaction}Initial Hideouts per Faction",
			1,
			15,
			"{=adjban_format_Hideouts}0 Hideouts",
			RequireRestart = false,
			HintText = "{=adjban_hint_InitialHideoutsPerFaction}[Default: 3]",
			Order = 0)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int NumberOfInitialHideoutsAtEachBanditFaction { get; set; } = 3;

		[SettingPropertyInteger(
			"{=adjban_name_MaxHideoutsPerFaction}Maximum Hideouts per Faction",
			1,
			25,
			"{=adjban_format_Hideouts}0 Hideouts",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxHideoutsPerFaction}[Default: 10]",
			Order = 1)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int NumberOfMaximumHideoutsAtEachBanditFaction { get; set; } = 10;

		[SettingPropertyInteger(
			"{=adjban_name_MinPartiesToInfestHideout}Minimum Parties to Infest Hideout",
			1,
			10,
			"{=adjban_format_Parties}0 Parties",
			RequireRestart = false,
			HintText = "{=adjban_hint_MinPartiesToInfestHideout}[Default: 2]",
			Order = 2)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int NumberOfMinimumBanditPartiesInAHideoutToInfestIt { get; set; } = 2;

		[SettingPropertyInteger(
			"{=adjban_name_MaxPartiesInHideout}Maximum Parties in Hideout",
			1,
			10,
			"{=adjban_format_Parties}0 Parties",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxPartiesInHideout}[Default: 4]",
			Order = 3)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int NumberOfMaximumBanditPartiesInEachHideout { get; set; } = 4;

		[SettingPropertyFloatingInteger(
			"{=adjban_name_MaxTroopCountFirstFight}Maximum Troop Count in First Fight - Factor",
			0f,
			10f,
			"0.0",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxTroopCountFirstFight}Actual count depends on number of troops in hideout and player progress (lowest maximum is 12 with player progress at 0). Set to 0 for unlimited. [Default: 1.0]",
			Order = 4)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public float NumberOfMaximumTroopCountForFirstFightInHideoutFactor { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"{=adjban_name_MaxTroopCountBossFight}Maximum Troop Count in Boss Fight - Factor",
			0f,
			10f,
			"0.0",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxTroopCountBossFight}Actual count depends on number of troops in hideout and player progress (lowest maximum is 6 with player progress at 0). Set to 0 for unlimited. [Default: 1.0]",
			Order = 5)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public float NumberOfMaximumTroopCountForBossFightInHideoutFactor { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"{=adjban_name_SpawnPercFirstFight}Spawn Percentage in First Fight",
			0f,
			0.99f,
			"0%",
			RequireRestart = false,
			HintText = "{=adjban_hint_SpawnPercFirstFight}Percentage of troops to be spawned in the initial fight before the boss battle. [Default: 75%]",
			Order = 6)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public float SpawnPercentageForFirstFightInHideoutMission { get; set; } = 0.75f;


		[SettingPropertyInteger(
			"{=adjban_name_MinBanditTroopsHideoutMission}Minimum Bandit Troops in Hideout Mission",
			1,
			100,
			"{=adjban_format_Troops}0 Troops",
			RequireRestart = false,
			HintText = "{=adjban_hint_MinBanditTroopsHideoutMission}[Default: 10]",
			Order = 7)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int NumberOfMinimumBanditTroopsInHideoutMission { get; set; } = 10;

		[SettingPropertyInteger(
			"{=adjban_name_MaxPlayerTroopsHideoutMission}Maximum Player Troops in Hideout Mission",
			1,
			100,
			"{=adjban_format_Troops}0 Troops",
			RequireRestart = false,
			HintText = "{=adjban_hint_MaxPlayerTroopsHideoutMission}[Default: 10]",
			Order = 8)]
		[SettingPropertyGroup(
			HideoutsGroupName,
			GroupOrder = 2)]
		public int PlayerMaximumTroopCountForHideoutMission { get; set; } = 10;
		#endregion
	}
}
