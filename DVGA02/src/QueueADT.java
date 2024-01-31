public interface QueueADT<E>{
public void enqueue(E element);
public E dequeue();
public boolean isEmpty();
public boolean isFull();
}