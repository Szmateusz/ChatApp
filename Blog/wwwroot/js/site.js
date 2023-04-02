
function Invite() {

	let SelectUser = document.getElementById('userList');

	let user = SelectUser.value;

	let list = document.getElementById("usersInGroupList");

	let element = document.createElement('li');
	element.className = "userListBlock userOnUserList";
	let span = document.createElement('span');
	
	let img = document.createElement('img');
	img.alt = "user avatar";
	img.classList.add("avatar_user");

	$.ajax({
		type: 'POST',
		url: "/Chat/Invite",
		data: {
			usrId: userList.value
		},
		success: function (data) {
			console.log("sukces");

			img.src = "/lib/user_avatar/" + data.imgUrl;
			span.innerHTML = data.name;

			element.appendChild(img);
			element.appendChild(span);

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

function addGroupToList(groupName,groupId,imgUrl) {

	console.log("sukces-lista");



	var list = document.getElementById("groupList");

	var li = document.createElement("li");
	var a = document.createElement("a");
	let img = document.createElement('img');
	img.src = "/lib/room_avatar/" + imgUrl;
	img.alt = "room avatar";
	img.classList.add("avatar_room");
	
	a.href = "/Chat/SelectGroup?roomId="+groupId;
	a.innerHTML = groupName;
	//li.innerHTML = group;
	li.className = "nav-link";

	li.appendChild(img);
	li.appendChild(a);
	list.appendChild(li);

}

function leaveFromGroup(userName) {

	connection.invoke("LeaveGroup", RoomId.toString(), userName);

	
	connection.invoke("DeleteUserFromGroup",userName,RoomId.toString());

	window.location.href ="/Chat/SelectGroup?roomId=-1";


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