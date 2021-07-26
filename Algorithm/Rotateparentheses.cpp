#include <string>
#include <vector>
#include <stack>
using namespace std;

string RotateStr(string s)
{
	char temp = s[0];
	s = s.substr(1, s.size());
	return s += temp;
}

bool CheckCorrect(string s)
{
	stack<char> st;
	for (int i = 0;i < s.length();i++)
	{
		bool check = false;

		if (s[i] == '(' || s[i] == '{' || s[i] == '[')
		{
			check = true;
			st.push(s[i]);
		}
		else
		{
			if (st.empty()) return false;

			if (st.top() == '(' && s[i] == ')')st.pop();
			else if (st.top() == '{' && s[i] == '}')st.pop();
			else if (st.top() == '[' && s[i] == ']')st.pop();
		}
	}
	return st.empty();
}
int solution(string s)
{
	int answer = 0;
	if (CheckCorrect(s)) answer++;

	for (int i = 1;i < s.size();i++)
	{
		s = RotateStr(s);
		if (CheckCorrect(s)) answer++;
	}
	return answer;
}