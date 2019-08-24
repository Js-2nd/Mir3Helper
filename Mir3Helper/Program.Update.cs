namespace Mir3Helper
{
	using System;
	using System.Threading.Tasks;

	partial class Program
	{
		DateTime m_Now;
		DateTime m_HideTime;
		DateTime m_HealTime;
		DateTime m_HealUserTime;
		DateTime m_SummonBeastTime;
		DateTime m_SummonSkeletonTime;

		async Task<UpdateAction> Update()
		{
			if (m_Assist.Hp == 0 || m_Assist.Moving || m_Assist.Casting) return UpdateAction.Wait;
			m_Assist.Update();
			m_Now = DateTime.UtcNow;
			if (m_Now >= m_HideTime && !m_Assist.Hiding && m_Assist.TryCastSkill(Skill.隐身术))
			{
				m_HideTime = m_Now + TimeSpan.FromSeconds(m_HasUser ? 5 : 20);
				return UpdateAction.Skill;
			}

			int dist = 0;
			int target = 0;
			if (m_HasUser)
			{
				m_User.Update();
				dist = (m_User.Pos - m_Assist.Pos).Abs().MaxComponent;
				if (m_AutoWarp && dist > 3 && m_Now >= m_Assist.LastWarpTime + TimeSpan.FromSeconds(3.5))
				{
					await m_Assist.CoupleWarp();
					return UpdateAction.Warp;
				}

				target = GetAttackTarget(m_User, true);
				if (target != 0 && TryPoison(m_Assist, target)) return UpdateAction.Skill;
			}

			if (TryHeal(m_Assist, m_Assist, 50) ||
			    m_Now >= m_HealTime && TryHeal(m_Assist, m_Assist, 10))
			{
				m_HealTime = m_Now + TimeSpan.FromSeconds(5);
				return UpdateAction.Skill;
			}

			bool canCastOnUser = m_HasUser && dist <= 7 && m_User.Hp > 0;
			if (canCastOnUser)
			{
				if (TryBuff(m_Assist, m_User, Skill.幽灵盾) ||
				    TryBuff(m_Assist, m_User, Skill.神圣战甲术) ||
				    TryBuff(m_Assist, m_User, Skill.强震魔法) ||
				    m_User.Class != PlayerClass.Mage && TryBuff(m_Assist, m_User, Skill.猛虎强势))
					return UpdateAction.Skill;
			}

			if (TryBuff(m_Assist, m_Assist, Skill.幽灵盾) ||
			    TryBuff(m_Assist, m_Assist, Skill.神圣战甲术)) return UpdateAction.Skill;

			if (m_AssistAttack && m_Now >= m_Assist.LastWarpTime + TimeSpan.FromSeconds(3))
			{
				if (m_Now >= m_SummonBeastTime && TrySummon(m_Assist, Skill.召唤神兽))
				{
					m_SummonBeastTime = m_Now + TimeSpan.FromSeconds(10);
					return UpdateAction.LongSkill;
				}

				if (m_Now >= m_SummonSkeletonTime)
				{
					if (m_Assist.GetSkill(Skill.超强召唤骷髅).IsValid
						? TrySummon(m_Assist, Skill.超强召唤骷髅)
						: TrySummon(m_Assist, Skill.召唤骷髅))
					{
						m_SummonSkeletonTime = m_Now + TimeSpan.FromSeconds(10);
						return UpdateAction.LongSkill;
					}
				}
			}

			if (canCastOnUser)
			{
				if (TryHeal(m_Assist, m_User, 50) ||
				    m_Now >= m_HealUserTime && TryHeal(m_Assist, m_User, 10))
				{
					m_HealUserTime = m_Now + TimeSpan.FromSeconds(5);
					return UpdateAction.Skill;
				}
			}

			if (m_AssistAttack && target != 0)
			{
				var unit = m_Assist.GetUnit(target);
				if (unit.IsValid && !unit.IsDead)
				{
					if (unit.Type == UnitType.Player) m_Assist.AttackMode.Set(AttackMode.Group);
					if (m_Assist.TryCastSkill(Skill.月魂灵波, unit)) return UpdateAction.Skill;
				}
			}

			return UpdateAction.Wait;
		}

		int GetAttackTarget(Game unit, bool strict = false)
		{
			int id = 0;
			if (!strict || unit.Attacking) id = unit.AttackTarget;
			if (id == 0 && (!strict || unit.Casting)) id = unit.SkillTarget;
			if (id == 0) return 0;
			var target = unit.GetUnit(id);
			if (!target.IsValid || target.IsDead ||
			    target.Type == UnitType.Player && unit.AttackMode == AttackMode.Peace) return 0;
			return id;
		}

		bool TryHeal(Game self, Game target, int deltaHp)
		{
			var skill = self.GetSkill(Skill.治愈术);
			if (!skill.IsValid) return false;
			if (target.MaxHp - target.Hp < deltaHp) return false;
			return self.TryCastSkill(Skill.治愈术, target.Self);
		}

		bool TryBuff(Game self, Game target, Skill skill)
		{
			var data = self.GetSkill(skill);
			if (!data.IsValid) return false;
			if (!target.Buff.CanReceive(skill, data.Amulet)) return false;
			if (self.AttackMode != AttackMode.Peace) self.AttackMode.Set(AttackMode.Peace);
			return self.TryCastSkill(skill, target.Self);
		}

		int m_PoisonTarget;
		DateTime m_RedPoisonTime;
		DateTime m_GreenPoisonTime;

		bool TryPoison(Game self, int target)
		{
			var skill = self.GetSkill(Skill.施毒术);
			if (!skill.IsValid) return false;
			var unit = self.GetUnit(target);
			if (!unit.IsValid || unit.IsDead) return false;
			switch (unit.Type)
			{
				case UnitType.Monster:
					if (unit.MaxHp <= 1200) return false;
					break;
				case UnitType.Player:
					if (self.AttackMode == AttackMode.Peace) self.AttackMode.Set(AttackMode.Group);
					break;
				default: return false;
			}

			if (m_PoisonTarget != target)
			{
				m_PoisonTarget = target;
				m_RedPoisonTime = default;
				m_GreenPoisonTime = default;
			}

			if (m_Now >= m_RedPoisonTime && !unit.RedPoison)
			{
				if (self.TryCastSkill(Skill.施毒术, unit, SkillPoison.Red))
				{
					m_RedPoisonTime = m_Now + TimeSpan.FromSeconds(20);
					return true;
				}
			}

			if (m_Now >= m_GreenPoisonTime && !unit.GreenPoison)
			{
				if (self.TryCastSkill(Skill.施毒术, unit, SkillPoison.Green))
				{
					m_GreenPoisonTime = m_Now + TimeSpan.FromSeconds(20);
					return true;
				}
			}

			return false;
		}

		bool TrySummon(Game self, Skill skill)
		{
			var data = self.GetSkill(skill);
			if (!data.IsValid) return false;
			string name;
			if (skill == Skill.召唤骷髅) name = "变异骷髅";
			else if (skill == Skill.召唤神兽) name = "神兽";
			else if (skill == Skill.超强召唤骷髅) name = "超强骷髅";
			else return false;
			name = $"{name}({self.Name})";
			foreach (var unit in self.GetOtherUnits())
			{
				string unitName = unit.Name;
				if (string.IsNullOrWhiteSpace(unitName) || unitName == name) return false;
			}

			return self.TryCastSkill(skill);
		}
	}
}