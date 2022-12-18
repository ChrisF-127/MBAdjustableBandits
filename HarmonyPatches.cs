using HarmonyLib;
using Helpers;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace AdjustableBandits
{
	internal static class HarmonyPatches
	{
		public static void Initialize()
		{
			var harmony = new Harmony("sy.adjustablebandits");

			harmony.Patch(AccessTools.Method(typeof(MobileParty), "FillPartyStacks"), 
				transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.MobileParty_FillPartyStacks_Transpiler)));
			harmony.Patch(AccessTools.Method(typeof(DefaultPartySizeLimitModel), "CalculateMobilePartyMemberSizeLimit"),
				postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.DefaultPartySizeLimitModel_CalculateMobilePartyMemberSizeLimit_Postfix)));
		}

		private static IEnumerable<CodeInstruction> MobileParty_FillPartyStacks_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			bool applied = false;
			LocalBuilder lb_f = null, lb_f2 = null, lb_f3 = null;
			var list = new List<CodeInstruction>(instructions);
			for (int i = 0; i < list.Count - 2; i++)
			{
				if (list[i].opcode == OpCodes.Stloc_S
					&& list[i].operand is LocalBuilder lb
					&& lb.LocalType == typeof(float))
				{
					if (lb.LocalIndex == 7)
						lb_f = lb;
					else if (lb.LocalIndex == 8)
						lb_f2 = lb;
					else if (lb.LocalIndex == 9)
					{
						lb_f3 = lb;

						if (lb_f != null && lb_f2 != null && lb_f3 != null)
						{
							// insert after "Stloc_S 9"
							list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f));  // 7
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f2)); // 8
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f3)); // 9
							list.Insert(++i, new CodeInstruction(OpCodes.Call, typeof(AdjustableBandits).GetMethod(nameof(AdjustableBandits.ModifyVariables), BindingFlags.Static | BindingFlags.Public)));
							applied = true;
							break;
						}
					}
				}
			}

			if (!applied)
				throw new Exception($"{nameof(AdjustableBandits)}: failed to apply Harmony-patch '{nameof(MobileParty_FillPartyStacks_Transpiler)}'");
			return list;
		}

		private static void DefaultPartySizeLimitModel_CalculateMobilePartyMemberSizeLimit_Postfix(ref ExplainedNumber __result, MobileParty party)
		{
			if (party.IsBandit)
			{
				const int baseLimit = 20;
				var sizeLimit = AdjustableBandits.Settings.BanditPartySizeLimit;
				if (sizeLimit > baseLimit)
					__result.Add(sizeLimit - baseLimit, new TextObject("Bandit Bonus"));
			}
		}
	}
}
