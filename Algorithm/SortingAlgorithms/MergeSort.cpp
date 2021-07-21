#include "AlgorithmContainer.h"

#define MAXSIZE 8
int temp[MAXSIZE];

void MergeSort(int list[], int start, int mid, int end)
{
	int part1 = start;	//ù��° �迭�� ù��°
	int part2 = mid + 1;	// �ι�° �迭�� ù����
	int index = start;		//��� �迭�� �ε���

	while (part1 <= mid && part2 <= end)
	{
		// �迭 1			 �迭 2			temp�迭
		// part1			part2		  index
		//  ��	              ��				��
		//[21][10]          [12][20]		[][][][]
		// 
		// �迭1�ǰ��� �迭2�� ���� ���ؼ� �� ������ temp �迭�� �ְ� �ε��� ����

		if (list[part1] <= list[part2]) temp[index++] = list[part1++];
		else temp[index++] = list[part2++];
	}
	while (part1 <= mid) temp[index++] = list[part1++];	//���� part1�� �迭�� ������ �����ʾ����� ������ ����
	while (part2 <= end) temp[index++] = list[part2++];	//���� part2�� �迭�� ������ �����ʾ����� ������ ����

	for (int a = 0;a <= end;a++)
		list[a] = temp[a];	//���ĵ� �迭�� ����

}

void Divide(int list[], int start, int end)
{
	int mid;
	if (start < end)
	{
		mid = (start + end) / 2;
		Divide(list, start, mid);		//���� �迭
		Divide(list, mid + 1, end);		//������ �迭
		MergeSort(list, start, mid, end);	//������ �迭���� ����
	}
}

void StartMergeSort()
{
	int n = MAXSIZE;
	int list[] = { 21,10,12,20,25,13,15,22 };
	Divide(list, 0, n - 1);

	for (int i = 0;i < n;i++)
	{
		cout << list[i] << endl;
	}
}
