
function Invite() {

	let SelectUser = document.getElementById('userList');

	let user = SelectUser.value;
	let name = "";

	let list = document.getElementById("usersInGroupList");

	let element = document.createElement('li');

	$.ajax({
		type: 'POST',
		url: "/Chat/Invite",
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
	
	
	sendInviteGroupToHub(user);


	for (var i = 0; i < SelectUser.length; i++) {
		if (SelectUser.options[i].value == user)
			SelectUser.remove(i);
	}

}

function addGroupToList(groupName,groupId) {

	console.log("sukces-lista");



	var list = document.getElementById("groupList");

	var li = document.createElement("li");
	var a = document.createElement("a");
	
	a.href = "/Chat/SelectGroup?roomId="+groupId;
	a.innerHTML = groupName;
	//li.innerHTML = group;
	li.className = "nav-link";

	
	li.appendChild(a);
	list.appendChild(li);

}

function leaveFromGroup(userName) {

	connection.invoke("LeaveGroup", RoomId.toString(), userName);

	
	connection.invoke("DeleteUserFromGroup",userName,RoomId.toString());

	window.location.reload();

}

function deleteUserFromList(userName) {

	let list = document.getElementById("usersInGroupList");
	
	let listItems = list.getElementsByTagName('li');

	for (let i = 0; i < listItems.length; i++) {
		if (listItems[i].textContent == userName) {

			listItems[i].remove();
			
		}	
	}
	
}