#include "AlgorithmContainer.h"
//�� ���ڿ� ��� ���ؼ� �� ���ڰ� �� ������ �ٲ۴�.
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