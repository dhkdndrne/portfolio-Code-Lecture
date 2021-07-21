#include "AlgorithmContainer.h"

void StartInsertionSort()
{
	vector<int> list = { 8,5,6,2,4 };
	int key;
	int temp;
	//InsertRandomNum(&list);

	for (int i = 1;i < list.size();i++)
	{
		key = list[i];
		for (int j = i - 1;j >= 0;j--)
		{
			if (list[j] > key)
			{
				temp = list[j + 1];
				list[j + 1] = list[j];
				list[j] = temp;
			}
		}
	}

	ShowList(&list);
}