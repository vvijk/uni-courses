public interface StackADT<T> {
	public void push(T element);
	public T pop();
	public boolean isEmpty();
	public boolean isFull();
}