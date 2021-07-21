#include "AlgorithmContainer.h"
void InsertRandomNum(vector<int>* list)
{
	int num = 1;
	list->push_back((rand() % 9) + 1);

	while (num < 9)
	{
		bool check = true;
		int random = (rand() % 9) + 1;

		for (auto listNum : (*list))
		{
			if (listNum == random)
			{
				check = false;
				break;
			}
		}

		if (check)
		{
			list->push_back(random);
			num++;
		}
	}
}
void ShowList(vector<int>* list)
{
	for (auto num : (*list))
	{
		cout << num << endl;
	}
	cout << endl;
}


int main()
{
	//StartSelectionSort();
	//StartBubbleSort();
	//StartInsertionSort();
	//StartMergeSort();
	//StartHeapSort();
	StartQuickSort();
	//StartcQueue();
}
