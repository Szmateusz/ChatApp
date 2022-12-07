
function Invite() {

	let SelectUser = document.getElementById('userList');

	let user = SelectUser.value;
	let name = "";

	let list = document.getElementById("usersInGroupList");

	let element = document.createElement('li');

	$.ajax({
		type: 'POST',
		url: "/Blog/Invite",
		data: {
			usrId: userList.value
		},
		success: function (data) {
			name = data.name;
			console.log("sukces");
			element.innerHTML = name;
			list.appendChild(element);
			


		},
		error: function () {
			console.log("niepowodzenie");

		},
		dataType: "json"
	});
	
	



	for (var i = 0; i < SelectUser.length; i++) {
		if (SelectUser.options[i].value == user)
			SelectUser.remove(i);
	}

}

function setGroup(id) {
	RoomId = id;

	setTimeout(function () {

		console.log("Dolaczylem..");
		connection.invoke("JoinGroup", RoomId.toString());
	}, 500);



}