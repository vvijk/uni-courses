import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Rectangle;

public class Ball extends Sprite{
	public Ballstate ballstateHori;
	public Ballstate ballstateVerti;
	public HitBatState hitbatstate;
	private int movY, movX;
	public Ball(int x, int y, int width, int heigth) {
		super(x, y, width, heigth);
		ballstateVerti = Ballstate.Idle;
		movY = 12;
	}
	@Override
	public void update(Keyboard keyboard) {	
		
						//START POSITION AND LAUNCH
		
		if(ballstateVerti == Ballstate.Idle) {
 			if(keyboard.isKeyDown(Key.Left)) {
 				setX(getX() - 9);
 			}
 			if(keyboard.isKeyDown(Key.Right)) {
 				setX(getX() + 9);
 			}
 		}
 		if(keyboard.isKeyDown(Key.Space) && (ballstateVerti == Ballstate.Idle)) {
 			ballstateVerti = Ballstate.Up;
 		}
 		if(ballstateVerti == Ballstate.Up) {
 			setY(getY() - movY);
 		}
 							//ROOF  
 		if(getY() <= 0) {
 			ballstateVerti = Ballstate.Down;
 		}
 		if(ballstateVerti == Ballstate.Down) {
 			setY(getY() + movY); 			
 		}
 							//LEFT WALL
 		if(getX() <= 0) {
 			ballstateHori = Ballstate.Right;
 			hitbatstate = HitBatState.Idle;
 		}
 		if(ballstateHori == Ballstate.Right) {
 			setX(getX() + movX);	
 		}
 							//RIGHT WALL
 		
 		if(getX() >= Game.WINDOW_WIDTH - 20) {
 			ballstateHori = Ballstate.Left;	
 			hitbatstate = HitBatState.Idle;
 		}
 		if(ballstateHori == Ballstate.Left) {
 			setX(getX() - movX);
 		}
 							//BATSTATE	
 		if(hitbatstate == HitBatState.HitRight) {
 			movY = 12;
 			movX = 1;
 			setX(getX() + movX);
 		}else if(hitbatstate == HitBatState.HitFarRight) {
 			movY = 12;
 			movX = 5;
 			setX(getX() + movX);
 		}else if(hitbatstate == HitBatState.HitLeft) {
 			movY = 12;
 			movX = 1;
 			setX(getX() - movX);
 		}else if(hitbatstate == HitBatState.HitFarLeft) {
 			movY = 12;
 			movX = 5;
 			setX(getX() - movX);
 		}else if(hitbatstate == HitBatState.HitMid) {
 			movY = 12;
 			setX(getX());	
 		}
 		
}

	@Override
	public void draw(Graphics2D graphics) {
		graphics.setColor(Color.magenta.darker());
		graphics.fillOval(getX(), getY(), getWidth(), getHeight());
		graphics.setColor(Color.magenta);
		graphics.fillOval(getX() + 2, getY() + 2, getWidth() - 4, getHeight() - 4);
	}
	
	public void respawnBall(Bat bat, Lives lives) {
			lives.lifeLost();
			ballstateVerti = Ballstate.Idle;
			ballstateHori = null;
			hitbatstate = null;
			setX(bat.getX() + 53);
			setY(bat.getY() - 21);
	}
	
	public boolean isColliding(Bat bat) {
		Rectangle ballRect = new Rectangle(getX(), getY(), getWidth(), getHeight());

		Rectangle batFarLeft = new Rectangle(bat.getX(), bat.getY(), 24, bat.getHeight());
		Rectangle batLeft = new Rectangle(bat.getX() + 25, bat.getY(), 24, bat.getHeight());	
		Rectangle batMid = new Rectangle(bat.getX() + 50, bat.getY(), 24, bat.getHeight());
		Rectangle batRight = new Rectangle(bat.getX() + 75, bat.getY(), 24, bat.getHeight());
		Rectangle batFarRight = new Rectangle(bat.getX() + 100, bat.getY(), 24, bat.getHeight());

		//			BALL n BAT INTERSECTS
		
		if(ballRect.intersects(batFarLeft)) {
		ballstateHori = Ballstate.Null;
		ballstateVerti = Ballstate.Up;
		hitbatstate = HitBatState.HitFarLeft;
		return ballRect.intersects(batFarLeft);
		
		}else if(ballRect.intersects(batLeft)) {
		ballstateHori = Ballstate.Null;
		ballstateVerti = Ballstate.Up;
		hitbatstate = HitBatState.HitLeft;
		return ballRect.intersects(batLeft);
		
		}else if(ballRect.intersects(batMid)) {
		ballstateHori = Ballstate.Null;
		ballstateVerti = Ballstate.Up;
		hitbatstate = HitBatState.HitMid;	
		return ballRect.intersects(batMid);	
		
		}else if(ballRect.intersects(batRight)) {
		ballstateHori = Ballstate.Null;
		ballstateVerti = Ballstate.Up;
		hitbatstate = HitBatState.HitRight;
		return ballRect.intersects(batRight);
		
		}else if(ballRect.intersects(batFarRight)) {
		ballstateHori = Ballstate.Null;
		ballstateVerti = Ballstate.Up;
		hitbatstate = HitBatState.HitFarRight;
		return ballRect.intersects(batFarRight);
		}else {
			return false;
		}
	}	
}