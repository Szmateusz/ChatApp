
var connection = new signalR.HubConnectionBuilder()
	.withUrl("/messageHub")
	.build();


connection.on('receiveMessage', addMessageToChat);
connection.on('receiveToGroup', addGroupToList);
connection.on('deleteFromGroup',deleteUserFromList);



	connection.start().then(() => {
		console.log("Success.");
		connection.invoke("JoinGroup", RoomId.toString());
	})
	.catch(error => {
			console.error(error.message);
	});




function sendMessageToHub(message) {


	connection.invoke('SendMessageToGroup', RoomId.toString(), message);
}

function sendInviteGroupToHub(user) {

	console.log("TEST" + user + RoomId.toString());
	connection.invoke('InviteToGroup', RoomId, user);
}



