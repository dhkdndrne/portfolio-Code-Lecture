#include "AlgorithmContainer.h"
//첫번째 자료를 리스트중 가장 작은 값이랑 바꿔치기

void StartSelectionSort()
{
	srand((unsigned int)time(NULL));

	int i, j;
	int min, temp;
	vector<int> list;
	InsertRandomNum(&list);

	for (i = 0;i < list.size() - 1;i++)
	{
		min = i;
		for (j = i + 1;j < list.size();j++)
		{
			if (list[j] < list[min])
				min = j;
		}

		temp = list[i];
		list[i] = list[min];
		list[min] = temp;

	}
	ShowList(&list);
}