// using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
// using MegaCrit.Sts2.Core.Models.Powers;
using sts2mod.Powers;

namespace sts2mod.Cards;

// 【卡牌类名 - 你想要的名称】
public sealed class AnthonyDaddy : CardModel
{
	// 动态变量：格挡值 15（升级后可能改这里）
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new BlockVar(15m, ValueProp.Move)
	];

	// 悬浮提示：显示“格挡”说明
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.Static(StaticHoverTip.Block)
	];

	// 【构造参数 - 费用/类型/稀有度/目标，按需修改】
	public AnthonyDaddy()
		: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		// 施加能力，层数即 15（动态变量 Block 的值）
		await PowerCmd.Apply<AnthonyDaddyPower>(
			choiceContext,
			base.Owner.Creature,
			base.DynamicVars.Block.BaseValue,
			base.Owner.Creature,
			this
		);
	}

	protected override void OnUpgrade()
	{
		// 【升级效果 - 默认费用减1，你也可以改成别的】
		base.EnergyCost.UpgradeBy(-1);
	}
}
