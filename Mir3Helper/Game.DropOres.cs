namespace Mir3Helper
{
	using System.Threading.Tasks;
	using static PInvoke.User32;

	partial class Game
	{
		public bool CanDropOres => Hp > 0 && !PickUpItem.IsValid && IsPickaxeEquipped;

		public async Task<int> DropOres()
		{
			if (!CanDropOres) return -1;
			if (!Bag.IsOpened)
			{
				Window.KeyDown(VirtualKey.VK_W);
				await Task.Delay(100);
				if (!Bag.IsOpened) return -1;
			}

			int count = 0;
			int scroll = Bag.Scroll;
			for (int i = 0; i < 48; i++)
			{
				if (scroll != Bag.Scroll) break;
				var item = Bag.Item(i, scroll);
				if (item.IsValid)
				{
					string name = item.Name;
					int durability = item.Durability;
					if (ShouldDrop(name, durability))
					{
						if (!CanDropOres) break;
						var pos = Bag.ItemPos(i);
						Window.Click(pos);
						await Task.Delay(Program.Random.Next(300, 500));
						if (PickUpItem.IsValid)
						{
							if (PickUpItem.Name == name && PickUpItem.Durability == durability)
							{
								Window.Click(YesButton);
								++count;
								await Task.Delay(Program.Random.Next(1200, 2000));
							}
							else // wrong item picked up
							{
								Window.Click(pos);
								await Task.Delay(500);
								break;
							}
						}
					}
				}
			}

			return count;
		}

		bool IsPickaxeEquipped
		{
			get
			{
				var weapon = WeaponItem;
				return weapon.IsValid && weapon.Name.EndsWith("鹤嘴锄");
			}
		}

		bool ShouldDrop(string name, int durability)
		{
			switch (name)
			{
				case "铜矿": return true;
				case "铁矿": return durability < 15000;
//				case "银矿": return false;
//				case "金矿": return false;
//				case "黑铁": return false;
				case "紫水晶":
				case "石榴石": return true;
				case "金刚石": return durability < 10000;
//				case "钢玉矿石": return false;
				default: return false;
			}
		}
	}
}