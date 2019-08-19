namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	partial class Program
	{
		DateTime m_WarpTime;
		DateTime m_HideTime;
		DateTime m_HealTime;

		async Task<double> Update()
		{
			if (m_Assist.Moving || m_Assist.Casting) return 0.1;
			m_Assist.Update();
			int distance = 0;
			if (m_HasUser)
			{
				m_User.Update();
				var delta = m_User.Pos - m_Assist.Pos;
				distance = Math.Max(Math.Abs(delta.X), Math.Abs(delta.Y));
			}

			var now = DateTime.UtcNow;
			if (m_HasUser && now >= m_WarpTime && distance > 2)
			{
				await m_Assist.CoupleWarp();
				m_WarpTime = now + TimeSpan.FromSeconds(3.5);
				return 2;
			}

			if (now >= m_HideTime && !m_Assist.Hiding)
			{
				m_Assist.TryCastSkill(2, m_Assist.Id);
				m_HideTime = now + TimeSpan.FromSeconds(m_HasUser ? 5 : 15);
				return 1;
			}


			var heal = m_Assist.GetSkill(Skill.治愈术);
			if (heal.IsValid)
			{
				m_Assist.TryCastSkill(heal, m_Assist.Id);
			}

			int deltaHp = m_Assist.MaxHp - m_Assist.Hp;
			if (deltaHp >= 50)
			{
			}
			else if (now >= m_HealTime && deltaHp >= 20)
			{
			}

			if (m_Assist.Hp < m_Assist.MaxHp - 20)
			{
				m_Assist.TryCastSkill(1, m_Assist.Id);
				return DefaultActionDelay;
			}

			if (m_HasUser && distance < 9 && m_User.Hp > 0)
			{
//				if (m_User.AllBuffAtkMagic().All(buff => buff < 3))
////				if (m_User.BuffAtkThunder < 3)
//				{
//					m_Assist.CastAssistMagic(9, m_User.Id);
//					return DefaultActionDelay;
//				}
			}

			if (Distance <= 9 && m_User.Hp > 0)
			{
//				if (m_User.BuffDef < 5)
//				{
//					m_Assist.CastAssistMagic(8, m_User.Id);
//					return DefaultActionDelay;
//				}

//				if (m_User.AllBuffDefMagic().All(t => t < 5))
//				{
//					m_Assist.CastAssistMagic(7, m_User.Id);
//					return DefaultActionDelay;
//				}

				if (!m_Same && m_User.Hp < m_User.MaxHp - 30)
				{
					m_Assist.TryCastSkill(1, m_User.Id);
					return DefaultActionDelay;
				}
			}

			return 0.5;
		}
	}
}