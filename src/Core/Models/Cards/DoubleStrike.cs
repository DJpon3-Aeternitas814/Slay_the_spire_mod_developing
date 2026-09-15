using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
namespace sts2mod.Cards;

public sealed class DoubleStrike : CardModel
{
	// 攻击类卡牌，添加“打击”标签
	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

	// 动态变量：伤害6，力量1
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(6m, ValueProp.Move),
		new PowerVar<StrengthPower>(1m)
	];

	// 悬浮提示：显示力量说明
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<StrengthPower>()
	];

	// 构造函数：1费，攻击，普通，单体目标
	public DoubleStrike()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

		// 第一段攻击
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

		// 获得力量
		await PowerCmd.Apply<StrengthPower>(
			choiceContext,
			base.Owner.Creature,
			base.DynamicVars.Strength.BaseValue,
			base.Owner.Creature,
			this
		);

		// 第二段攻击（此时力量已生效，伤害会自动加上力量层数）
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		// 升级后：伤害6→7，力量1→2
		base.DynamicVars.Damage.UpgradeValueBy(1m);
		base.DynamicVars.Strength.UpgradeValueBy(1m);
	}
}
