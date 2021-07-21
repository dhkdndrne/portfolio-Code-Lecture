#include "AlgorithmContainer.h"
//옆 숫자와 계속 비교해서 옆 숫자가 더 작으면 바꾼다.
void StartBubbleSort()
{
	vector<int> list;
	int temp;

	InsertRandomNum(&list);

	ShowList(&list);

	for (int i = 0;i < list.size();i++)
	{
		for (int j = i + 1;j < list.size();j++)
		{
			if (list[i] < list[j])
			{
				temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}
		}
	}

	ShowList(&list);
}