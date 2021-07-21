#include "AlgorithmContainer.h"

#define SIZE 6
struct cQueue
{
	int arr[SIZE];
	int front, rear;

	bool isEmpty()
	{
		return rear == front;
	}
	bool isFull()
	{
		return ((rear + 1) % SIZE) == front;
	}
	void Enqueue(int data)
	{
		if (isFull())
		{
			cout << "ť�� ����á��" << endl;
			return;
		}
		rear = (rear + 1) % SIZE;
		arr[rear] = data;
	}
	int Dequeue()
	{
		if (isEmpty())
		{
			cout << "ť�� ����ִ�" << endl;
			return INFINITY;
		}
		return arr[front = (front + 1) % SIZE];
	}
};


void StartcQueue()
{
	cQueue q;

	q.Enqueue(100);
	q.Enqueue(10);
	q.Enqueue(200);
	q.Enqueue(200);
	q.Enqueue(200);

	while (!q.isEmpty())
	{
		cout << q.Dequeue() << endl;
	}
}
