#ifndef SERVER_H
#define SERVER_H
#define _CRT_SECURE_NO_WARNINGS
#pragma comment (lib, "ws2_32.lib")
#include <iostream>
#include <winsock.h>
#include <vector>
#include "Utility.h"
#include "Client.h"
using namespace std;

static class Server {
private:
	static SOCKET ServerSocket; //for activate server
	static WSAData WsaData; //winsock API data
	static SOCKADDR_IN ServerAddress; //server address value
	static int NextID; // next client's ID value +1
	static vector<Client*> Connections; // Currently connected client's list
	static Util Util; // using Utill class
public:
	static void Start();
	static int ClientCountInRoom(int RoomID);
	static void PlayClient(int RoomID);
	static void ExitClient(int RoomID);
	static void PutClient(int RoomID, int x, int y);
	static void FullClient(Client* client);
	static void EnterClient(Client* client);
	static void ServerThread(Client* client);
};
#endif // ! SERVER_H
