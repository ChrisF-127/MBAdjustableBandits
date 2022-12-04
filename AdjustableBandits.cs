using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace AdjustableBandits
{
	public class AdjustableBandits : MBSubModuleBase
	{
		public static MCMSettings Settings { get; private set; }

		private bool isInitialized = false;

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			try
			{
				base.OnGameStart(game, gameStarterObject);
				if (game.GameType is Campaign)
				{
					if (gameStarterObject is CampaignGameStarter campaignGameStarter)
						campaignGameStarter.AddModel(new AdjustableBanditsDensityModel());
					else
						throw new Exception($"Unknown {nameof(gameStarterObject)}: '{gameStarterObject?.GetType()}'");
				}
			}
			catch (Exception exc)
			{
				var text = $"ERROR: Adjustable Bandits failed to initialize ({nameof(OnGameStart)}):";
				InformationManager.DisplayMessage(new InformationMessage(text + exc.GetType().ToString(), new Color(1f, 0f, 0f)));
				FileLog.Log(text + "\n" + exc.ToString());
			}
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			try
			{
				base.OnBeforeInitialModuleScreenSetAsRoot();
				if (isInitialized)
					return;

				Settings = GlobalSettings<MCMSettings>.Instance;
				if (Settings == null)
					throw new Exception("Settings is null");

				HarmonyPatches.Initialize();
				isInitialized = true;
			}
			catch (Exception exc)
			{
				var text = $"ERROR: Adjustable Bandits failed to initialize ({nameof(OnBeforeInitialModuleScreenSetAsRoot)}):";
				InformationManager.DisplayMessage(new InformationMessage(text + exc.GetType().ToString(), new Color(1f, 0f, 0f)));
				FileLog.Log(text + "\n" + exc.ToString());
			}
		}

		public static void ModifyVariables(MobileParty mobileParty, ref float f, ref float f2, ref float f3)
		{
			// TODO match faction to faction specific multiplier
			var modifier = Settings.BanditMultiplier;

			//string s = $"{mobileParty.MapFaction}\nbefore:\t{f:0.000}\t{f2:0.000}\t{f3:0.000}";
			f = Limit(f * modifier);
			f2 = Limit(f2 * modifier);
			f3 = Limit(f3 * modifier);
			//FileLog.Log(s + $" after:\t{f:0.000}\t{f2:0.000}\t{f3:0.000}");
		}
		private static float Limit(float f) => f > 1f ? f : 1f;
	}
}
