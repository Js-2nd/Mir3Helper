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
			Window.KeyDown(VirtualKey.VK_W);
			await Task.Delay(100);
			if (!Bag.IsOpened)
			{
				Window.KeyDown(VirtualKey.VK_W);
				await Task.Delay(100);
				if (!Bag.IsOpened) return -1;
			}

			int count = 0;
			for (int i = 0; i < 600; i++)
			{
				var item = Bag.Item(i);
				if (!item.IsValid) continue;
				string name = item.Name;
				int durability = item.Durability;
				if (ShouldDrop(name, durability))
				{
					if (!CanDropOres) break;
					await Bag.EnsureItemVisible(i);
					var pos = Bag.ItemPosByIndex(i);
					if (pos == null) continue;
					Window.Click(pos.Value);
					await Task.Delay(500);
					if (!PickUpItem.IsValid) continue;
					if (PickUpItem.Name != name || PickUpItem.Durability != durability) break;
					Window.Click(YesButton);
					await Task.Delay(Program.Random.Next(1500, 2500));
					if (PickUpItem.IsValid) break;
					++count;
				}
				else if (name.EndsWith("鹤嘴锄") && durability >= 1000 && CanDropOres && WeaponItem.Durability < 1000)
				{
					await Bag.EnsureItemVisible(i);
					var pos = Bag.ItemPosByIndex(i);
					if (pos == null) continue;
					Window.Click(pos.Value);
					await Task.Delay(500);
					if (!PickUpItem.IsValid) continue;
					bool opened = StatusOpened;
					if (opened)
					{
						Window.KeyDown(VirtualKey.VK_Q);
						await Task.Delay(100);
					}

					Window.KeyDown(VirtualKey.VK_Q);
					await Task.Delay(100);
					Window.Click(StatusWeaponPos);
					await Task.Delay(1500);
					if (!opened) Window.KeyDown(VirtualKey.VK_Q);
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
				case "铁矿": return durability < 16000;
				case "银矿": return durability < 4000;
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