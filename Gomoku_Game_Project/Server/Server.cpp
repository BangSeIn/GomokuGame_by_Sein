#include "Server.h"
#include <cstdlib>
#include <ctime>

SOCKET Server::ServerSocket;
WSAData Server::WsaData;
SOCKADDR_IN Server::ServerAddress;
int Server::NextID;
vector<Client*> Server::Connections;
Util Server::Util;

void Server::EnterClient(Client* client) {
	//send message to Client 'connection checked'
	char* sent = new char[256];
	ZeroMemory(sent, 256);
	sprintf(sent, "%s", "[Enter]");
	send(client->GetClientSocket(), sent, 256, 0);
}

void Server::FullClient(Client* client) {
	//send message to client 'Room Full'
	char* sent = new char[256];
	ZeroMemory(sent, 256);
	sprintf(sent, "%s", "[Full]");
	send(client->GetClientSocket(), sent, 256, 0);

}

void Server::PlayClient(int RoomID) {
	char* sent = new char[256];
	srand((unsigned int)time(NULL));
	int Setfirst = rand();
	bool black = true;

	//Set order randomly
	if (Setfirst % 2 == 0) black = false;

	for (int i = 0; i < Connections.size(); i++) {
		if (Connections[i]->GetRoomID() == RoomID) {
			ZeroMemory(sent, 256);
			
			if (black) { //set the Client as black who came first into the room 
				sprintf(sent, "%s", "[Play]Black");
				black = false;
			}
			else {
				sprintf(sent, "%s", "[Play]White");
				black = true;
			}
			send(Connections[i]->GetClientSocket(), sent, 256, 0);
		}
	}
}

void Server::ExitClient(int RoomID) {
	//send message to client if opponent quit the room
	char* sent = new char[256];
	for (int i = 0; i < Connections.size(); i++) {
		if (Connections[i]->GetRoomID() == RoomID) {
			ZeroMemory(sent, 256);
			sprintf(sent, "%s", "[Exit]");
			send(Connections[i]->GetClientSocket(), sent, 256, 0);
		}
	}
}

void Server::PutClient(int RoomID, int x, int y) {
	//When horse is put on board, inform clients about it
	char* sent = new char[256];
	for (int i = 0; i < Connections.size(); i++) {
		if (Connections[i]->GetRoomID() == RoomID) {
			ZeroMemory(sent, 256);
			string data = "[Put]" + to_string(x) + "," + to_string(y);
			sprintf(sent, "%s", data.c_str());
			send(Connections[i]->GetClientSocket(), sent, 256, 0);
		}
	}
}

int Server::ClientCountInRoom(int RoomID) {
	//Count number of clients currently connected in Room
	int count = 0;
	for (int i = 0; i < Connections.size(); i++) {
		if (Connections[i]->GetRoomID() == RoomID) {
			count++;
		}
	}
	return count;
}

void Server::Start() {
	WSAStartup(MAKEWORD(2, 2), &WsaData);
	ServerSocket = socket(AF_INET, SOCK_STREAM, NULL);

	ServerAddress.sin_addr.s_addr = inet_addr("127.0.0.1");
	ServerAddress.sin_port = htons(9704);
	ServerAddress.sin_family = AF_INET;

	cout << "[Launching Gomoku Game Server..]" << endl;
	bind(ServerSocket, (SOCKADDR*)&ServerAddress, sizeof(ServerAddress));
	listen(ServerSocket, 32);

	int AddressLength = sizeof(ServerAddress);
	while (true)
	{
		SOCKET ClientSocket = socket(AF_INET, SOCK_STREAM, NULL);
		if (ClientSocket = accept(ServerSocket, (SOCKADDR*)&ServerAddress, &AddressLength)) {
			Client* client = new Client(NextID, ClientSocket);
			cout << "[New User Connected]" << endl;
			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ServerThread, (LPVOID)client, NULL, NULL);
			Connections.push_back(client);
			NextID++;
		}
		Sleep(100);
	}
}

void Server::ServerThread(Client* client) {
	char* sent = new char[256];
	char* received = new char[256];
	int size = 0;
	while (true) {
		ZeroMemory(received, 256);
		if ((size = recv(client->GetClientSocket(), received, 256, NULL)) > 0) {
			string ReceivedString = string(received);
			vector<string> tokens = Util.GetTokens(ReceivedString, ']');
			if (ReceivedString.find("[Enter]") != -1) {
				string RoomID = tokens[1];
				int RoomInt = atoi(RoomID.c_str());
				int ClientCount = ClientCountInRoom(RoomInt);
				//if more than two players are in the room
				if (ClientCount >= 2) {
					FullClient(client);
				}
				//Connection Success
				client->SetRoomID(RoomInt);
				cout << "Client [" << client->GetClientID() << "] Entered in Room #" << client->GetRoomID() << endl;
				//Send message to client suceessfully entered into room
				EnterClient(client);
				//if opponent is already exist, start game
				if (ClientCount == 1) {
					PlayClient(RoomInt);
				}
			}
			else if (ReceivedString.find("[Put]") != -1) {
				//get client's information sent message
				string data = tokens[1];
				vector<string> DataTokens = Util.GetTokens(data, ',');
				int RoomID = atoi(DataTokens[0].c_str());
				int x = atoi(DataTokens[1].c_str());
				int y = atoi(DataTokens[2].c_str());
				//Send the location of horse user just put
				PutClient(client->GetRoomID(), x, y);
			}

			else if (ReceivedString.find("[Play]") != -1) {
				string RoomID = tokens[1];
				int RoomInt = atoi(RoomID.c_str());
				//Send Starting notice to room number that user entered
				PlayClient(client->GetRoomID());
			}
		}
		else {
			cout << "Client [" << client->GetClientID() << "] is disconnected" << endl;
			//find the player who left the game
			for (int i = 0; i < Connections.size(); i++) {
				if (Connections[i]->GetClientID() == client->GetClientID()) {
					//if someone who's playing game with other left
					if (Connections[i]->GetClientID() != -1 &&
						ClientCountInRoom(Connections[i]->GetRoomID() == 2)) {
						//send message to other still in the room
						ExitClient(Connections[i]->GetRoomID());
					}
					Connections.erase(Connections.begin() + i);
					break;
				}
			}
			delete client;
			break;
		}
	}
 }