import java.awt.Color;
import java.awt.Graphics2D;
import java.util.ArrayList;

public class Blocks{
	private ArrayList <Block> oneHitBlocks;
	private ArrayList <Block> twoHitBlocks;
	private ArrayList <Block> threeHitBlocks;

	final static int BLOCK_HEIGHT = 25;
	final static int BLOCK_WIDTH = 80;
	
	public Blocks() {
		oneHitBlocks = new ArrayList<Block>();
		for (int i = 1; i < 9; i++) {
			oneHitBlocks.add(new Block (84*i, 200, BLOCK_WIDTH, BLOCK_HEIGHT, Color.magenta));
		}	
		twoHitBlocks = new ArrayList<Block>();
		for (int i = 1; i < 9; i++) {
			twoHitBlocks.add(new Block (84*i, 170, BLOCK_WIDTH, BLOCK_HEIGHT, Color.darkGray));
			}
		threeHitBlocks = new ArrayList<Block>();
		for (int i = 1; i < 5; i++) {
			threeHitBlocks.add(new Block (84*i+168, 140, BLOCK_WIDTH, BLOCK_HEIGHT, Color.green.darker()));
			}
		}
	
	public void update(Keyboard keyboard, Ball ball, Points points) {

			//ONE HIT BLOCKS
		
		for(int i = 0; i < oneHitBlocks.size(); i++) {
			Block box = oneHitBlocks.get(i);
			box.update(keyboard);
			if(box.isColliding(ball)) {
				oneHitBlocks.remove(i);
				points.incresePoints(10);
			}
		}
			//TWO HIT BLOCKS
		
		for(int i = 0; i < twoHitBlocks.size(); i++) {
			Block box = twoHitBlocks.get(i);
			box.update(keyboard);
			if(box.isColliding(ball)) {
				box.setHP(box.getHP(i)-1);
				points.incresePoints(20);
			}
		}
			//THREE HIT BLOCKS
		
		for(int i = 0; i < threeHitBlocks.size(); i++) {
			Block box = threeHitBlocks.get(i);
			box.update(keyboard);
			if(box.isColliding(ball)) {
				box.setHP(box.getHP(i)-1);
				points.incresePoints(30);
			}
		}
	}
	public void draw(Graphics2D graphics) {	
		
				// ONE HIT BLOCKS
		
		for(int i = 0; i < oneHitBlocks.size(); i++) {
			Block box = oneHitBlocks.get(i);
			box.draw(graphics);
		}
				// TWO HIT BLOCKS________CHANGEING COLOUR ON HP
		
		for(int i = 0; i < twoHitBlocks.size(); i++) {
			Block box = twoHitBlocks.get(i);
			box.draw(graphics);
			if(box.getHP(i) == 2) {   
				box.setColor(Color.lightGray);
			}
			if(box.getHP(i) == 1) {
				twoHitBlocks.remove(i);
			}
		}
				// THREE HIT BLOCKS________CHANGEING COLOUR ON HP
		
		for(int i = 0; i < threeHitBlocks.size(); i++) {
			Block box = threeHitBlocks.get(i);
			box.draw(graphics);
			if(box.getHP(i) == 2) {   
				box.setColor(Color.green.brighter());
			}
			if(box.getHP(i) == 1) {
				box.setColor(Color.magenta);
			}
			if(box.getHP(i) == 0) {
				threeHitBlocks.remove(i);
			}
		}
	}
	public boolean finished() {
		if(oneHitBlocks.isEmpty() && twoHitBlocks.isEmpty() && threeHitBlocks.isEmpty()) {
			return true;
		}else 
			return false;
	}
}