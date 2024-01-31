import java.awt.Color;
import java.awt.Graphics2D;

public class Points extends Sprite{

	public Points() {
		super(50, 50, 100, 100);
	}
	private int points = 0;
	
	@Override
	public void update(Keyboard keyboard) {
		
	}

	@Override
	public void draw(Graphics2D graphics) {
		graphics.setColor(Color.green);
		graphics.drawString("Points: " + points, getX(), getY());
		
	}
	public int getPoints() {
		return points;
	}

	public void incresePoints(int x) {
		points += x;
	}
	public void decreasePoints() {
		points = points - 1;
	}
}
