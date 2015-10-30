using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KBSGame
{
    public class Menu : Gui 
    {
        //Allows text to be added to buttons.
		struct Button {
			public String text;
			public Button(String text)
			{
				this.text = text;
			}
		};
        //Declarating attributes
		private enum IMAGES : int {start=0, resume, editor, help, back, exit, quit, title, count}
		private List<List<Button>> menus;
		private List<Button> buttonList;
        private String menu;
		private int width;
		private int buttonHeight;
		private World world;
        //For loading images
		Image[] images;
		//For loading levels
		FileInfo[] files;
		EditorGui editorGui;

		int hoverIndex = -1, clickIndex = -1;
		Point hoverPos = new Point (0, 0), clickPos = new Point (0, 0);

        //Basic constructor for the resolution.
		public Menu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World world) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
			this.world = world;
			menus = new List<List<Button>> ();
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
			width = Math.Min (StaticVariables.dpi * 2, this.xRes / 2);
			buttonHeight = Math.Min (StaticVariables.dpi, yRes / 5);

            //ButtonList for the main menu.
			List<Button> buttonList = new List<Button> ();
			buttonList.Add(new Button("Start"));
			buttonList.Add(new Button("Editor"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Quit"));
			menus.Insert ((int)STATE.main, buttonList);

            //ButtonList for the menu when you pause the game.
			buttonList = new List<Button> ();
			buttonList.Add(new Button("Resume"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Exit"));
			menus.Insert ((int)STATE.pause, buttonList);

            //ButtonList for the menu in the editor.
			buttonList = new List<Button> ();
			buttonList.Add(new Button("Resume"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Exit"));
			menus.Insert ((int)STATE.editor, buttonList);

            //ButtonList for the menu after pressing "Start" to select a level.
			buttonList = new List<Button> ();
			buttonList.Add(new Button("Back"));
			menus.Insert ((int)STATE.levelloader, buttonList);

			editorGui = new EditorGui ((int)GUI.editor, xRes, yRes, drawRatio, world);
			loadButtonImages ();
			changeState (STATE.main);
			this.setActive (true);
        }
        /// <summary>
        /// This method checks where the mouse has been clicked on the screen, based on that a certain menu gets opened.
        /// </summary>
        /// <param name="mousePos"></param>
		public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);
			if (mousePos.X > width)
				clickIndex = -1;
			else
				clickIndex = mousePos.Y / buttonHeight;

			if (StaticVariables.currentState == STATE.main) { //When you are in the main menu:
				switch (clickIndex) {
				case 0: //When you press on start
					DirectoryInfo d = new DirectoryInfo (StaticVariables.levelFolder);
					files = d.GetFiles ("*.xml");

					changeState (STATE.levelloader);
					break;
				case 1: //When you press on editor
					setActive (false);
					world.FillWorld (TERRAIN.grass_normal, new Size (50, 50));
					editorGui.reset (xRes, yRes, drawRatio, world);
					editorGui.setActive (true);

					changeState (STATE.editor);
					break;
                case 2: //When you press on help
                    
                    break;
				case 3: //When you press on quit
					Application.Exit ();
					break;
				default:
					break;
				}
			} else if (StaticVariables.currentState == STATE.pause) { //If you press on escape, based on what screen you are, shows different menu options.
				switch (clickIndex) {
				case 0:
					StaticVariables.controller.disableModalGui ();
					setActive (false);
					break;
				case 2:
					StaticVariables.controller.disableModalGui ();
					changeState (STATE.main);
					break;
				default:
					break;
				}
			} else if (StaticVariables.currentState == STATE.editor) {
				switch (clickIndex) {
				case 0:
					setActive (false);
					break;
				case 1:
					break;
				case 2:
					editorGui.setActive (false);
					changeState (STATE.main);
					break;
				default:
					break;
				}
			} else if (StaticVariables.currentState == STATE.levelloader) {
				switch (clickIndex) {
				case 0:
					changeState (STATE.main);
					break;
				default:
					if (StaticVariables.currentState == STATE.levelloader) {
						int offsetY = StaticVariables.dpi * buttonList.Count;
						int index = (hoverPos.Y - offsetY) / 10;
						if (hoverPos.X < width) {
							if (index < files.Length) {
								String name = files [index].Name.Substring (0, files [index].Name.Length - 4);
								world.loadLevel (name);
								changeState (STATE.pause);
								setActive (false);
							}
						}
					}
					break;
				}
			}
        }

		public override void setMouseHover(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);
			if (mousePos.X > width)
				hoverIndex = -1;
			else {
				hoverIndex = mousePos.Y / buttonHeight;
				if (hoverIndex >= buttonList.Count)
					hoverIndex = -1;
			}
			hoverPos = mousePos;
		}

        /// <summary>
        /// Method to open the menu by pressing escape. Does only work if you are not in the main menu.
        /// </summary>
        /// <param name="key"></param>
		public override void setInput (Keys key)
		{
			if (key == Keys.Escape && StaticVariables.currentState != STATE.main && StaticVariables.currentState != STATE.levelloader) {
				if (!isActive () && StaticVariables.controller.modalActive ())
					return;
				if (StaticVariables.currentState == STATE.pause && !isActive ())
					StaticVariables.controller.setModalGui (GUI.def);
				switchActive ();
			}
		}

        //Simple add method to add a new button to the buttonList.
		public void addMenuItem(String text)
        {
			this.buttonList.Add(new Button(text));
        }

        //Simple getter for a buttonList.
		public String[] getButtonList()
        {
			String[] returnStrings = new String[buttonList.Count];
			for (int i = 0; i < buttonList.Count; i++) {
				returnStrings [i] = buttonList [i].text;
			}
			return returnStrings;
        }

        /// <summary>
        /// Method which paints everything onto the screen.
        /// </summary>
        /// <returns>It returns a rectangle on the left, with a proper hover bar and text based on the state.</returns>
        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            //sets font style
            StringFormat style = new StringFormat();
            style.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

            //Paints the tab on the left
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, width, yRes);
            g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), 0, hoverIndex * buttonHeight, width, buttonHeight);
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, StaticVariables.dpi / 4, style);
            
            if (StaticVariables.currentState == STATE.main) //If you are on the main menu, draws this menu:
            {
				g.DrawImage(images[(int)IMAGES.title], xRes / 2 - width / 2 - 100, 0, width *3, yRes / 3);
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = (buttonHeight * i + buttonHeight / 6);
                    if (i == 0)
                    {
						g.DrawImage(images[(int)IMAGES.start], x, y, 150, 70);
                    }
                    if (i == 1)
                    {
						g.DrawImage(images[(int)IMAGES.editor], x, y, 150, 70);
                    }
                    if (i == 2)
                    {
						g.DrawImage(images[(int)IMAGES.help], x, y, 150, 70);
                    }
                    if (i == 3)
                    {
						g.DrawImage(images[(int)IMAGES.quit], x, y, 150, 70);
                    }
                }
            }
            if (StaticVariables.currentState == STATE.editor) //If you are on the editor, draws this menu:
            {
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;
                    if (i == 0)
                    {
						g.DrawImage(images[(int)IMAGES.resume], x, y, 150, 70);
                    }
                    if (i == 1)
                    {
						g.DrawImage(images[(int)IMAGES.help], x, y, 150, 70);
                    }
                    if (i == 2)
                    {
						g.DrawImage(images[(int)IMAGES.exit], x, y, 150, 70);
                    }
                }
            }

            if (StaticVariables.currentState == STATE.pause) //If you press pause, draws this menu:
            {
                Image gamename = Image.FromFile(StaticVariables.textFolder + "/pause.png");
                g.DrawImage(gamename, xRes / 2 - width / 2 - 100, 0, width * 3, yRes / 3);
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;
                    if (i == 0)
                    {
						g.DrawImage(images[(int)IMAGES.resume], x, y, 150, 70);
                    }
                    if (i == 1)
                    {
						g.DrawImage(images[(int)IMAGES.help], x, y, 150, 70);
                    }
                    if (i == 2)
                    {
						g.DrawImage(images[(int)IMAGES.exit], x, y, 150, 70);
                    }
                }
            }
            if (StaticVariables.currentState == STATE.levelloader) //If you press on start on the main menu, draws this menu:
                {
                Image gamename = Image.FromFile(StaticVariables.textFolder + "/loadlevel.png");
                g.DrawImage(gamename, xRes / 2 - width / 2 - 100, 0, width * 3, yRes / 3);
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;
					g.DrawImage(images[(int)IMAGES.back], x, y, 150, 70);
                }
                //Draws a list of all map names under the "Back" tab.
                    for (int i = 0; i < files.Length; i++)
                    {
                        float x = StaticVariables.dpi / 4;
                        int offsetY = StaticVariables.dpi * buttonList.Count;
                        float y = (offsetY + i * 10);
                        font = new Font("Tahoma", 9, FontStyle.Bold);

                    int index = (hoverPos.Y - offsetY) / 10;
                        if (index == i && hoverPos.X < width)
                        {
                            if (index < files.Length)
                                g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, y - 2, width, 10);
                        }
                        String name = files[i].Name.Substring(0, files[i].Name.Length - 4);
                        g.DrawString(name, font, new SolidBrush(Color.White), x, y - 5);
                    }
                }
                g.Dispose();
                return this.buffer;
        }

        //Resize method.
		public override void resize (int ScreenresX, int ScreenresY, float drawRatio)
		{
			base.resize (ScreenresX, ScreenresY, drawRatio);
			buttonHeight = Math.Min (StaticVariables.dpi, yRes / 5);
		}

        //Changes state based on the situation given to it.
		public void changeState(STATE state)
		{
            //If you are on the main menu, it draws a part of this map onto the background. (Our easter egg)
			if (state == STATE.main) {
				world.loadLevel ("main");
				world.setFocusEntity(world.getEntitiesByType (ENTITIES.plant)[35]); //Sets focus on a plant on the map.
			}
			StaticVariables.currentState = state;
			buttonList = menus [(int)state];
		}

        /// <summary>
        /// Simple Getter.
        /// </summary>
        /// <returns>Returns the EditorGui.</returns>
		public EditorGui getEditorGui() {
			return editorGui;
		}

        /// <summary>
        /// Method to load up the mainmenu.
        /// </summary>
		public void mainmenu() {
			StaticVariables.world.loadLevel("mainmenu");
			changeState (STATE.main);
			setActive (true);
		}

        /// <summary>
        /// Method which loads all the text pictures once to prevent having it to load multiple times.
        /// </summary>
		private void loadButtonImages()
		{
			images = new Image[(int)IMAGES.count];
			images[(int)IMAGES.start] = Image.FromFile (StaticVariables.textFolder + "/menu_start.png");
			images[(int)IMAGES.resume] = Image.FromFile (StaticVariables.textFolder + "/menu_resume.png");
			images[(int)IMAGES.back] = Image.FromFile (StaticVariables.textFolder + "/menu_back.png");
			images[(int)IMAGES.exit] = Image.FromFile (StaticVariables.textFolder + "/menu_exit.png");
			images[(int)IMAGES.quit] = Image.FromFile (StaticVariables.textFolder + "/menu_quit.png");
			images[(int)IMAGES.editor] = Image.FromFile (StaticVariables.textFolder + "/menu_editor.png");
			images[(int)IMAGES.help] = Image.FromFile (StaticVariables.textFolder + "/menu_help.png");
			images[(int)IMAGES.title] = Image.FromFile (StaticVariables.textFolder + "/gamename.png");
		}
	}
}
