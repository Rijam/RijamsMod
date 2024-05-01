using Terraria;
using Terraria.GameContent.UI;

namespace RijamsMod.NPCs
{
	public class HellTraderMissingItemCurrency : CustomCurrencySingleCoin
	{
		public HellTraderMissingItemCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{

		}
		public override bool Accepts(Item item)
		{
			return false;
		}
		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			lines[currentLine++] = ""; // Remove the line that says the price.
		}
	}
}