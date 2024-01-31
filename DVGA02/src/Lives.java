import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics2D;
import java.util.ArrayList;

public class Lives {
	private ArrayList <Life> lives;
	Font text = new Font ("Courier New", 1, 20);
	
	public Lives() {
		lives = new ArrayList<Life>();
		for(int i = 0; i < 3; i++) {
			lives.add(new Life(650+(25*i), 35));
		}
	}

	public void draw(Graphics2D graphics) {
		graphics.setColor(Color.green);
		graphics.setFont(text);
		graphics.drawString("Lives:", 570, 50);
		

		for(int i = 0; i < lives.size(); i++) {
				Life life = lives.get(i);
				life.draw(graphics);
			}
	}
	
	public void lifeLost() {
		if(!lives.isEmpty())
		lives.remove(0);
	}
	public boolean isDead() {
		if(lives.isEmpty()) {
			return true;
		}else 
			return false; 
	}

	
}

