using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace MoreBandits
{
	internal static class HarmonyPatches
	{
		private static bool _initialized = false;
		public static void Initialize()
		{
			if (_initialized)
				return;
			_initialized = true;

			try
			{
				var method = typeof(MobileParty).GetMethod("FillPartyStacks", BindingFlags.NonPublic | BindingFlags.Instance);
				var patches = Harmony.GetPatchInfo(method);
				if (patches?.Transpilers?.Count > 0)
				{
					string output = "";
					foreach (var patch in patches.Transpilers)
						output += patch.ToString();
					FileLog.Log("MoreBandits: Warning: transpiler patches detected: " + output);
				}

				var harmony = new Harmony("com.sy.morebandits");

				// Patch MobileParty.FillPartyStacks
				harmony.Patch(method, transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_MobileParty_FillPartyStacks)));

				// Disable NoBanditsNoCry.FillPartyStacks patch
				method = Type.GetType("NoBanditsNoCry.Patches.FillPartyStacks, NoBanditsNoCry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")?.GetMethod("Prefix", BindingFlags.Public | BindingFlags.Static);
				if (method != null)
					harmony.Patch(method, transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_NoBanditsNoCry_FillPartyStacks)));


				//harmony.Patch(
				//	typeof(TestClass).GetMethod("Test_ori", BindingFlags.Instance | BindingFlags.NonPublic),
				//	transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_MobileParty_FillPartyStacks)));
				//harmony.Patch(
				//	typeof(TestClass).GetMethod("Test_mod", BindingFlags.Instance | BindingFlags.NonPublic),
				//	transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_MobileParty_FillPartyStacks)));
			}
			catch (Exception e)
			{
				FileLog.Log(e.ToString());
			}
		}

		private static IEnumerable<CodeInstruction> Transpiler_MobileParty_FillPartyStacks(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			//FileLog.Log("");

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
							list.Insert(++i, new CodeInstruction(OpCodes.Call, typeof(Main).GetMethod(nameof(Main.ModifyVariables), BindingFlags.Static | BindingFlags.Public)));
							applied = true;
							break;
						}
					}
				}
			}

			//foreach (var instruction in list)
			//	FileLog.Log(instruction.ToString());
			if (!applied)
				throw new Exception("MoreBandits: failed to apply Harmony-patch 'Transpiler_MobileParty_FillPartyStacks'");
			return list;
		}

		private static IEnumerable<CodeInstruction> Transpiler_NoBanditsNoCry_FillPartyStacks(IEnumerable<CodeInstruction> instructions)
		{
			yield return new CodeInstruction(OpCodes.Ldc_I4_1);
			yield return new CodeInstruction(OpCodes.Ret);
		}
	}

	//public class TestClass
	//{
	//	public bool IsBandit { get; private set; }
	//	private void Test_mod(PartyTemplateObject pt)
	//	{
	//		if (IsBandit)
	//		{
	//			float playerProgress = Campaign.Current.PlayerProgress;
	//			float num = 0.4f + 0.8f * playerProgress;
	//			int num2 = MBRandom.RandomInt(2);
	//			float num3 = ((num2 == 0) ? MBRandom.RandomFloat : (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * 4f));
	//			float num4 = ((num2 == 0) ? (num3 * 0.8f + 0.2f) : (1f + num3));
	//			float randomFloat = MBRandom.RandomFloat;
	//			float randomFloat2 = MBRandom.RandomFloat;
	//			float randomFloat3 = MBRandom.RandomFloat;
	//			float f = ((pt.Stacks.Count > 0) ? ((float)pt.Stacks[0].MinValue + num * num4 * randomFloat * (float)(pt.Stacks[0].MaxValue - pt.Stacks[0].MinValue)) : 0f);
	//			float f2 = ((pt.Stacks.Count > 1) ? ((float)pt.Stacks[1].MinValue + num * num4 * randomFloat2 * (float)(pt.Stacks[1].MaxValue - pt.Stacks[1].MinValue)) : 0f);
	//			float f3 = ((pt.Stacks.Count > 2) ? ((float)pt.Stacks[2].MinValue + num * num4 * randomFloat3 * (float)(pt.Stacks[2].MaxValue - pt.Stacks[2].MinValue)) : 0f);
	//			Main.ModifyVariable(this, ref f, ref f2, ref f3);
	//			AddElementToMemberRoster(pt.Stacks[0].Character, MBRandom.RoundRandomized(f));
	//			if (pt.Stacks.Count > 1)
	//			{
	//				AddElementToMemberRoster(pt.Stacks[1].Character, MBRandom.RoundRandomized(f2));
	//			}
	//			if (pt.Stacks.Count > 2)
	//			{
	//				AddElementToMemberRoster(pt.Stacks[2].Character, MBRandom.RoundRandomized(f3));
	//			}
	//			return;
	//		}
	//	}

	//	private void Test_ori(PartyTemplateObject pt)
	//	{
	//		if (IsBandit)
	//		{
	//			float playerProgress = Campaign.Current.PlayerProgress;
	//			float num = 0.4f + 0.8f * playerProgress;
	//			int num2 = MBRandom.RandomInt(2);
	//			float num3 = ((num2 == 0) ? MBRandom.RandomFloat : (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * 4f));
	//			float num4 = ((num2 == 0) ? (num3 * 0.8f + 0.2f) : (1f + num3));
	//			float randomFloat = MBRandom.RandomFloat;
	//			float randomFloat2 = MBRandom.RandomFloat;
	//			float randomFloat3 = MBRandom.RandomFloat;
	//			float f = ((pt.Stacks.Count > 0) ? ((float)pt.Stacks[0].MinValue + num * num4 * randomFloat * (float)(pt.Stacks[0].MaxValue - pt.Stacks[0].MinValue)) : 0f);
	//			float f2 = ((pt.Stacks.Count > 1) ? ((float)pt.Stacks[1].MinValue + num * num4 * randomFloat2 * (float)(pt.Stacks[1].MaxValue - pt.Stacks[1].MinValue)) : 0f);
	//			float f3 = ((pt.Stacks.Count > 2) ? ((float)pt.Stacks[2].MinValue + num * num4 * randomFloat3 * (float)(pt.Stacks[2].MaxValue - pt.Stacks[2].MinValue)) : 0f);
	//			AddElementToMemberRoster(pt.Stacks[0].Character, MBRandom.RoundRandomized(f));
	//			if (pt.Stacks.Count > 1)
	//			{
	//				AddElementToMemberRoster(pt.Stacks[1].Character, MBRandom.RoundRandomized(f2));
	//			}
	//			if (pt.Stacks.Count > 2)
	//			{
	//				AddElementToMemberRoster(pt.Stacks[2].Character, MBRandom.RoundRandomized(f3));
	//			}
	//			return;
	//		}
	//	}

	//	public int DoStuff = 0;
	//	public int AddElementToMemberRoster(CharacterObject element, int numberToAdd, bool insertAtFront = false)
	//	{
	//		return DoStuff = numberToAdd;
	//	}
	//}
}
