import java.awt.Color;
import java.awt.Graphics2D;

public class Life extends Sprite{
	public Life(int x, int y) {
		super(x, y, 20, 20);
	}

	@Override
	public void update(Keyboard keyboard) {
	}

	@Override
	public void draw(Graphics2D graphics) {
			
		graphics.setColor(Color.white);
		graphics.fillOval(getX(), getY(), getWidth(), getHeight());

		graphics.setColor(Color.black);
		graphics.fillOval(getX() + 2, getY() + 2, getWidth() - 4, getHeight() - 4);

		graphics.setColor(Color.cyan);
		graphics.fillOval(getX() + 4, getY() + 4, getWidth() - 8, getHeight() - 8);
		
		graphics.setColor(Color.red);
		graphics.fillOval(getX() + 6, getY() + 6, getWidth() - 12, getHeight() - 12);
		
		graphics.setColor(Color.yellow);
		graphics.fillOval(getX() + 8, getY() + 8, getWidth() - 16, getHeight() - 16);
	}
	
}
