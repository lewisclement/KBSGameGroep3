using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame
{
    public class Controller
    {
		private int modalGuiID = -1;
		private HashSet<Keys> keypresses;
		private HashSet<Keys> processedkeys;
		private long lastTick = 0;
		private float movement;
		private int iteration = 0;

		public Controller() {
			keypresses = new HashSet<Keys> ();
			processedkeys = new HashSet<Keys> ();
		}

		public void gameover() {
			StaticVariables.renderer.getGui ((int)GUI.gameover).setActive (true);
            StaticVariables.renderer.getGui((int)GUI.guiinventory).setActive(false);
        }

		public void finish() {
			StaticVariables.renderer.getGui ((int)GUI.finish).setActive (true);
            StaticVariables.renderer.getGui((int)GUI.guiinventory).setActive(false);
        }

		public void render() {
			StaticVariables.renderer.render ();
		}

		public void mouseClick(Point point, bool leftClick = true) {
			for (int i = 0; i < StaticVariables.renderer.getGuiCount (); i++) {
				if (modalGuiID >= 0 && modalGuiID != i)
					continue;
				if (StaticVariables.renderer.getGui (i).isActive ())
					StaticVariables.renderer.getGui (i).setMouseClick(point, leftClick);
			}
		}

		public void mouseHover(Point point, bool leftClick = true) {
			for (int i = 0; i < StaticVariables.renderer.getGuiCount (); i++) {
				if (modalGuiID >= 0 && modalGuiID != i)
					continue;
				if (StaticVariables.renderer.getGui (i).isActive ())
					StaticVariables.renderer.getGui (i).setMouseHover(point);
			}
		}

		public void setModalGui(GUI gui) {
			if (modalActive ())
				return;

			modalGuiID = (int)gui;
			StaticVariables.renderer.setModalGui (gui);
		}

		public void disableModalGui() {
			modalGuiID = -1;
			StaticVariables.renderer.disableModalGui ();
		}

		public bool modalActive()
		{
			bool returnBool = false;
			if (modalGuiID >= 0)
				returnBool = true;

			return returnBool;
		}

		public void setKeyPress(Keys key) {
			keypresses.Add (key);
		}

		public void setKeyRelease(Keys key) {
			keypresses.Remove (key);
			processedkeys.Remove (key);
		}

		public void cycle(object sender, EventArgs e) {
			if (modalGuiID < 0) {
				long currentTick = System.DateTime.UtcNow.Ticks / 10000;
				movement = currentTick - lastTick;

				if(movement > 1000) //If no cycles for a second or more
					movement = 100;

				movement /= 200.0f;

				processInput ();

				if (StaticVariables.world.getPlayer () != null) {
					Enemy[] enemies = StaticVariables.world.getEnemies ();
					foreach (Enemy enemy in enemies) {
						enemy.processTick ();
					}
				}

				lastTick = currentTick;
			}
			StaticVariables.controller.render();
			iteration++;
		}
			
		private void processInput()
		{
			foreach (Keys key in keypresses) {
				Player player = StaticVariables.world.getPlayer ();

				if (player != null)
					switch (key) {
					case Keys.Up:
					player.move (StaticVariables.world, new PointF (0.0f, -movement));
						player.CurrentDirection = (int)Player.Direction.Up;
						break;
					case Keys.Down:
					player.move (StaticVariables.world, new PointF (0.0f, movement));
						player.CurrentDirection = (int)Player.Direction.Down;
						break;
					case Keys.Left:
					player.move (StaticVariables.world, new PointF (-movement, 0.0f));
						player.CurrentDirection = (int)Player.Direction.Left;
						break;
					case Keys.Right:
					player.move (StaticVariables.world, new PointF (movement, 0.0f));
						player.CurrentDirection = (int)Player.Direction.Right;
						break;
					case Keys.Space:
						player.PickupItems (StaticVariables.world);
						break;
					case Keys.Z:
						if(processedkeys.Add(key))
							player.DropItem (StaticVariables.world);
						break;
					}
				else
					switch (key) { 
					case Keys.Up:
					StaticVariables.world.getFocusEntity ().move (StaticVariables.world, new PointF (0.0f, -movement));
						break;
					case Keys.Down:
					StaticVariables.world.getFocusEntity ().move (StaticVariables.world, new PointF (0.0f, movement));
						break;
					case Keys.Left:
					StaticVariables.world.getFocusEntity ().move (StaticVariables.world, new PointF (-movement, 0.0f));
						break;
					case Keys.Right:
					StaticVariables.world.getFocusEntity ().move (StaticVariables.world, new PointF (movement, 0.0f));
						break;
					}

				if(processedkeys.Add(key)) {
					switch (key) {
					case Keys.Escape:
						StaticVariables.renderer.getGui ((int)GUI.def).setInput (Keys.Escape);
						break;
					case Keys.E:
						StaticVariables.world.getPlayer ().DropItem (StaticVariables.world);
						break;
					case Keys.I:
                        if(StaticVariables.currentState == STATE.pause)
						    StaticVariables.renderer.getGui ((int)GUI.guiinventory).switchActive ();
						break;
					default:
						return;
					}
				}
			}
		}
    }
}
