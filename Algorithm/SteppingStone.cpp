#include <string>
#include <vector>
#include <algorithm>
using namespace std;

int solution(vector<int> stones, int k)
{
	int left = 1, right = *max_element(stones.begin(), stones.end());

	while (left <= right)
	{
		int mid = (left + right) / 2;
		int cnt = 0;
		bool flag = false;
		for (int i = 0; i < stones.size();i++)
		{
			if (stones[i] - mid <= 0) cnt++;
			else cnt = 0;

			if (cnt >= k)
			{
				flag = true;
				break;
			}
		}

		if (flag)
		{
			right = mid - 1;
		}
		else left = mid + 1;
	}


	return left;
}