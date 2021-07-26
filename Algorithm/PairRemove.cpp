#include <iostream>
#include<string>
#include <stack>
using namespace std;

int solution(string s)
{
	stack<char> st;

	for (int i = 0;i < s.length();i++)
	{
		if (st.size() == 0 || st.top() != s[i])
		{
			st.push(s[i]);
		}
		else if (st.top() == s[i])
		{
			st.pop();
		}
	}
	return st.size() == 0;
}