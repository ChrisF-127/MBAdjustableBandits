using MCM.Abstractions.Settings.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MoreBandits
{
	public class Main : MBSubModuleBase
	{
		public static MCMSettings Settings { get; private set; }

		private bool isInitialized = false;

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
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

		public static void ModifyVariables(MobileParty mobileParty, ref float f, ref float f2, ref float f3)
		{
			// TODO match faction to faction specific multiplier
			var modifier = Settings.BanditMultiplier;

			f = Limit(f * modifier);
			f2 = Limit(f2 * modifier);
			f3 = Limit(f3 * modifier);
		}
		private static float Limit(float f) => f > 1f ? f : 1f;
	}
}
