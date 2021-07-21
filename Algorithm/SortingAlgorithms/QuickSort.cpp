#include "AlgorithmContainer.h"

int Divide(vector<int>* list, int start, int end)
{
	int low, high, pivot, temp;

	low = start + 1;
	high = end;
	pivot = (*list)[start];

	while (low <= high)
	{
		while ((*list)[low] < pivot && low <= end) low++;
		while ((*list)[high] > pivot && high >= start) high--;

		if (low < high)
		{
			temp = (*list)[low];
			(*list)[low] = (*list)[high];
			(*list)[high] = temp;
		}
	}

	temp = (*list)[start];
	(*list)[start] = (*list)[high];
	(*list)[high] = temp;

	return high;
}

void QuickSort(vector<int>* list, int start, int end)
{
	if (start < end)
	{
		int pivot = Divide(&(*list), start, end);
		QuickSort(&(*list), start, pivot - 1);
		QuickSort(&(*list), pivot + 1, end);
	}
}

void StartQuickSort()
{
	vector<int> list;
	InsertRandomNum(&list);

	QuickSort(&list, 0, list.size() - 1);

	for (int i = 0;i < list.size();i++)
	{
		cout << list[i] << endl;
	}

}