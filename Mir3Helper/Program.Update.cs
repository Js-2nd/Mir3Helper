namespace Mir3Helper
{
	using System;
	using System.Threading.Tasks;

	partial class Program
	{
		DateTime m_Now;
		DateTime m_WarpTime;
		DateTime m_HideTime;
		DateTime m_HealTime;
		DateTime m_HealUserTime;

		async Task<double> Update()
		{
			if (m_Assist.Moving || m_Assist.Casting) return 0.2;
			m_Now = DateTime.UtcNow;
			m_Assist.Update();
			int dist = 0;
			if (m_HasUser)
			{
				m_User.Update();
				dist = (m_User.Pos - m_Assist.Pos).Abs().MaxComponent;
				if (m_AutoWarp && dist > 3 && m_Now >= m_WarpTime && m_Assist.Hp > 0)
				{
					await m_Assist.CoupleWarp();
					m_WarpTime = m_Now + TimeSpan.FromSeconds(3.5);
					return 2;
				}
			}

			if (m_Now >= m_HideTime && !m_Assist.Hiding && m_Assist.TryCastSkill(Skill.隐身术))
			{
				m_HideTime = m_Now + TimeSpan.FromSeconds(m_HasUser ? 5 : 20);
				return 1;
			}

			if (TryHeal(m_Assist, m_Assist, 50) ||
			    m_Now >= m_HealTime && TryHeal(m_Assist, m_Assist, 10))
			{
				m_HealTime = m_Now + TimeSpan.FromSeconds(5);
				return 1;
			}

			if (m_HasUser && dist <= 7 && m_User.Hp > 0)
			{
				if (TryBuff(m_Assist, m_User, Skill.幽灵盾) ||
				    TryBuff(m_Assist, m_User, Skill.神圣战甲术) ||
				    TryBuff(m_Assist, m_User, Skill.强震魔法) ||
				    m_User.Class != PlayerClass.Mage && TryBuff(m_Assist, m_User, Skill.猛虎强势))
					return 1;
				if (TryHeal(m_Assist, m_User, 50) ||
				    m_Now >= m_HealUserTime && TryHeal(m_Assist, m_User, 10))
				{
					m_HealUserTime = m_Now + TimeSpan.FromSeconds(5);
					return 1;
				}
			}

			if (TryBuff(m_Assist, m_Assist, Skill.幽灵盾) ||
			    TryBuff(m_Assist, m_Assist, Skill.神圣战甲术)) return 1;
			if (m_HasUser && TryPoison(m_Assist, m_User)) return 1;
			return 0.5;
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

		bool TryPoison(Game self, Game unit)
		{
			var skill = self.GetSkill(Skill.施毒术);
			if (!skill.IsValid) return false;
			int targetId = 0;
			if (unit.Casting) targetId = unit.SkillTarget;
			if (targetId == 0 && unit.Attacking) targetId = unit.AttackTarget;
			if (targetId == 0) return false;
			if (m_PoisonTarget != targetId)
			{
				m_PoisonTarget = targetId;
				m_RedPoisonTime = default;
				m_GreenPoisonTime = default;
			}

			var target = self.GetUnit(targetId);
			if (!target.IsValid || target.Hp == 0) return false;
			var targetType = target.Type;
			if (targetType == UnitType.Player)
			{
				if (unit.AttackMode == AttackMode.Peace) return false;
				if (self.AttackMode == AttackMode.Peace) self.AttackMode.Set(AttackMode.Group);
			}
			else if (targetType == UnitType.Monster)
			{
				if (target.MaxHp <= 1200) return false;
			}
			else return false;

			if (m_Now >= m_RedPoisonTime && !target.RedPoison)
			{
				if (self.TryCastSkill(Skill.施毒术, target, SkillPoison.Red))
				{
					m_RedPoisonTime = m_Now + TimeSpan.FromSeconds(20);
					return true;
				}
			}
			else if (m_Now >= m_GreenPoisonTime && !target.GreenPoison)
			{
				if (self.TryCastSkill(Skill.施毒术, target, SkillPoison.Green))
				{
					m_GreenPoisonTime = m_Now + TimeSpan.FromSeconds(20);
					return true;
				}
			}

			return false;
		}
	}
}