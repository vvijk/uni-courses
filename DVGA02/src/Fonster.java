import java.awt.Color;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Scanner;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class Fonster implements ActionListener{
	private JPanel highscorePanel;
	private JPanel highscorePanel2;
	private JPanel latestRunPanel;
	private JList<Player> list;
	private JList<Integer> latestList;
	private Points points;
	
	private Highscore players;
	private latestrunz<Integer> playerScore;

	private JLabel highscoreLabel;
	private JLabel playerName;
	private JLabel latestRunLabel;
	private JLabel playerPoints;
	private JButton saveButton;
	private JTextField input;
	
	private File f;
	private File fL;
	private final String nameFile = "namn.txt";
	private final String latestrunzFile = "latestrunz.txt";
	
	private boolean pressed;
	Font text = new Font ("Courier New", 1, 20);
	Font text2 = new Font ("Courier New", 1, 15);

	public Fonster(GameBoard board, Points points) {
		board.setSize(800, 600);
		int temporaryPoints = points.getPoints();
		String point = Integer.toString(temporaryPoints);
		
		this.points = points;
		
		players = new Highscore(10);
		playerScore = new latestrunz<Integer>(3);

	//LASER IN GAMMLA HIGHSCORES:
		for(int k = 0; k < 10; k++) {
			Player playerK = new Player("---", 0);
			players.add(playerK, 0);
		  }
		
		lasIn(nameFile);

		// LATEST RUN
		lasInScore(latestrunzFile);
		playerScore.enqueue(Integer.parseInt(point));
		skrivUtScore(latestrunzFile, points.toString());
		
		for(int i = 0; i < playerScore.getSize(); i++) {
			System.out.println(playerScore.getModel().getElementAt(i).toString());
		}
		
	// HIGHSCORE LIST
		list = new JList<Player>(players.getModel());
		list.setSize(100, 300);
		
	// LATESTRUN LIST
		latestList = new JList<Integer>(playerScore.getModel());
		latestList.setSize(100, 300);
		
	//HIGSCORE HEADER  
		highscoreLabel = new JLabel("YOUR SCORE:");		
		highscoreLabel.setSize(400, 30);
		highscoreLabel.setFont(text);
		highscoreLabel.setForeground(Color.green);
		highscoreLabel.setLocation(150, 0);
		
		playerPoints = new JLabel(Integer.toString(temporaryPoints));
		playerPoints.setFont(text);
		playerPoints.setForeground(Color.green);
		
		playerName = new JLabel("YOUR NAME");
		playerName.setSize(100, 30);
		playerName.setFont(text2);
		playerName.setForeground(Color.green);
		playerName.setLocation(150, 30);
		
		saveButton = new JButton("Save");
		saveButton.setSize(400, 50);
		saveButton.setLocation(0, 150);
		
		input = new JTextField();
		input.setSize(100, 100);
		input.setDocument(new LangdRestrict(3));

		latestRunLabel = new JLabel("LATEST RUN");
		latestRunLabel.setSize(200, 30);
		latestRunLabel.setFont(text);
		latestRunLabel.setForeground(Color.green);
		latestRunLabel.setLocation(550, 30);
		
// HIGHSCORE LISTZ
		highscorePanel = new JPanel();
		highscorePanel.setSize(400, 600);
		highscorePanel.setLocation(0, 200);
		highscorePanel.setVisible(true);
		highscorePanel.setBackground(Color.gray.brighter());
		highscorePanel.add(list);
//HEADER
		highscorePanel2 = new JPanel(new GridLayout(5, 1));
		highscorePanel2.setSize(400, 200);
		highscorePanel2.setLocation(0, 0);
		highscorePanel2.setVisible(true);
		highscorePanel2.setBackground(Color.gray);
		highscorePanel2.add(highscoreLabel);
		highscorePanel2.add(playerPoints);
//	IF ENOUGH SCORE SHOW THIS
		for(int i = 0; i < players.getModel().getSize(); i++) {
			if(temporaryPoints > players.getPosition(i).getScore()) {
				highscorePanel2.add(playerName);
				highscorePanel2.add(input);
				highscorePanel2.add(saveButton);
				break;
			}
		}
		
// LATEST RUN SHIT
		latestRunPanel = new JPanel();
		latestRunPanel.setSize(400, 600);
		latestRunPanel.setLocation(400, 0);
		latestRunPanel.setVisible(true);
		latestRunPanel.setBackground(Color.gray.darker());
		latestRunPanel.add(latestRunLabel);
		latestRunPanel.add(latestList);
		

		board.add(highscorePanel);
		board.add(highscorePanel2);
		board.add(latestRunPanel);
		
		saveButton.addActionListener(this);
		skrivUt(nameFile, players);
		
		//Lägg till möjlighet att skriva in Initialer endast om man fått tillräckligt mkt poäng
		
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		if(pressed == false && !input.getText().isEmpty()) {
			if(e.getSource()==saveButton) {
					for(int i = 0; i < players.getModel().getSize(); i++) {
						if(points.getPoints() > players.getPosition(i).getScore()) {
							if(players.isFull()) {
								players.removeLast();
							}
							players.add(new Player(input.getText(), points.getPoints()), i);
							break;
						}
					}
				}
			for(int i = 0; i < players.getModel().getSize(); i++) {
				System.out.println(players.getPosition(i));
			}
				skrivUt(nameFile, players);
				pressed = true;
			}
		}


	public void lasIn(String filnamn) {
	
		f = new File(filnamn);
		String tmp;
		String name;
		String score;
		try
		{
			Scanner scan = new Scanner(f);
			while(scan.hasNextLine())
			{
				tmp = scan.nextLine();
				System.out.println(tmp + "	Detta är tmp.");
				String [] parts = tmp.split("# ");
				name = parts[0];
				score = parts[1];
				int intScore = Integer.parseInt(score);
				
				for(int i = 0; i < 10; i++) {	
			
						if(intScore > players.getModel().get(i).getScore()){
							
							players.getModel().remove(i);
								players.add(new Player(name, intScore), i);
						break;
						}
					}
				}
			scan.close();

		} catch (FileNotFoundException e){
			System.out.println("Filen fanns inte");
		}
	}
public void lasInScore(String filnamn) {
		
		fL = new File(filnamn);
		String tmp;
		try
		{
			Scanner scan = new Scanner(fL);
			while(scan.hasNextLine())
			{
				tmp = scan.nextLine();
					playerScore.enqueue(Integer.parseInt(tmp));
				}
			scan.close();

		} catch (FileNotFoundException e){
			System.out.println("Filen fanns inte");
		}
}
public void skrivUtScore(String filnamn, String points) {
	Integer tmp;
	FileWriter fw = null; 
	try
	{
		if(f.exists()) {
			f.delete();
			f.createNewFile();
			fw = new FileWriter(filnamn);
			for(int i=0;i<playerScore.getSize();i++){
				tmp = playerScore.getModel().elementAt(i);
				fw.write(tmp + "\n");
			}
		}
		fw.close();
	} catch (IOException e)
	{
		System.out.println("Det gick inte att skriva till filen");
	}
}
	
	public String addChar(String str, char ch, int position) {
		StringBuffer sb = new StringBuffer(str);
		sb.insert(position, ch);
		return sb.toString();
	}
	
	public void skrivUt(String filnamn, Highscore aktuell) {
		String tmp;
		FileWriter fw = null; 
		try
		{
			if(f.exists()) {
				f.delete();
				f.createNewFile();
				fw = new FileWriter(filnamn);
				
				for(int i=0;i<aktuell.size();i++){
					tmp = aktuell.getPosition(i).toString();
					String tmp2 = addChar(tmp, '#', 3);
					System.out.println("tmp innehåller: " + tmp2);
					fw.write(tmp2 + "\n");
				}
			}
			fw.close();
		} catch (IOException e)
		{
			System.out.println("Det gick inte att skriva till filen");
		}
	}
}