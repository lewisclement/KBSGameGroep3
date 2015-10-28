﻿using System;
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
		struct Button {
			public String text;

			public Button(String text)
			{
				this.text = text;
			}
		};

		public enum STATE : int {main=0, pause, editor, levelloader}

		private List<List<Button>> menus;
		private List<Button> buttonList;
        private String menu;
		private int width;
		private int buttonHeight;
		private STATE currentState;
		private World world;

		//For loading levels
		FileInfo[] files;

		EditorGui editorGui;

		int hoverIndex = -1, clickIndex = -1;
		Point hoverPos = new Point (0, 0), clickPos = new Point (0, 0);

		public Menu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World world) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
			this.world = world;
			menus = new List<List<Button>> ();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
			width = Math.Min (StaticVariables.dpi * 2, this.xRes / 2);
			buttonHeight = Math.Min (StaticVariables.dpi, yRes / 5);

			List<Button> buttonList = new List<Button> ();
			buttonList.Add(new Button("Start"));
			buttonList.Add(new Button("Editor"));
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Quit"));
			menus.Insert ((int)STATE.main, buttonList);

			buttonList = new List<Button> ();
			buttonList.Add(new Button("Resume"));
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Exit"));
			menus.Insert ((int)STATE.pause, buttonList);

			buttonList = new List<Button> ();
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Exit"));
			menus.Insert ((int)STATE.editor, buttonList);

			buttonList = new List<Button> ();
			buttonList.Add(new Button("Back"));
			menus.Insert ((int)STATE.levelloader, buttonList);

			editorGui = new EditorGui ((int)GUI.editor, xRes, yRes, drawRatio, world);

			changeState (STATE.main);
			this.setActive (true);
        }

		public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);

			if (mousePos.X > width)
				clickIndex = -1;
			else
				clickIndex = mousePos.Y / buttonHeight;

			if (currentState == STATE.main) {
				switch (clickIndex) {
				case 0:
					DirectoryInfo d = new DirectoryInfo (StaticVariables.levelFolder);
					files = d.GetFiles ("*.xml");

					changeState (STATE.levelloader);
					break;
				case 1:
					setActive (false);
					world.FillWorld (TERRAIN.grass, new Size (50, 50));
					editorGui.reset (xRes, yRes, drawRatio, world);
					editorGui.setActive (true);

					changeState (STATE.editor);
					break;
				case 4:
					Application.Exit ();
					break;
				default:
					break;
				}
			} else if (currentState == STATE.pause) {
				switch (clickIndex) {
				case 0:
					setActive (false);
					break;
				case 3:
					changeState (STATE.main);
					break;
				default:
					break;
				}
			} else if (currentState == STATE.editor) {
				switch (clickIndex) {
				case 1:
					break;
				case 2:
					editorGui.setActive (false);
					changeState (STATE.main);
					break;
				default:
					break;
				}
			} else if (currentState == STATE.levelloader) {
				switch (clickIndex) {
				case 0:
					changeState (STATE.main);
					break;
				default:
					if (currentState == STATE.levelloader) {
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

		public override void setInput (Keys key)
		{
			if (key == Keys.Escape && currentState != STATE.main && currentState != STATE.levelloader) {
				switchActive ();
			}
		}

		public void addMenuItem(String text)
        {
			this.buttonList.Add(new Button(text));
        }

		public String[] getButtonList()
        {
			String[] returnStrings = new String[buttonList.Count];
			for (int i = 0; i < buttonList.Count; i++) {
				returnStrings [i] = buttonList [i].text;
			}

			return returnStrings;
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            StringFormat style = new StringFormat();
            style.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, width, yRes);
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, StaticVariables.dpi / 4, style);
            g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), 0, hoverIndex * buttonHeight, width, buttonHeight);

            if (currentState == STATE.main)
            {
                Image gamename = Image.FromFile(StaticVariables.textFolder + "/gamename.png");
                g.DrawImage(gamename, xRes / 2 - width / 2 - 100, 0, width *3, yRes / 3);

                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = (buttonHeight * i + buttonHeight / 6);

                    if (i == 0)
                    {
                        Image start = Image.FromFile(StaticVariables.textFolder + "/menu_start.png");
                        g.DrawImage(start, x, y, 150, 70);
                    }
                    if (i == 1)
                    {
                        Image editor = Image.FromFile(StaticVariables.textFolder + "/menu_editor.png");
                        g.DrawImage(editor, x, y, 150, 70);
                    }
                    if (i == 2)
                    {
                        Image settings = Image.FromFile(StaticVariables.textFolder + "/menu_settings.png");
                        g.DrawImage(settings, x, y, 150, 70);
                    }
                    if (i == 3)
                    {
                        Image help = Image.FromFile(StaticVariables.textFolder + "/menu_help.png");
                        g.DrawImage(help, x, y, 150, 70);
                    }
                    if (i == 4)
                    {
                        Image quit = Image.FromFile(StaticVariables.textFolder + "/menu_quit.png");
                        g.DrawImage(quit, x, y, 150, 70);
                    }
                }
            }
            if (currentState == STATE.editor)
            {
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;

                    if (i == 0)
                    {
                        Image start = Image.FromFile(StaticVariables.textFolder + "/menu_settings.png");
                        g.DrawImage(start, x, y, 150, 70);
                    }
                    if (i == 1)
                    {
                        Image editor = Image.FromFile(StaticVariables.textFolder + "/menu_help.png");
                        g.DrawImage(editor, x, y, 150, 70);
                    }
                    if (i == 2)
                    {
                        Image editor = Image.FromFile(StaticVariables.textFolder + "/menu_exit.png");
                        g.DrawImage(editor, x, y, 150, 70);
                    }
                }
            }


            if (currentState == STATE.pause)
            {
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;

                    if (i == 0)
                    {
                        Image resume = Image.FromFile(StaticVariables.textFolder + "/menu_resume.png");
                        g.DrawImage(resume, x, y, 150, 70);
                    }
                    if (i == 1)
                    {
                        Image settings = Image.FromFile(StaticVariables.textFolder + "/menu_settings.png");
                        g.DrawImage(settings, x, y, 150, 70);
                    }
                    if (i == 2)
                    {
                        Image help = Image.FromFile(StaticVariables.textFolder + "/menu_help.png");
                        g.DrawImage(help, x, y, 150, 70);
                    }
                    if (i == 3)
                    {
                        Image exit = Image.FromFile(StaticVariables.textFolder + "/menu_exit.png");
                        g.DrawImage(exit, x, y, 150, 70);
                    }
                }
            }
            if (currentState == STATE.levelloader)
                {
                for (int i = 0; i < buttonList.Count; i++)
                {
                    float fontSize = StaticVariables.dpi / 3;
                    float x = StaticVariables.dpi / 4;
                    float y = buttonHeight * i + buttonHeight / 6;
                    Image back = Image.FromFile(StaticVariables.textFolder + "/menu_back.png");
                    g.DrawImage(back, x, y, 150, 70);
                }
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

		public override void resize (int ScreenresX, int ScreenresY, float drawRatio)
		{
			base.resize (ScreenresX, ScreenresY, drawRatio);

			buttonHeight = Math.Min (StaticVariables.dpi, yRes / 5);
		}

		public void changeState(STATE state)
		{
			if (state == STATE.main) {
				world.loadLevel ("mainmenu");
				world.setFocusEntity(world.getEntitiesByType (ENTITIES.plant)[40]); //Quick'n'Dirty
			}

			currentState = state;
			buttonList = menus [(int)state];
		}

		public EditorGui getEditorGui() {
			return editorGui;
		}
    }

}
