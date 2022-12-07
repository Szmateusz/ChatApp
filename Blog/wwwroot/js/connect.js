﻿let RoomId = "";


var connection = new signalR.HubConnectionBuilder()
	.withUrl("/messageHub")
	.build();


connection.on('receiveMessage', addMessageToChat);

connection.start()
	.catch(error => {
		console.error(error.message);
	});



function sendMessageToHub(message) {


	connection.invoke('SendMessageToGroup', RoomId.toString(), message);
}



