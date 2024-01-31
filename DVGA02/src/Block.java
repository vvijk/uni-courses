import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Rectangle;

public class Block extends Sprite{
	private Color colorx;
	private int HP;
	
	
	public Block(int x, int y, int width, int heigth, Color colorx) {
	super(x, y, width, heigth);
		this.colorx = colorx;
		setHP(3);
	}
	@Override
	public void update(Keyboard keyboard) {
		
	}
	@Override
	public void draw(Graphics2D graphics) {
		graphics.setColor(colorx);
		graphics.fillRect(getX(), getY(), getWidth(), getHeight());
	}
	public void setColor (Color newColor) {
		this.colorx = newColor;
	}
	public int getHP (int x) {return this.HP;}
	public int setHP (int x) {
		HP = x;
		return HP;
	}
	
	public boolean isColliding(Ball ball) {
		Rectangle ballRect = new Rectangle(ball.getX(), ball.getY(), ball.getWidth(), ball.getHeight());
		
		Rectangle blocksBottom = new Rectangle(getX(), getY() + Blocks.BLOCK_HEIGHT - 1, getWidth(), 1);
		Rectangle blocksTop = new Rectangle(getX(), getY(), getWidth(), 1);
		Rectangle blocksLeft = new Rectangle(getX(), getY(),1 , getHeight());
		Rectangle blocksRight = new Rectangle(getX() + Blocks.BLOCK_WIDTH + 1, getY(), 1, getHeight());
		
		if(ballRect.intersects(blocksBottom)) {
			ball.ballstateVerti = Ballstate.Down;
			return blocksBottom.intersects(ballRect);
			
		}else if (ballRect.intersects(blocksTop)) {
			ball.ballstateVerti = Ballstate.Up;
			return blocksTop.intersects(ballRect);
			
		}else if (ballRect.intersects(blocksRight)) {
			ball.hitbatstate = ball.hitbatstate.HitRight;
			ball.ballstateHori = ball.ballstateHori.Right;
			return blocksRight.intersects(ballRect);
			
		}else if (ballRect.intersects(blocksLeft)) {
			ball.hitbatstate = ball.hitbatstate.HitLeft;
			ball.ballstateHori = ball.ballstateHori.Left;
			return blocksLeft.intersects(ballRect);
			
		}else {
			return false;
		}
		
	}

}
