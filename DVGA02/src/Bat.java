import java.awt.Color;
import java.awt.Graphics2D;

public class Bat extends Sprite{
	public Bat(int x, int y, int width, int heigth) {
		super(x, y, width, heigth);

	}

	@Override
	public void update(Keyboard keyboard) {
	
		if(keyboard.isKeyDown(Key.Right)){
			setX(getX() + 9);
		}if(keyboard.isKeyDown(Key.Left)){
			setX(getX() - 9);
		}
	}

	@Override
	public void draw(Graphics2D graphics) {
		graphics.setColor(Color.darkGray);
		graphics.fillRect(getX(), getY(), getWidth(), getHeight());

		graphics.setColor(Color.magenta);
		graphics.fillRect(getX() + 3, getY() + 3, getWidth() - 6, getHeight() - 6);

		graphics.setColor(Color.white);
		graphics.fillRect(getX() + 5, getY() + 5, getWidth() - 9, getHeight() - 9);
		
		
		graphics.setColor(Color.magenta);
		graphics.drawRect(getX(), getY(), getWidth(), getHeight());
	
	if(getY() >= Game.WINDOW_HEIGHT) {
		setY(9);
	}
}

	
}

