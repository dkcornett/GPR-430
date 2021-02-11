/*
   Copyright 2021 Daniel S. Buckstein
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
	   http://www.apache.org/licenses/LICENSE-2.0
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

/*
	GPRO Net SDK: Networking framework.
	By Daniel S. Buckstein
	main-server.c/.cpp
	Main source for console server application.
*/
//
//File name: main-server.cpp
//Purpose: server side of chat
//Contributors: Nick Brennan-Martin and Dianna Cornett




#include "gpro-net/gpro-net.h"


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <iostream>
#include <fstream>

#include "RakNet/RakPeerInterface.h"


#include <stdio.h>
#include <string.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID

//#define MAX_CLIENTS 10
//#define SERVER_PORT 7777

enum GameMessages
{
	ID_GAME_MESSAGE_1 = ID_USER_PACKET_ENUM + 1
};

//structure to retail username and ip of users
#pragma pack(push, 1)
struct Lobby
{

	unsigned char ID_;

	std::string username[10];
	std::string ip[10];

};
#pragma pack(pop)

int main(void)
{
	//char str[512];

	//open text file to log messages
	std::ofstream log;
	log.open("test.txt");
	bool is = false;
	int count = 0;


	const unsigned short SERVER_PORT = 7777;
	const unsigned short MAX_CLIENTS = 10;

	RakNet::RakPeerInterface* peer = RakNet::RakPeerInterface::GetInstance();
	RakNet::Packet* packet;


	RakNet::SocketDescriptor sd(SERVER_PORT, 0);
	peer->Startup(MAX_CLIENTS, &sd, 1);


	printf("Starting the server.\n");
	// We need to let the server accept incoming connections from the clients
	peer->SetMaximumIncomingConnections(MAX_CLIENTS);

	Lobby room;

	while (1)
	{
		for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
		{
			switch (packet->data[0])
			{
			case ID_REMOTE_DISCONNECTION_NOTIFICATION:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("Another client has disconnected.\n");
				break;
			case ID_REMOTE_CONNECTION_LOST:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("Another client has lost the connection.\n");
				break;
			case ID_REMOTE_NEW_INCOMING_CONNECTION:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("Another client has connected.\n");
				break;
			case ID_CONNECTION_REQUEST_ACCEPTED:
			{
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("Our connection request has been accepted.\n");

				// Use a BitStream to write a custom user message
				// Bitstreams are easier to use than sending casted structures, and handle endian swapping automatically
				RakNet::BitStream bsOut;
				bsOut.Write((RakNet::MessageID)ID_GAME_MESSAGE_1);
				bsOut.Write("Hello world");
				peer->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
			}
			break;
			case ID_NEW_INCOMING_CONNECTION:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("A connection is incoming.\n");

				//when user comes in get ip and username
				room.ip[peer->NumberOfConnections() - 1] = peer->GetLocalIP(peer->NumberOfConnections() - 1);
				room.username[peer->NumberOfConnections() - 1] = peer->GetSystemAddressFromIndex(peer->NumberOfConnections() - 1).ToString();

				break;
			case ID_NO_FREE_INCOMING_CONNECTIONS:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("The server is full.\n");
				break;
			case ID_DISCONNECTION_NOTIFICATION:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("A client has disconnected.\n");

				break;
			case ID_CONNECTION_LOST:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("A client lost the connection.\n");

				break;

			case ID_GAME_MESSAGE_1:
			{
				RakNet::RakString rs;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(RakNet::MessageID));
				bsIn.Read(rs);
				printf("%s\n", rs.C_String());
			}
			break;

			default:
				log << count << " " << "Another client has disconnected.\n";
				count++;
				printf("Message with identifier %i has arrived.\n", packet->data[0]);
				break;
			}
		}
	}

	//close txt file
	log.close();

	RakNet::RakPeerInterface::DestroyInstance(peer);

	return 0;
}


//receive packet
unsigned char GetPacketIdentifier(RakNet::Packet* p)
{
	if ((unsigned char)p->data[0] == ID_TIMESTAMP)
		return (unsigned char)p->data[sizeof(unsigned char) + sizeof(unsigned long)];
	else
		return (unsigned char)p->data[0];
}

//receive structure
//void DoMyPacketHandler(RakNet::Packet* packet)
//{
//	// Cast the data to the appropriate type of struct
//	MyStruct* s = (MyStruct*)packet->data;
//	assert(p->length == sizeof(MyStruct)); // This is a good idea if you’re transmitting structs.
//	if (p->length != sizeof(MyStruct))
//		return;
//
//	// Perform the functionality for this type of packet, with your struct,  MyStruct *s
//}