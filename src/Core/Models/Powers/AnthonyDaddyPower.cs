using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Powers;

public sealed class AnthonyDaddyPower : PowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.Static(StaticHoverTip.Block)
	];

	public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
	{
		if (player != base.Owner.Player)
		{
			return;
		}

		decimal currentBlock = base.Owner.Block;    // 属性名已确认
		decimal targetBlock = base.Amount;           // 15

		if (currentBlock > targetBlock)
		{
			// 多了就削减差值，remover 传 null 表示无来源
			await CreatureCmd.LoseBlock(
				choiceContext,
				base.Owner,
				currentBlock - targetBlock,
				null
			);
		}
		else if (currentBlock < targetBlock)
		{
			// 少了就补差值，GainBlock 不接受 choiceContext
			Flash();
			await CreatureCmd.GainBlock(
				base.Owner,
				targetBlock - currentBlock,
				ValueProp.Unpowered,
				null
			);
		}
		// 相等就什么都不做
	}
}
