import java.awt.Graphics2D;

public class Game {
	final static int GROUND_POS = 400;
	final static int WINDOW_HEIGHT = 600;
	final static int WINDOW_WIDTH = 800;
	
	
	private Ball ball = new Ball(GROUND_POS - 10, WINDOW_HEIGHT - 52, 20, 20);
	private Bat bat = new Bat(GROUND_POS - (125/2), WINDOW_HEIGHT - 30, 125, 10);
	private Blocks blocks = new Blocks ();
	private Points points = new Points();
	private Lives lives = new Lives();
	private GameBoard board;
	private Fonster f;
	private int tickcount = 0;
	private GameState gameState;
	public Game(GameBoard board) {
		this.board = board;
	}

	public void update(Keyboard keyboard) {
		ball.update(keyboard);
		bat.update(keyboard);
		blocks.update(keyboard, ball, points);
		ball.isColliding(bat); 
		
		if(ball.getY() > WINDOW_HEIGHT && !lives.isDead()) {
			points.decreasePoints();
			ball.respawnBall(bat, lives);
		}
		if(lives.isDead()) {
			gameState = gameState.LOSS;
		}
		if(blocks.finished()) {
			gameState = gameState.WIN;
		}
		
		if(gameState == gameState.WIN || gameState == gameState.LOSS) {
			tickcount++;
			if(tickcount == 1) {
				f = new Fonster(board, points);
			}
		}
	}

	public void draw(Graphics2D graphics) {
		lives.draw(graphics);
		ball.draw(graphics);
		bat.draw(graphics);
		blocks.draw(graphics);
		points.draw(graphics);

	}
}