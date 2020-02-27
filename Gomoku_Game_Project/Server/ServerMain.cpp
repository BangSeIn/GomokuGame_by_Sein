#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>

#include <Winsock.h>

#include <iostream>

#include <vector>

#include <sstream>

#pragma comment (lib, "ws2_32.lib")



using namespace std;



class Client {

private:

	int clientID;

	int roomID;

	SOCKET clientSocket;

public:

	Client(int clientID, SOCKET clientSocket) {

		this->clientID = clientID;

		this->roomID = -1;

		this->clientSocket = clientSocket;

	}

	int getClientID() {

		return clientID;

	}

	int getRoomID() {

		return roomID;

	}

	void setRoomID(int roomID) {

		this->roomID = roomID;

	}

	SOCKET getClientSocket() {

		return clientSocket;

	}

};



SOCKET serverSocket;

vector<Client> connections;

WSAData wsaData;

SOCKADDR_IN serverAddress;



int nextID;



vector<string> getTokens(string input, char delimiter) {

	vector<string> tokens;

	istringstream f(input);

	string s;

	while (getline(f, s, delimiter)) {

		tokens.push_back(s);

	}

	return tokens;

}



int clientCountInRoom(int roomID) {

	int count = 0;

	for (int i = 0; i < connections.size(); i++) {

		if (connections[i].getRoomID() == roomID) {

			count++;

		}

	}

	return count;

}



void playClient(int roomID) {

	char* sent = new char[256];

	bool black = true;

	for (int i = 0; i < connections.size(); i++) {

		if (connections[i].getRoomID() == roomID) {

			ZeroMemory(sent, 256);

			if (black) {

				sprintf(sent, "%s", "[Play] : Black");

				black = false;

			}

			else {

				sprintf(sent, "%s", "[Play] : White");

			}

			send(connections[i].getClientSocket(), sent, 256, 0);

		}

	}

}



void exitClient(int roomID) {

	char* sent = new char[256];

	for (int i = 0; i < connections.size(); i++) {

		if (connections[i].getRoomID() == roomID) {

			ZeroMemory(sent, 256);

			sprintf(sent, "%s", "[Exit]");

			send(connections[i].getClientSocket(), sent, 256, 0);

		}

	}

}



void putClient(int roomID, int x, int y) {

	char* sent = new char[256];

	for (int i = 0; i < connections.size(); i++) {

		if (connections[i].getRoomID() == roomID) {

			ZeroMemory(sent, 256);

			string data = "[Put]" + to_string(x) + "," + to_string(y);

			sprintf(sent, "%s", data.c_str());

			send(connections[i].getClientSocket(), sent, 256, 0);

		}

	}

}



//Control the one client's Connection

void ServerThread(Client* client) {

	char* sent = new char[256];

	char* received = new char[256];

	int size = 0;

	while (true) {

		ZeroMemory(received, 256);

		if ((size = recv(client->getClientSocket(), received, 256, NULL)) > 0) {

			string receivedString = string(received);

			vector<string> tokens = getTokens(receivedString, ']');

			if (receivedString.find("[Enter]") != -1) {

				/* Find the client sent message*/

				for (int i = 0; i < connections.size(); i++) {

					string roomID = tokens[1];

					int roomInt = atoi(roomID.c_str());

					if (connections[i].getClientSocket() == client->getClientSocket()) {

						int clientCount = clientCountInRoom(roomInt);

						/*If more then 2 players are in the same room, send full */

						if (clientCount >= 2) {

							ZeroMemory(sent, 256);

							sprintf(sent, "%s", "[Full]");

							send(connections[i].getClientSocket(), sent, 256, 0);

							break;

						}

						cout << "클라이언트 [" << client->getClientID() << "]: " << roomID << "번 방으로 접속" << endl;

						/* Renew the user's room connect information*/

						Client* newClient = new Client(*client);

						newClient->setRoomID(roomInt);

						connections[i] = *newClient;

						/*Send Message connected into the room*/
						ZeroMemory(sent, 256);

						sprintf(sent, "%s", "[Enter]");

						send(connections[i].getClientSocket(), sent, 256, 0);

						/* Start the game if the opponent is already in the room */

						if (clientCount == 1) {

							playClient(roomInt);

						}

					}

				}

			}

			else if (receivedString.find("[Put]") != -1) {

				/* Receieve the client's information who sent message */

				string data = tokens[1];

				vector<string> dataTokens = getTokens(data, ',');

				int roomID = atoi(dataTokens[0].c_str());

				int x = atoi(dataTokens[1].c_str());

				int y = atoi(dataTokens[2].c_str());

				/* Send the location of horse user put*/

				putClient(roomID, x, y);

			}

			else if (receivedString.find("[Play]") != -1) {

				/* Find the client that sent message */

				string roomID = tokens[1];

				int roomInt = atoi(roomID.c_str());

				/* Send the location of horse user put*/

				playClient(roomInt);

			}

		}

		else {

			ZeroMemory(sent, 256);

			sprintf(sent, "Client [%i] is Disconnected", client->getClientID());

			cout << sent << endl;

			/* Find the player that exit */

			for (int i = 0; i < connections.size(); i++) {

				if (connections[i].getClientID() == client->getClientID()) {

					/* If someone exit during the game */

					if (connections[i].getRoomID() != -1 &&

						clientCountInRoom(connections[i].getRoomID()) == 2) {

						/* Send mesage to other who's in the room */

						exitClient(connections[i].getRoomID());

					}

					connections.erase(connections.begin() + i);

					break;

				}

			}

			delete client;

			break;

		}

	}

}



int main() {

	/*cout << ("Set the Port -> ");

	u_short port;

	cin >> port;

	cout << endl;*/

	WSAStartup(MAKEWORD(2, 2), &wsaData);

	serverSocket = socket(AF_INET, SOCK_STREAM, NULL);



	serverAddress.sin_addr.s_addr = inet_addr("127.0.0.1");

	serverAddress.sin_port = htons(9704);

	serverAddress.sin_family = AF_INET;



	cout << "[ Gomoku Game Server Launched.. ]" << endl;

	bind(serverSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress));

	listen(serverSocket, 32);



	int addressLength = sizeof(serverAddress);

	while (true) {

		SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, NULL);

		if (clientSocket = accept(serverSocket, (SOCKADDR*)&serverAddress, &addressLength)) {

			Client* client = new Client(nextID, clientSocket);

			cout << "[ New User is connected ]" << endl;

			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ServerThread, (LPVOID)client, NULL, NULL);

			connections.push_back(*client);

			nextID++;

		}

		Sleep(100);

	}
}