#include "Utility.h"

vector<string> Util::GetTokens(string input, char delimiter) {
	vector<string> Tokens;
	istringstream f(input);
	string s;
	while (getline(f, s, delimiter)) {
		Tokens.push_back(s);
	}
	return Tokens;
}