let RoomId = "";


var connection = new signalR.HubConnectionBuilder()
	.withUrl("/messageHub")
	.build();


connection.on('receiveMessage', addMessageToChat);

connection.start()
	.catch(error => {
		console.error(error.message);
	});




function setGroup(id) {
	RoomId = id;

	setTimeout(function () {

		console.log("Dolaczylem..");
		connection.invoke("JoinGroup", RoomId.toString());
	}, 500);  
	

	
}



function sendMessageToHub(message) {
	
	
	connection.invoke('SendMessageToGroup', RoomId.toString(), message);
}



