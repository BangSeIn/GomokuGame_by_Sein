#ifndef CLIENT_H
#define CLIENT_H
#include <winsock.h>
class Client {
private:
	int ClientID;
	int RoomID;
	SOCKET ClientSocket; //for connection
public:
	Client(int ClientID, SOCKET ClientSocket);
	int GetClientID();
	int GetRoomID();
	void SetRoomID(int RoomID);
	SOCKET GetClientSocket();
};
#endif // !CLIENT_H
