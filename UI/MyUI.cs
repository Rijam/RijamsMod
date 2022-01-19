using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;

namespace RijamsMod.UI
{
    class TheUI : UIState
    {
        public DragableUIPanel TheUIPanel;
        public UIHoverImageButton TheUIButton;
        public static bool Visible;
        public int ItemType = ModContent.ItemType<Items.Tools.TestMM>();
        private bool pick;
        private bool axe;
        private bool hammer;
        public Vector2 panelOffeset;
        static public bool[] buttonsActive = new bool[3];

        // In OnInitialize, we place various UIElements onto our UIState (this class).
        // UIState classes have width and height equal to the full screen, because of this, usually we first define a UIElement that will act as the container for our UI.
        // We then place various other UIElement onto that container UIElement positioned relative to the container UIElement.
        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            TheUIPanel = new DragableUIPanel();
            TheUIPanel.SetPadding(0);

            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(coinCounterPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            TheUIPanel.Left.Set(Main.MouseScreen.X + 10, 0f);
            TheUIPanel.Top.Set(Main.MouseScreen.Y + 10, 0f);
            TheUIPanel.Width.Set(140f, 0f);
            TheUIPanel.Height.Set(50f, 0f);
            TheUIPanel.BackgroundColor = new Color(73, 94, 171) * 0.5f;

            Texture2D buttonPickaxeTextureOff = ModContent.GetTexture("RijamsMod/UI/UIPickaxeOff");
            UIHoverImageButton pickaxe = new UIHoverImageButton(buttonPickaxeTextureOff, "Pickaxe Mode");
            pickaxe.Left.Set(5, 0f);
            pickaxe.Top.Set(5, 0f);
            pickaxe.Width.Set(40, 0f);
            pickaxe.Height.Set(40, 0f);
            pickaxe.OnClick += new MouseEvent(PickButtonClicked);
            TheUIPanel.Append(pickaxe);

            Texture2D buttonAxeTextureOff = ModContent.GetTexture("RijamsMod/UI/UIAxeOff");
            UIHoverImageButton axe = new UIHoverImageButton(buttonAxeTextureOff, "Axe Mode");
            axe.Left.Set(50, 0f);
            axe.Top.Set(5, 0f);
            axe.Width.Set(40, 0f);
            axe.Height.Set(40, 0f);
            axe.OnClick += new MouseEvent(AxeButtonClicked);
            TheUIPanel.Append(axe);

            Texture2D buttonHammerTextureOff = ModContent.GetTexture("RijamsMod/UI/UIHammerOff");
            UIHoverImageButton hammer = new UIHoverImageButton(buttonHammerTextureOff, "Hammer Mode");
            hammer.Left.Set(95, 0f);
            hammer.Top.Set(5, 0f);
            hammer.Width.Set(40, 0f);
            hammer.Height.Set(40, 0f);
            hammer.OnClick += new MouseEvent(HammerButtonClicked);
            TheUIPanel.Append(hammer);

            Append(TheUIPanel);
        }
        private void PickButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            pick = !pick;
        }
        private void AxeButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            axe = !axe;
        }
        private void HammerButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            hammer = !hammer;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //if (!Visible) { return; }
            panelOffeset = TheUIPanel.GetOffset();
            Texture2D buttonPickaxeTextureOn = ModContent.GetTexture("RijamsMod/UI/UIPickaxeOn");
            Texture2D buttonAxeTextureOn = ModContent.GetTexture("RijamsMod/UI/UIAxeOn");
            Texture2D buttonHammerTextureOn = ModContent.GetTexture("RijamsMod/UI/UIHammerOn");

            if (pick)
            {
                spriteBatch.Draw(buttonPickaxeTextureOn, new Vector2(5f, 5f) + panelOffeset, Color.White);
            }
            if (axe)
            {
                spriteBatch.Draw(buttonAxeTextureOn, new Vector2(50f, 5f) + panelOffeset, Color.White);
            }
            if (hammer)
            {
                spriteBatch.Draw(buttonHammerTextureOn, new Vector2(95f, 5f) + panelOffeset, Color.White);
            }

            WhichButtons();
            //UILogic();
        }
        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            Visible = false;
        }

        /*private void UILogic()
        {
            Player player = Main.LocalPlayer;

            if (!ShouldStayOpen(player, true))
            {
                Visible = false;
                return;
            }
        }

        public bool ShouldStayOpen(Player player, bool checkForRightClick)
        {
            if ((player.mouseInterface || player.lastMouseInterface) ||
                (player.dead) ||
                (checkForRightClick && Main.mouseRight && Main.mouseRightRelease) ||
                (player.HeldItem.type != ItemType))
                return false;

            return true;
        }*/
        public void WhichButtons()
        {
            buttonsActive[0] = pick;
            buttonsActive[1] = axe;
            buttonsActive[2] = hammer;
        }
    }
}