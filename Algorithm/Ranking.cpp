#include <string>
#include <vector>

int solution(int n, vector<vector<int>> results)
{
	int answer = 0;
	vector<vector<int>>player(n, vector<int>(n, 0)); //fightnum / checkWin

	for (auto p : results)
	{
		player[p[0] - 1][p[1] - 1] = 1;
	}

	for (int k = 0;k < n;k++)
	{
		for (int i = 0;i < n;i++)
		{
			for (int j = 0;j < n;j++)
			{
				if (player[i][k] == 1 && player[k][j] == 1)
				{
					player[i][j] = 1;	//
				}
			}
		}
	}

	for (int i = 0;i < n;i++)
	{
		int fightcount = 0;
		for (int j = 0;j < n;j++)
		{
			if (player[j][i] == 1 || player[i][j] == 1) fightcount++;
		}

		if (fightcount == n - 1) answer++;
	}

	return answer;
}