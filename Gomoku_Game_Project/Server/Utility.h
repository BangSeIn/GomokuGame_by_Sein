#ifndef UTILITY_H
#define UTILITY_H
#include <vector>
#include <sstream>
using namespace std;

class Util {
public:
	//ex) input -> Hello Hi Sein / delimiter ->' '(space)
	//divide input as 3 elements of vector { Hello,Hi,Sein }
	vector<string> GetTokens(string input, char delimiter);
};

#endif // !UTILITY_H
