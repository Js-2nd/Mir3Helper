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

			if (m_Now >= m_HideTime && !m_Assist.Hiding)
			{
				if (m_Assist.TryCastSkill(Skill.隐身术))
				{
					m_HideTime = m_Now + TimeSpan.FromSeconds(m_HasUser ? 5 : 20);
					return 1;
				}
			}

			if (TryHeal(m_Assist, m_Assist, ref m_HealTime)) return 1;

			if (m_HasUser && dist <= 7 && m_User.Hp > 0)
			{
				if (TryCastBuff(m_Assist, Skill.强震魔法, m_User) ||
				    TryCastBuff(m_Assist, Skill.神圣战甲术, m_User) ||
				    TryCastBuff(m_Assist, Skill.幽灵盾, m_User) ||
				    m_User.Class != PlayerClass.Mage && TryCastBuff(m_Assist, Skill.猛虎强势, m_User))
					return 1;

				var skill = m_Assist.GetSkill(Skill.施毒术);
				if (skill.IsValid)
				{
					int targetId = m_User.SkillTarget.Value;
					if (targetId != 0)
					{
						var target = m_Assist.GetUnit(targetId);
						if (target.IsValid && target.Hp > 0)
						{
							if (target.Type == UnitType.Monster && target.MaxHp >= 1500)
							{
								if (!target.RedPoison && m_Assist.TryCastSkill(Skill.施毒术, target, SkillPoison.Red) ||
								    !target.GreenPoison && m_Assist.TryCastSkill(Skill.施毒术, target, SkillPoison.Green))
									return 1;
							}
						}
					}
				}

				if (TryHeal(m_Assist, m_User, ref m_HealUserTime)) return 1;
			}

			if (TryCastBuff(m_Assist, Skill.神圣战甲术, m_Assist) ||
			    TryCastBuff(m_Assist, Skill.幽灵盾, m_Assist)) return 1;

			return 0.5;
		}

		bool TryHeal(Game self, Game target, ref DateTime healTime)
		{
			var skill = self.GetSkill(Skill.治愈术);
			if (skill.IsValid)
			{
				int delta = target.MaxHp - target.Hp;
				if (delta >= 50 || m_Now >= healTime && delta >= 20)
				{
					if (self.TryCastSkill(Skill.治愈术, target.Self))
					{
						healTime = m_Now + TimeSpan.FromSeconds(5);
						return true;
					}
				}
			}

			return false;
		}

		bool TryCastBuff(Game self, Skill skill, Game target)
		{
			var s = self.GetSkill(skill);
			return s.IsValid &&
			       target.Buff.CanReceive(skill, s.Amulet) &&
			       self.TryCastSkill(skill, target.Self);
		}
	}
}