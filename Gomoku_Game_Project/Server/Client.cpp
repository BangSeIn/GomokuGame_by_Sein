#include "Client.h"

Client::Client(int ClientID, SOCKET ClientSocket) {
	this->ClientID = ClientID;
	this->RoomID = -1;
	this->ClientSocket = ClientSocket;
}
int Client::GetClientID() {
	return ClientID;
}
int Client::GetRoomID() {
	return RoomID;
}
void Client::SetRoomID(int RoomID) {
	this->RoomID = RoomID;
}
SOCKET Client::GetClientSocket() {
	return ClientSocket;
}