#include "AlgorithmContainer.h"

#define MAXSIZE 8
int temp[MAXSIZE];

void MergeSort(int list[], int start, int mid, int end)
{
	int part1 = start;	//첫번째 배열의 첫번째
	int part2 = mid + 1;	// 두번째 배열의 첫번재
	int index = start;		//결과 배열의 인덱스

	while (part1 <= mid && part2 <= end)
	{
		// 배열 1			 배열 2			temp배열
		// part1			part2		  index
		//  ↓	              ↓				↓
		//[21][10]          [12][20]		[][][][]
		// 
		// 배열1의값과 배열2의 값을 비교해서 더 작을걸 temp 배열에 넣고 인덱스 증가

		if (list[part1] <= list[part2]) temp[index++] = list[part1++];
		else temp[index++] = list[part2++];
	}
	while (part1 <= mid) temp[index++] = list[part1++];	//만약 part1의 배열이 끝까지 돌지않았을때 끝까지 넣음
	while (part2 <= end) temp[index++] = list[part2++];	//만약 part2의 배열이 끝까지 돌지않았을때 끝까지 넣음

	for (int a = 0;a <= end;a++)
		list[a] = temp[a];	//정렬된 배열을 삽입

}

void Divide(int list[], int start, int end)
{
	int mid;
	if (start < end)
	{
		mid = (start + end) / 2;
		Divide(list, start, mid);		//왼쪽 배열
		Divide(list, mid + 1, end);		//오른쪽 배열
		MergeSort(list, start, mid, end);	//나눠진 배열들을 병합
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
