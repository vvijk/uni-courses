import javax.swing.DefaultListModel;

public class latestrunz<E> implements QueueADT<Integer>{
	private DefaultListModel<Integer> runz;
	private int max;
	
	public latestrunz(int maxstorlek) {
		runz = new DefaultListModel();
		max = maxstorlek;
	}
	
	public int getSize() {
		return runz.size();
	}

	public DefaultListModel<Integer> getModel() {
		return runz;
	}

	@Override
	public boolean isEmpty() {
		return isEmpty();
	}

	@Override
	public boolean isFull() {
		return runz.size()==max;
	}

	@Override
	public void enqueue(Integer element) {
		if(isFull()) {
			dequeue();
		}
		runz.add(runz.getSize(), element);
	}

	@Override
	public Integer dequeue() {
		
		return runz.remove(0);
	}

}