import javax.swing.DefaultListModel;

public class Highscore {
	private DefaultListModel<Player> lista;
	private int max;
	public Highscore(int maxStorlek) {
		lista = new DefaultListModel<>();
		max = maxStorlek;
	}

	public void push(Player element){
		lista.add(0, element);
	}

	public Player pop(){
		Player temp = lista.get(0);
		lista.remove(0);
		return temp;
	}

	public boolean isEmpty(){
		return lista.isEmpty();
	}

	public boolean isFull(){
		return lista.size()==max;
	}
	
	public DefaultListModel<Player> getModel(){ 
		return lista;
	}
	public Player getPosition(int i) {
		return lista.get(i);
	}

	public int size() {
		return lista.size();
	}

	public String get(int i) {
		String tmp = lista.get(i).toString();
		return tmp;
	}

	public void add(Player element, int index) {
		lista.add(index, element);
	}
	public void removeLast() {
		lista.remove(lista.getSize()-1);
	}
	
}