#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include <stdio.h>
#include <string>
using namespace std;

void StartSelectionSort();		//선택정렬
void StartBubbleSort();			//버블정렬
void StartInsertionSort();		//삽입정렬

void StartMergeSort();			//합병정렬
void StartHeapSort();			//힙정렬
void StartQuickSort();			//퀵정렬

void StartcQueue();
// 유틸
void InsertRandomNum(vector<int>* list);
void ShowList(vector<int>* list);