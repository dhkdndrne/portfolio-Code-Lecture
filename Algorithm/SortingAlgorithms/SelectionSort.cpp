#include "AlgorithmContainer.h"
//ù��° �ڷḦ ����Ʈ�� ���� ���� ���̶� �ٲ�ġ��

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