#include <string>
#include <vector>
#include <map>
using namespace std;

string DivideString(string st, int targetBlank)
{
	string temp;
	int target = 0;

	for (auto s : st)
	{
		if (s == ' ') target++;
		else if (targetBlank == target)
		{
			if (s == ' ') break;
			temp += s;
		}
	}

	return temp;
}

vector<string> solution(vector<string> record)
{
	vector<string> answer;
	vector<string> userId;
	map<string, string> userData;	//key = id, value =  닉네임

	string state;
	string id;
	string nickName;

	for (int i = 0;i < record.size();i++)
	{
		state = DivideString(record[i], 0);
		id = DivideString(record[i], 1);
		nickName = DivideString(record[i], 2);

		if (state == "Enter")
		{
			answer.push_back("님이 들어왔습니다.");
			userId.push_back(id);
			userData[id] = nickName;
		}
		else if (state == "Leave")
		{
			answer.push_back("님이 나갔습니다.");
			userId.push_back(id);
		}
		else
		{
			userData[id] = nickName;
		}

	}
	for (int i = 0; i < answer.size(); i++)
	{
		answer[i] = userData[userId[i]] + answer[i];
	}
	return answer;
}