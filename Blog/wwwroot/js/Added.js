
function Invite() {
	let a = document.getElementById('userList').value;

	$.ajax({
		type: 'POST',
		url: "/Blog/Invite",
		data: {
			usrId: userList.value
		},
		success: function () {
			console.log("sukces");
		},
		error: function () {

		},
		dataType: "json"
	});

}