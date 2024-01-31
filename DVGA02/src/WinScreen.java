import java.awt.Color;
import java.awt.Graphics2D;

public class WinScreen extends Sprite{
	public WinScreen() {
		super(0, 0, 800, 600);
	}

	@Override
	public void update(Keyboard keyboard) {	
		
	}

	@Override
	public void draw(Graphics2D graphics) {
		graphics.setColor(Color.green);
		graphics.fillRect(0, 0, 900, 900);
		graphics.setColor(Color.white);
		graphics.drawString("YOU WON", 380, 250);
		}
}