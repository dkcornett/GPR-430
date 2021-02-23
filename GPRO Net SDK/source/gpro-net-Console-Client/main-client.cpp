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

	main-client.c/.cpp
	Main source for console client application.
*/

/*
* This project is Assignment 2 as solved by Dianna Cornett and Nicholas Brennan-Martin
* check our readme for more details
*/

//reminder: USING UDP, FAKING TCP

#include "gpro-net/gpro-net.h"
#include "gpro-net//gpro-net-common/gpro-net-console.h"
#include "gpro-net//gpro-net-common/gpro-net-gamestate.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <iostream>



#include "RakNet/RakPeerInterface.h"
#include "RakNet/RakNetTypes.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/GetTime.h"


//definitions for max clients and server port as shown in the tutorial
#define MAX_CLIENTS 10
#define SERVER_PORT 7777


//things that are common to all message tyoes
//	-> timestamp ID (constant)
//	-> timestamp
//	-> actual message identifier
//	-> ability to read and write
//	-> ability to determine packet settings
//things that are different
//	-> actual message data
//		-> any type of raw bytes
//		-> no raw or encapsulated pointers
// other unknowns
//	-> what if we have a pointer or pool???

//	common interface
//	-> want to be able to read/write from/to bitstream easily
//	-> way of mapping our data to the bitstream 

//our message data/identifiers
enum gproMessageID
{
	ID_CHATMESSAGE = ID_USER_PACKET_ENUM + 1,
};

class cMessage
{
//	const RakNet::Time time;
	const RakNet::MessageID id;

protected:
	cMessage(/*RakNet::Time time_new, */
		RakNet::MessageID id_new) :	/*time(time_new),*/ 
		id(id_new) {}
public: 
//	RakNet::Time GetTime() const { return time; };
	RakNet::MessageID GetID() const { return id; };

	//	decypher?


	virtual RakNet::BitStream& Read(RakNet::BitStream& bsp) 
	{
		//bsp->Read(time);
		return bsp;
	}
	virtual RakNet::BitStream& Write(RakNet::BitStream& bsp) const 
	{
		//bsp->Write(time);
		//return bsp;
	}


};

RakNet::BitStream& operator>>(RakNet::BitStream& bsp, cMessage& msg)
{
	return msg.Read(bsp);
}
RakNet::BitStream& operator<<(RakNet::BitStream& bsp, cMessage const& msg)
{
	return msg.Write(bsp);
}


//	message header
class cMessageHeader
{
	// no data: timestamp ID
	RakNet::Time time;

	//	sequence
	int count;
	//RakNet::MessageID* id_list;
	RakNet::MessageID id_list[16];
};

//time message
class cTimeMessage : public cMessage
{
	RakNet::Time time;
public:
	cTimeMessage() : cMessage(ID_TIMESTAMP), time(RakNet::GetTime()) {}
	RakNet::Time GetTime() const { return time; };
	bool Read(RakNet::BitStream* bsp)
	{
	//	RakNet::MessageID fakeID;
	//	bsp->Read(fakeID);
		bsp->Read(time);
		return true;
	}
	bool Write(RakNet::BitStream* bsp) const
	{
	//	bsp->Write(GetID());
		bsp->Write(time);
		return true;
	}
};

//class cChatMessage : public cMessage
//{
//	//	sender, receiver content
//	//RakNet::RakString rstr;
//	//std::string str;
//	char* cstr;
//	int len;
//	
//public:
//	//cChatMessage(std::string str_new) : cMessage(ID_CHATMESSAGE), str(str_new) {}
//	cChatMessage(char* cstr_new) : cMessage(ID_CHATMESSAGE), cstr(cstr_new), len(strlen(cstr_new)) {}
//	//
//	bool Read(RakNet::BitStream* bsp)
//	{
//		//bsp->Read(str);	//encapsulated pointer :(
//		bsp->Read(len);
//		//allocation?
//		bsp->Read(cstr, len);
//		return true;
//
//	}
//	bool Write(RakNet::BitStream* bsp) const
//	{
//		//	can write ID but can't read it back: need to fix
//		bsp->Write(len);
//		bsp->Write(cstr, len);	//bsp->Write((char const*)cstr);	//also works
//		return true;
//	}
//};


enum GameMessages
{
	ID_GAME_MESSAGE_1 = ID_USER_PACKET_ENUM + 1,
	ID_GAME_MESSAGE_2
};

#pragma pack (push)

struct GameMessage1
{/*
	//if timestamping then 1) time id, 2) you'll see :)
	//char timeID; //ID_TIMESTAMP
//	RakNet::Time time; // assigned using RakNet::GetTime();
//	RakNet::Time time = RakNet::GetTime(); //like this
	//id type: char
	char msgID;
	
	//the message
	char msg[512];
	//the time stamp

	char timeStamp = RakNet::GetTime();
	*/

	//let's try that again while looking at the documentation
	

	char timeID; //ID_TIMESTAMP gets attached to this
	//RakNet::Time timeStamp = RakNet::GetTime();
	RakNet::Time timeStamp;
	//id type:: char
	char msgID;
	char msg[512];

};

#pragma pack (pop)
struct ClientUser
{
//	char userName;
	char userName[512];
	bool isActive;
};


struct GameState
{
	RakNet::RakPeerInterface* peer;

};

void handleInputLocal(GameState* state)
{
	//RakNet::RakPeerInterface
	//local controllers, devices etc
}

void handleInputRemote(GameState* state)
{
	RakNet::RakPeerInterface* peer = state->peer;
	//RakNet::Packet* packet;
}

void handleUpdate(GameState* state)
{
	//game loop update
}

void handleOutputLocal(GameState* state)
{
	//output
}

void handleOutputRemote(GameState* state)
{
	//local output
}

void GetUserList()
{
	//this function being handled by Dianna
	//get struct username 
	struct Lobby;
	//for lobby list print usernames	//get the list

}

char EnterMessage(unsigned char* data)
{
	//this function being handled by Dianna
	char str[512];
	std::cin.getline(str, 512);

	if (str == "userlist" || "user list")
	{
		GetUserList();
		//str = to.String("you have requested a user list");
		return *str;
	}
	else
	{
		return *str;
	}

}

int main(void)
{ 
	//sample code from RakNet (http://www.jenkinssoftware.com/raknet/manual/tutorial.html) input and debugged/ fixed for C++11 by Dianna
	//char str[512];
	RakNet::RakPeerInterface *peer = RakNet::RakPeerInterface::GetInstance();
	RakNet::Packet* packet;
	RakNet::SocketDescriptor sd;
	const char SERVER_IP[] = "172.16.2.63";
	//GameState gs[1] = {0};
	//GameState gs;
	//RakNet::SocketDescriptor gs;
	//gs->peer->Startup(1, &sd, 1);
	//gs->peer = RakNet::RakPeerInterface::GetInstance();
	//gs->peer->SetMaximumIncomingConnections(0);
	//gs->peer->Connect(SERVER_IP, SERVER_PORT, 0, 0);

	//game loop
	//while (1)
	//{
	//	//input
	//}

	//	RakNet::SocketDescriptor sd; //moved to outside this loop
	peer->Startup(1, &sd, 1);

	peer->SetMaximumIncomingConnections(0);

	peer->Connect(SERVER_IP, SERVER_PORT, 0, 0);
	printf("Starting the client.\n");

	//while (1)
	//{ 
	//	//input
	//	handleInputLocal(gs);
	//	//recieve & merge
	//	handleInputRemote(gs);
	//	//update
	//	handleUpdate(gs);
	//	//package & send
	//	handleOutputRemote(gs);
	//	// output
	//	handleOutputLocal(gs);
	//}
	//the loop
	while (1)
	{
		for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
		{

			RakNet::MessageID msg = packet->data[0];
			//if (msg == ID_TIMESTAMP) 
			//{
			//	RakNet::RakString rs;
			//	RakNet::BitStream bsIn(packet->data, packet->length, false);
			//	unsigned char useTimeStamp;
			//	RakNet::Time timeStamp;
			//	bsIn.IgnoreBytes(sizeof(RakNet::MessageID));
			//	//bsIn.RakNet::GetTime();
			//	//hande time
			//	// 1) bistream
			//	// 2) skip msg byte
			//	// 3) read time
			//	// 4) read new msg byte: what is the acutal ID to handle
			//	
			//}

			switch (msg)
			{
			case ID_REMOTE_DISCONNECTION_NOTIFICATION:
			{
				printf("Another client has disconnected.\n");
				break;
			}
			case ID_REMOTE_CONNECTION_LOST:
			{
				printf("Another client has lost the connection.\n");
				break;
			}
			case ID_REMOTE_NEW_INCOMING_CONNECTION:
			{
				printf("Another client has connected.\n");
				break;
			}
			case ID_CONNECTION_REQUEST_ACCEPTED:
			{
				printf("Our connection request has been accepted.\n");

				//using BitStream to write a custom user message
				//from tutorial: "Bitstreams are easier to use than sending casted structures, and handle engian swapping automatically"
				RakNet::BitStream bsOut;
				bsOut.Write((RakNet::MessageID)ID_GAME_MESSAGE_1);
				//bsOut.Write((RakNet::Time)RakNet::GetTime());
				bsOut.Write("Hello world");
				peer->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
				

				//GameMessage1 msg =
				//{
				////	(char)ID_TIMESTAMP,
				//	(char)ID_GAME_MESSAGE_1,
				////	RakNet::GetTime(),
				//};

				//peer->Send((char*)&msg, sizeof(msg), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);


				break;
			}
			case ID_NEW_INCOMING_CONNECTION:
			{	printf("A connection is incoming.\n");
			break;
			}
			case ID_NO_FREE_INCOMING_CONNECTIONS:
			{
				printf("The server is full.\n");
				break;
			}
			case ID_DISCONNECTION_NOTIFICATION:
			{
				
					printf("We have been disconnected.\n");

				break;
			}
			case ID_CONNECTION_LOST:
			{
				if (SERVER_IP == "a") // this is here due to unknown bug 
				{
					printf("A client lost the connection.\n");
				}
				else 
				{
					printf("Connection lost.\n");
				}
				break;
			}
			case ID_GAME_MESSAGE_1:
			{
				RakNet::RakString rs;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(RakNet::MessageID));
				bsIn.Read(rs);
				printf("%s\n", rs.C_String());

				break;
			}

			case ID_GAME_MESSAGE_2: 
			{


			}

			default:
			{
				printf("Message with identifier %i has arrived.\n", packet->data[0]);
				EnterMessage(packet->data);
				break;

			}
			


			}
		}

	}

	RakNet::RakPeerInterface::DestroyInstance(peer);
	printf("\n\n");
	system("pause");
	
}


//int main(int const argc, char const* const argv[])
//{
//
//	gpro_consoleDrawTestPatch();
//	printf("\n\n");
//	system("pause");
//}